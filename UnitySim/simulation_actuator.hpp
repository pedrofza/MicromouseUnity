SimpleSimulationActuator::SimpleSimulationActuator(int id, IActuatorManager& manager)
: m_id{id}, m_manager(manager)
{
}

int SimpleSimulationActuator::getId() const
{
  return m_id;
}

template <typename T>
MotorActuator<T>::MotorActuator(int id, IActuatorManager& manager)
: SimpleSimulationActuator(id, manager), m_voltage{0}
{
}

SimulationLogger::SimulationLogger(int id, IActuatorManager& manager)
: SimpleSimulationActuator(id, manager)
{
}

void SimulationLogger::log(const char * message)
{
  m_manager.notify(*this, message);
}

template <typename T>
void MotorActuator<T>::setVoltage(T voltage)
{
  if (voltage == m_voltage)
  {
    return;
  }
  m_voltage = voltage;
  T* voltagePointer = &voltage;
  byte* voltageBytePointer = reinterpret_cast<byte*>(voltagePointer);
  m_manager.notify(*this, voltageBytePointer, sizeof(T));
}

template <typename T>
T MotorActuator<T>::getVoltage() const
{
  return m_voltage;
}
