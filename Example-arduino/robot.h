#ifndef ROBOT_H
#define ROBOT_H

#include "position.h"
#include "interface_defs.h"
#include "robot_specs.h"

struct Displacement
{
  float linear;
  float angular;
};

struct Velocity
{
  float linear;
  float angular;
};

struct MazeSpecification
{
  float cellWallSize = 16.8 / 100;
  int sizeX = 16;
  int sizeY = 16;
};

struct BoundingBoxToTurningAxis
{
  float leftToAxis;
  float rightToAxis;
  float frontToAxis;
  float backToAxis;
};

class ICalculateVelocityFromPulses
{
  public:
  virtual Velocity getVelocity(int leftWheelPulses, int rightWheelPulses, unsigned long deltaTime) = 0;
};

class ICalculateDisplacementFromPulses
{
  public:
  virtual Displacement getDisplacement(int leftWheelPulses, int rightWheelPulses) = 0;
};

class SimpleVelocityFromPulsesCalculator : public ICalculateDisplacementFromPulses
{
  private:
  const RobotConstructionSpecification& m_specs;
  
  public:
  SimpleVelocityFromPulsesCalculator(const RobotConstructionSpecification& specs);
//  Velocity getVelocity(int leftWheelPulses, int rightWheelPulses, unsigned long deltaTime) override;
  Displacement getDisplacement(int leftWheelPulses, int rightWheelPulses) override;
};

class SimpleRobotPoseManager
{
  private:
  const RobotConstructionSpecification& m_specs;
  Pose2 m_pose;
  SimpleVelocityFromPulsesCalculator m_calc;
  
  public:
  SimpleRobotPoseManager(const RobotConstructionSpecification& specs);
//  void updatePose(int leftWheelPulses, int rightWheelPulses, unsigned long deltaTime);
  void updatePose(int leftWheelPulses, int rightWheelPulses);
  Pose2 getPose() const;
  void setPose(Pose2 pose);
  float deltaDistance() const;
};

template <typename T>
class MotorManager
{
  private:
  IMotor<T>& m_motor;
  unsigned long m_duration;
  unsigned long m_totalElapsedTime;
  
  T m_setPoint;
  T m_startingVoltage;
  
  public:
  MotorManager(IMotor<T>& motor);
  void setPoint(T setPoint, unsigned long duration);
  void update(unsigned long elapsedTime);
};

template <typename T>
class MotorManager2
{
  private:
  IMotor<T>& m_motor;
  
  T m_setPoint;
  T m_startingVoltage;
  unsigned long m_totalElapsedTime;
  float m_rate;
  
  public:
  MotorManager2(IMotor<T>& motor, float voltageVariationRate);
  void setPoint(T setPoint);
  void update(unsigned long elapsedTime);
};

enum class RobotMovement
{
  forward, turnLeft, turnRight, halt
};

#include "robot.hpp"

#endif
