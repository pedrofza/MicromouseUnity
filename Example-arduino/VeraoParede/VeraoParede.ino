#include <sim_robot.h>

SimRobot sr;

void setup() {
  Serial.begin(115200);
  sr.initialize();
}

float goalWallDistance = 0.5f;
float goalMeasurement = (goalWallDistance - sr.WHEEL_TO_WHEEL / 2) * sqrt(2);

float k = 50.0f;
int baseVoltage = 300;

void loop()
{
  sr.update();
  float leftDistance = sr.leftDistance();
  float error = goalMeasurement - leftDistance;
  sr.leftMotor(baseVoltage + k * error);
  sr.rightMotor(baseVoltage - k * error);
}
