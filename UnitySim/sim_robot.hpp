void SimRobot::initialize()
{
  _sensorManager.addSensor(_tofLeft);
  _sensorManager.addSensor(_tofRight);
  _sensorManager.addSensor(_tofFront);
  _sensorManager.addSensor(_encoderLeft);
  _sensorManager.addSensor(_encoderRight);
  _sensorManager.requestEnableSensors();
  _tofLeft.waitForNewData();
  _tofRight.waitForNewData();
  _tofFront.waitForNewData();
}

void SimRobot::update()
{
  _sensorManager.updateSensors();
}

void SimRobot::log(const char * message)
{
	_logger.log(message);
}

void SimRobot::delay(unsigned long ms)
{
	unsigned long started = millis();
	while (millis() - started < ms)
	{
		update();
	}
}


float SimRobot::leftDistance()
{
  return _tofLeft.distance();
}

float SimRobot::frontDistance()
{
  return _tofFront.distance();
}

float SimRobot::rightDistance()
{
  return _tofRight.distance();
}

int SimRobot::leftEncoder()
{
  return _encoderLeft.readReset();
}

int SimRobot::rightEncoder()
{
  return _encoderRight.readReset();
}

void SimRobot::leftMotor(int counts)
{
  _motorLeft.setVoltage(counts);
}

void SimRobot::rightMotor(int counts)
{
  _motorRight.setVoltage(counts);
}
