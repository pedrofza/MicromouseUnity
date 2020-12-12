#ifndef MEU_DEBUG_H
#define MEU_DEBUG_H

#include "position.h"

void p(const char * s)
{
  Serial.print(s); Serial.send_now(); delay(1); delay(1);
}

void pln(const char * s)
{
  Serial.println(s); Serial.send_now(); delay(1); delay(1);
}

void pln()
{
  Serial.println(); Serial.send_now(); delay(1); delay(1);
}

template <typename T>
void p(const char * t, const T& var)
{
	Serial.print(t); Serial.print(": "); Serial.println(var); Serial.send_now(); delay(1); delay(1);
}


void p(Pose2 pose)
{
  Serial.print("(x, y, theta) = ("); Serial.print(pose.position.x); Serial.print(", "); Serial.print(pose.position.y); Serial.print(", "); Serial.print(rad2deg(pose.angle)); Serial.println(")"); Serial.send_now(); delay(1); delay(1);
}

void p(const State& s)
{
  const char * wsenStr;
  switch(s.orientation)
  {
    case Direction::right:
      wsenStr = "R";
      break;
    case Direction::down:
      wsenStr = "B";
      break;
    case Direction::left:
      wsenStr = "L";
      break;
    case Direction::up:
      wsenStr = "F";
      break;
    default:
      wsenStr = "-";
      break;
  }
  Serial.print("(x, y, r) = ("); Serial.print(s.position.x); Serial.print(", "); Serial.print(s.position.y); Serial.print(", "); Serial.print(wsenStr); Serial.println(")"); Serial.send_now(); delay(1); delay(1);
}

void p(const char * t, const State& state)
{
  p(t); p(state); 
}

void p(RobotMovement m)
{
  Serial.print("Move: ");
  switch(m)
  {
    case RobotMovement::turnLeft:
      Serial.println("L");
      break;
    case RobotMovement::turnRight:
      Serial.println("R");
      break;
    case RobotMovement::forward:
      Serial.println("F");
      break;
    case RobotMovement::halt:
      Serial.println("H");
      break;
  }
  Serial.send_now(); delay(1); delay(1);
}

void p(StateTransition t)
{
  Serial.print("Transition: ");
  switch(t)
  {
    case StateTransition::turnLeft:
      Serial.println("L");
      break;
    case StateTransition::turnRight:
      Serial.println("R");
      break;
    case StateTransition::forward:
      Serial.println("F");
      break;
  }
  Serial.send_now(); delay(1); delay(1);
}

#endif
