SimpleRobotPoseManager::SimpleRobotPoseManager(const RobotConstructionSpecification& specs)
: m_specs(specs), m_pose{0, 0, 0}, m_calc{m_specs}
{
}

void SimpleRobotPoseManager::updatePose(int leftWheelPulses, int rightWheelPulses)
{
  Displacement d = m_calc.getDisplacement(leftWheelPulses, rightWheelPulses);

  m_pose.angle += d.angular;
  m_pose.position.x += d.linear * cos(m_pose.angle);
  m_pose.position.y += d.linear * sin(m_pose.angle);
}

//void SimpleRobotPoseManager::updatePose(int leftWheelPulses, int rightWheelPulses, unsigned long deltaTime)
//{
//  Velocity v = m_calc.getVelocity(leftWheelPulses, rightWheelPulses, deltaTime);
//  m_pose.theta += v.angular * deltaTime;
//  m_pose.x += v.linear * cos(m_pose.theta) * deltaTime;
//  m_pose.y += v.linear * sin(m_pose.theta) * deltaTime;
//}

Pose2 SimpleRobotPoseManager::getPose() const
{
  return m_pose;
}

void SimpleRobotPoseManager::setPose(Pose2 pose)
{
  m_pose = pose;
}

float SimpleRobotPoseManager::deltaDistance() const
{
  return m_pose.position.magnitude();
  //return sqrt(m_pose.position.x * m_pose.position.x + m_pose.position.y * m_pose.position.y);
}


template <typename T>
MotorManager<T>::MotorManager(IMotor<T>& motor)
: m_motor{motor}, m_duration{0}
{
}

template <typename T>
void MotorManager<T>::setPoint(T setPoint, unsigned long duration)
{
  m_setPoint = setPoint;
  m_duration = duration;
  m_totalElapsedTime = 0;
  m_startingVoltage = m_motor.getVoltage();
}

template <typename T>
void MotorManager<T>::update(unsigned long elapsedTime)
{
  m_totalElapsedTime += elapsedTime;
  if (m_totalElapsedTime >= m_duration)
  {
    m_motor.setVoltage(m_setPoint);
    return;
  }
  
  float progress = static_cast<float>(m_totalElapsedTime) / m_duration;
  
  T totalVoltageChange = m_setPoint - m_startingVoltage; //variable could be cached
  T addedVoltage = progress * totalVoltageChange;
  m_motor.setVoltage(m_startingVoltage + addedVoltage);
}

template <typename T>
MotorManager2<T>::MotorManager2(IMotor<T>& motor, float voltageVariationRate)
: m_motor(motor), m_rate{voltageVariationRate}
{
}

template <typename T>
void MotorManager2<T>::setPoint(T setPoint)
{
  m_setPoint = setPoint;
  m_startingVoltage = m_motor.getVoltage();
  m_totalElapsedTime = 0;
}

template <typename T>
void MotorManager2<T>::update(unsigned long elapsedTime)
{
  if (m_motor.getVoltage() == m_setPoint)
  {
    return;
  }
  
  m_totalElapsedTime += elapsedTime;
  long long change = static_cast<long long>(m_rate * m_totalElapsedTime);
  
  if (m_motor.getVoltage() < m_setPoint)
  {
    long long candidate = m_startingVoltage + change;
    if (candidate > m_setPoint)
    {
      m_motor.setVoltage(m_setPoint);
      return;
    }
    m_motor.setVoltage(static_cast<T>(candidate));
  }

  else
  {
    long long candidate = static_cast<long long>(m_startingVoltage - change);
    if (candidate < m_setPoint)
    {
      m_motor.setVoltage(m_setPoint);
      return;
    }
    m_motor.setVoltage(static_cast<T>(candidate));
  }
}

SimpleVelocityFromPulsesCalculator::SimpleVelocityFromPulsesCalculator(const RobotConstructionSpecification& specs)
: m_specs(specs)
{
}

//Velocity SimpleVelocityFromPulsesCalculator::getVelocity(int leftWheelPulses, int rightWheelPulses, unsigned long deltaTime)
//{
//  float leftWheelAngularSpeed = leftWheelPulses * 2 * PI / (m_specs.leftWheelPPR * deltaTime);
//  float rightWheelAngularSpeed = rightWheelPulses * 2 * PI / (m_specs.rightWheelPPR * deltaTime);
//
//  float leftWheelLinearSpeed = leftWheelAngularSpeed * m_specs.leftWheelRadius;
//  float rightWheelLinearSpeed = rightWheelAngularSpeed * m_specs.rightWheelRadius;
//
//  float robotLinearSpeed = (rightWheelLinearSpeed + leftWheelLinearSpeed) / 2;
//  float robotAngularSpeed = (rightWheelLinearSpeed - leftWheelLinearSpeed) / m_specs.wheelBaseDistance;
//  
//  return Velocity{.linear=robotLinearSpeed, .angular=robotAngularSpeed};
//}

Displacement SimpleVelocityFromPulsesCalculator::getDisplacement(int leftWheelPulses, int rightWheelPulses)
{
  float leftWheelAngularDisplacement = leftWheelPulses * 2 * PI / m_specs.leftWheelPPR;
  float rightWheelAngularDisplacement = rightWheelPulses * 2 * PI / m_specs.rightWheelPPR;

  float leftWheelLinearDisplacement = leftWheelAngularDisplacement * m_specs.leftWheelRadius;
  float rightWheelLinearDisplacement = rightWheelAngularDisplacement * m_specs.rightWheelRadius;

  float robotLinearDisplacement = (rightWheelLinearDisplacement + leftWheelLinearDisplacement) / 2;
  float robotAngularDisplacement = (rightWheelLinearDisplacement - leftWheelLinearDisplacement) / m_specs.wheelBaseDistance;
  
  return Displacement{.linear=robotLinearDisplacement, .angular=robotAngularDisplacement};
}
