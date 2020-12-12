#include "bitwise_utils.h"

template <typename T, int N>
T SimpleSimulationSensor<T, N>::getNewData()
{
  while (!hasNewData())
  {
    m_sensorManager.updateSensors();
  }
  m_hasNewData = false;
  return m_data;
}

template <typename T, int N>
void SimpleSimulationSensor<T, N>::waitForNewData()
{
  while (!hasNewData())
  {
    m_sensorManager.updateSensors();
  }
}

template <typename T, int N>
SimpleSimulationSensor<T, N>::SimpleSimulationSensor(int id, ISimulationSensorManager& sensorManager)
: m_id{id}, m_sensorManager(sensorManager), m_hasNewData{false}, m_data{}
{
}

template <typename T, int N>
int SimpleSimulationSensor<T, N>::getId() const
{
  return m_id;
}

template <typename T, int N>
bool SimpleSimulationSensor<T, N>::interpretByteArray(byte* bytes, int count, T& result) const
{
  if (count != N)
  {
    return false;
  }
  
  T data{};
  for (auto i = 0; i < count; ++i)
  {
    data |= bytes[i] << (8 * i);
  }
  result = data;
  return true;
}

template <typename T, int N>
void SimpleSimulationSensor<T, N>::notify(byte* newDataBytes, int count)
{
  T newData{};
  this->interpretByteArray(newDataBytes, count, newData);
  updateData(newData);
}


template <typename T, int N>
int SimpleSimulationSensor<T, N>::totalDataSizeInBytes() const
{
  return N;
}

template <typename T, int N>
bool SimpleSimulationSensor<T, N>::hasNewData() const
{
  return m_hasNewData;
}

template <typename T, int N>
void SimpleSimulationSensor<T, N>::updateData(T newData)
{
  m_data = newData;
  m_hasNewData = true;
}

template <typename T, int N>
DistanceSensor<T, N>::DistanceSensor(int id, ISimulationSensorManager& sensorManager, float maxDistance, int bits)
: SimpleSimulationSensor<T, N>(id, sensorManager), m_maxDistance(maxDistance), m_maxMeasurementBits(genMask(bits))
{
}

template <typename T, int N>
float DistanceSensor<T, N>::distance() const
{
  return this->m_data * m_maxDistance / m_maxMeasurementBits;
}

template <typename T, int N>
EncoderSensor<T, N>::EncoderSensor(int id, ISimulationSensorManager& sensorManager)
: SimpleSimulationSensor<T, N>(id, sensorManager)
{
}

template <typename T, int N>
void EncoderSensor<T, N>::notify(byte* newDataBytes, int count)
{
  T newData{};
  this->interpretByteArray(newDataBytes, count, newData);
  this->updateData(this->m_data + newData);
}

template <typename T, int N>
T EncoderSensor<T, N>::getNewData()
{
  return this->m_data;
}

template <typename T, int N>
T EncoderSensor<T, N>::readReset()
{
  T tmp = this->m_data;
  reset();
  return tmp;
}

template <typename T, int N>
void EncoderSensor<T, N>::reset()
{
  this->m_data = T{};
}

template <typename T, int N>
void EncoderSensor<T, N>::updateData(T newData)
{
  if (newData != T{})
  {
    this->m_hasNewData = true;
  }
  this->m_data = newData;
}
