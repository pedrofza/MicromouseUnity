#ifndef SIM_ROBOT_H
#define SIM_ROBOT_H

#include "Arduino.h"
#include "simulation.h"
#include "robot_specs.h"

class SimRobot
{
  private:
  StreamActuatorManager _actuatorManager{Serial};
  StreamSimulationSensorManager<5> _sensorManager{7, _actuatorManager, Serial};
  DistanceSensor<uint16_t, 2> _tofLeft{0, _sensorManager, 1.0, 16};
  DistanceSensor<uint16_t, 2> _tofRight{1, _sensorManager, 1.0, 16};
  DistanceSensor<uint16_t, 2> _tofFront{2, _sensorManager, 1.0, 16};
  EncoderSensor<int16_t, 2> _encoderLeft{3, _sensorManager};
  EncoderSensor<int16_t, 2> _encoderRight{4, _sensorManager};
  MotorActuator<int16_t> _motorLeft{5, _actuatorManager};
  MotorActuator<int16_t> _motorRight{6, _actuatorManager};
  SimulationLogger _logger{8, _actuatorManager};
  
  public:
  static constexpr float GEAR_RATIO = 29.86;
  static constexpr int ENCODER_PPR = 12;
  static constexpr float WHEEL_RADIUS = 0.016f;
  static constexpr float WHEEL_TO_WHEEL = 0.096f;
  static constexpr RobotConstructionSpecification specs
  {
	.leftWheelPPR = 29.86 * 12,
	.rightWheelPPR = 29.86 * 12,
	.leftWheelRadius = 0.016,
	.rightWheelRadius = 0.016,
	.wheelBaseDistance = 0.096f
  };
  void initialize();
  void update();
  void log(const char * message);
  void delay(unsigned long ms);
  float leftDistance();
  float frontDistance();
  float rightDistance();
  int leftEncoder();
  int rightEncoder();
  void leftMotor(int counts);
  void rightMotor(int counts);
};

#include "sim_robot.hpp"

#endif
