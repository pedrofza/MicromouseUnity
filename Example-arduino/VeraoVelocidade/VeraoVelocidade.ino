#include <sim_robot.h>

unsigned long odometryUpdateTime = 20; // In millisseconds.
constexpr float goalLinearSpeed = 0.8f;
constexpr float goalAngularSpeed = 1.57;

constexpr float linearGain = 10.0f;
constexpr float angularGain = 1.0f;

float angularOffset = 0.0;
float baseVoltage = 0.0;

SimRobot sr;

void setup() {
  Serial.begin(115200);
  sr.initialize();
}

void loop()
{
  sr.update();
  sr.delay(odometryUpdateTime);

  int leftEncoderCounts = sr.leftEncoder();
  int rightEncoderCounts = sr.rightEncoder();
  
  float leftDisplacement = countsToLinearDisplacement(leftEncoderCounts, sr.WHEEL_RADIUS, sr.GEAR_RATIO, sr.ENCODER_PPR);
  float rightDisplacement = countsToLinearDisplacement(rightEncoderCounts, sr.WHEEL_RADIUS, sr.GEAR_RATIO, sr.ENCODER_PPR);

  float deltaTimeInSeconds = (float)odometryUpdateTime / 1000;
  float leftWheelLinSpeed = leftDisplacement / deltaTimeInSeconds;
  float rightWheelLinSpeed = rightDisplacement / deltaTimeInSeconds;
  float linSpeed = calculateLinearSpeed(leftWheelLinSpeed, rightWheelLinSpeed);
  float angSpeed = calculateAngularSpeed(leftWheelLinSpeed, rightWheelLinSpeed, sr.WHEEL_TO_WHEEL);
  
  baseVoltage += (goalLinearSpeed - linSpeed) * linearGain;
  angularOffset += (goalAngularSpeed - angSpeed) * angularGain;
    
  sr.leftMotor(baseVoltage + angularOffset);
  sr.rightMotor(baseVoltage - angularOffset);
}

float calculateAngularSpeed(float leftWheelSpeed, float rightWheelSpeed, float wheelToWheel)
{
  return (leftWheelSpeed - rightWheelSpeed) / wheelToWheel;
}

float calculateLinearSpeed(float leftWheelSpeed, float rightWheelSpeed)
{
  return (leftWheelSpeed + rightWheelSpeed) / 2.0f;
}

float countsToLinearDisplacement(int counts, float wheelRadius, float gearRatio, int encoderPPR)
{
  return (2 * PI * wheelRadius * counts) / (gearRatio * encoderPPR);
}
