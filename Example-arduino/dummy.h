#ifndef DUMMY_H
#define DUMMY_H

class DummyMotor : public IMotor<int16_t>
{
  public:
  int16_t v;
  void setVoltage(int16_t voltage) override;
  int16_t getVoltage() const;
};

class DummySolver
{
  private:
  int m_move;
  
  public:
  DummySolver();
  RobotMovement nextMove();
  void update();
};

#include "dummy.hpp"

#endif
