StreamActuatorManager::StreamActuatorManager(Stream& dataStream)
: m_dataStream(dataStream)
{
}

void StreamActuatorManager::notify(ISimulationActuator& actuator, const byte* msg, size_t sz)
{
  int8_t actuatorId = static_cast<int8_t>(actuator.getId());
  m_dataStream.write(actuatorId);
  for (auto i = 0u; i < sz; ++i)
  {
    m_dataStream.write(msg[i]);
  }
}

void StreamActuatorManager::notify(ISimulationActuator& actuator, const char * msg)
{
  int8_t actuatorId = static_cast<int8_t>(actuator.getId());
  m_dataStream.write(actuatorId);
  int i = 0;
  do
  {
    m_dataStream.write(msg[i]);
  } while(msg[i++] != '\0');
}
