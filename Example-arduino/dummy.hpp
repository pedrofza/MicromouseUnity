void DummyMotor::setVoltage(int16_t voltage)
{
  v = voltage;
}

int16_t DummyMotor::getVoltage() const
{
  return v;
}


DummySolver::DummySolver()
: m_move{0}
{
}

RobotMovement DummySolver::nextMove()
{
  RobotMovement m;
  switch(m_move)
  {
    case 0:
    case 1:
    case 2:
    
    case 4:
    
    case 6:
    case 7:

    case 9:

    case 11:

    case 13:
    case 14:
    case 15:
    case 16:
    case 17:
    case 18:
    case 19:
    case 20:
    case 21:
    case 22:
    case 23:
    case 24:
    case 25:
    
    
      m = RobotMovement::forward;
      break;
    case 3:
    
    case 5:

    case 10:
      m = RobotMovement::turnRight;
      break;
      
    case 8:

    case 12:
      m = RobotMovement::turnLeft;
      break;
    default:
      m = RobotMovement::halt;
      break;
  }
  m_move++;
  return m;
}

void DummySolver::update()
{
  m_move++;
}
