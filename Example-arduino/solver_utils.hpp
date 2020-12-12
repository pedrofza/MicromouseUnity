Pose2 stateToPose(State s)
{
  Pose2 pose;
  pose.position.x = s.position.x * 0.18;
  pose.position.y = s.position.y * 0.18;
  double angle;
  switch(s.orientation)
  {
    case Direction::left:
      angle = PI;
      break;
    case Direction::down:
      angle = 3 * PI / 2;
      break;
    case Direction::right:
      angle = 0;
      break;
    case Direction::up:
      angle = PI / 2;
      break;
    default:
      angle = 0;
      break;
  }
  pose.angle = angle;
  return pose;
}