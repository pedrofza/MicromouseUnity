template <int N>
StreamSimulationSensorManager<N>::StreamSimulationSensorManager(int id, IActuatorManager& manager, Stream& sensorDataStream)
: SimpleSimulationActuator(id, manager), m_sensors{}, m_sensorDataStream(sensorDataStream)
{ 
}

template <int N>
int StreamSimulationSensorManager<N>::updateSensors()
{
  if (!m_sensorDataStream.available())
  {
    return 0;
  }
  
  int updatedSensorCount = 0;
  while (m_sensorDataStream.available())
  {
    byte targetSensorIdByte = m_sensorDataStream.read();
    int targetSensorId = static_cast<int>(targetSensorIdByte);
    ISimulationSensor* targetSensor = m_sensors[targetSensorId];
    int bytesRead = 0;
    while (bytesRead < targetSensor->totalDataSizeInBytes())
    {
      if (m_sensorDataStream.available())
      {
        m_buffer[bytesRead++] = m_sensorDataStream.read();
      }
    }
    targetSensor->notify(m_buffer, bytesRead);
    updatedSensorCount++;
  }
  return updatedSensorCount;
}

template <int N>
bool StreamSimulationSensorManager<N>::addSensor(ISimulationSensor& sensor)
{
  int arrIndex = sensor.getId();
  if (arrIndex < 0 || arrIndex >= N)
  {
    return false;
  }
  m_sensors[arrIndex] = &sensor;
  return true;
}

template <int N>
void StreamSimulationSensorManager<N>::requestStart()
{
  static constexpr byte REQUEST_START_MESSAGE {0x01};
  m_manager.notify(*this, &REQUEST_START_MESSAGE, 1);
}

template <int N>
void StreamSimulationSensorManager<N>::requestStop()
{
  static constexpr byte REQUEST_STOP_MESSAGE {0x00};
  m_manager.notify(*this, &REQUEST_STOP_MESSAGE, 1);
}

template <int N>
void StreamSimulationSensorManager<N>::requestEnableSensors()
{
  static constexpr byte ENABLE_SENSORS_MESSAGE {0x02};
  m_manager.notify(*this, &ENABLE_SENSORS_MESSAGE, 1);
}

template <int N>
void StreamSimulationSensorManager<N>::requestDisableSensors()
{
   
  static constexpr byte DISABLE_SENSORS_MESSAGE {0x03};
  m_manager.notify(*this, &DISABLE_SENSORS_MESSAGE, 1);
}
