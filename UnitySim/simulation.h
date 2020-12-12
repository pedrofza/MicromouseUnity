#ifndef SIMULATION_SENSOR_MANAGER_H
#define SIMULATION_SENSOR_MANAGER_H

#include "interface_defs.h"

class ISimulationSensorManager
{
  public:
  virtual int updateSensors() = 0;
};

class ISimulationActuator
{
  public:
  virtual int getId() const = 0;
};

class IActuatorManager
{
  public:
  virtual void notify(ISimulationActuator& actuator, const byte* msg, size_t sz) = 0;
  virtual void notify(ISimulationActuator& actuator, const char * msg) = 0;
};

class ISimulationSensor
{
  public:
  virtual void notify(byte* newDataBytes, int count) = 0;
  virtual int totalDataSizeInBytes() const = 0;
  virtual int getId() const = 0;
};

class SimpleSimulationActuator : public ISimulationActuator
{
  protected:
  int m_id;
  IActuatorManager& m_manager;
  
  public:
  int getId() const override;
  SimpleSimulationActuator(int id, IActuatorManager& manager);
};


template <int N>
class StreamSimulationSensorManager : public SimpleSimulationActuator, public ISimulationSensorManager
{
  private:
  ISimulationSensor* m_sensors[N];
  Stream& m_sensorDataStream;
  byte m_buffer[256];
  
  public:
  StreamSimulationSensorManager(int id, IActuatorManager& manager, Stream& sensorDataStream);
  int updateSensors() override;
  bool addSensor(ISimulationSensor& sensor);
  void requestStart();
  void requestStop();
  void requestEnableSensors();
  void requestDisableSensors();
};


template <typename T, int N>
class SimpleSimulationSensor : public ISimulationSensor
{
  protected:
  int m_id;
  ISimulationSensorManager& m_sensorManager;
  bool m_hasNewData;
  T m_data;
  virtual void updateData(T newData);
  
  public:
  SimpleSimulationSensor(int id, ISimulationSensorManager& sensorManager);
  int getId() const override;
  virtual void notify(byte* newDataBytes, int count) override;
  virtual bool interpretByteArray(byte* bytes, int count, T& result) const;
  int totalDataSizeInBytes() const override;
  bool hasNewData() const;
  void waitForNewData();
  virtual T getNewData();
};

template<typename T, int N>
class DistanceSensor : public SimpleSimulationSensor<T, N>, IDistanceSensor
{
  public:
  float m_maxDistance;
  unsigned int m_maxMeasurementBits;
  
  public:
  DistanceSensor(int id, ISimulationSensorManager& sensorManager, float maxDistance, int bits);
  float distance() const override;
};

template<typename T, int N>
class EncoderSensor : public SimpleSimulationSensor<T, N>
{
  public:
  EncoderSensor(int id, ISimulationSensorManager& sensorManager);
  void notify(byte* newDataBytes, int count) override;
  T getNewData() override;
  void reset();
  T readReset();
  protected:
  void updateData(T newData) override;
};

template <typename T>
class MotorActuator : public SimpleSimulationActuator, public IMotor<T>
{
  private:
  volatile T m_voltage;
  
  public:
  MotorActuator(int id, IActuatorManager& manager);
  void setVoltage(T voltage);
  T getVoltage() const;
};

class SimulationLogger : public SimpleSimulationActuator
{
  public:
  SimulationLogger(int id, IActuatorManager& manager);
  void log(const char * message);
};

class StreamActuatorManager : public IActuatorManager
{
  protected:
  Stream& m_dataStream;
  
  public:
  StreamActuatorManager(Stream& dataStream);
  void notify(ISimulationActuator& actuator, const byte* msg, size_t sz) override;
  void notify(ISimulationActuator& actuator, const char * msg) override;
};


#include "simulation_sensor.hpp"
#include "simulation_sensor_manager.hpp"
#include "simulation_actuator.hpp"
#include "simulation_actuator_manager.hpp"

#endif
