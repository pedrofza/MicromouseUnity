#include <sim_robot.h>

SimRobot sr;

void setup() {
  Serial.begin(115200);
  sr.initialize();
}

float goalWallDistance = 0.5f;
float goalMeasurement = (goalWallDistance - sr.WHEEL_TO_WHEEL / 2) * sqrt(2);

float k = 300.0f;
int baseVoltage = 900;

void loop()
{
  sr.update();
  float rightDistance = sr.rightDistance();
  float error = goalMeasurement - rightDistance;
  sr.leftMotor(baseVoltage - k * error);
  sr.rightMotor(baseVoltage);
}
