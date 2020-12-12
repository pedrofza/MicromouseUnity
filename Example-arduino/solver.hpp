#include <limits>

StateTransition stateFromTo(State from, State to)
{
  int x1 = from.position.x;
  int y1 = from.position.y;
  Direction r1 = from.orientation;
  int x2 = to.position.x;
  int y2 = to.position.y;
  Direction r2 = to.orientation;
  if (x1 != x2   &&   y1 == y2   &&   r1 == r2)
  {
    Direction r = r1;
    if ((r == Direction::left && x2 == x1 - 1) || (r == Direction::right && x2 == x1 + 1))
    {
      return StateTransition::forward;
    }
  }
  else if (x1 == x2   &&   y1 != y2   &&   r1 == r2)
  {
    Direction r = r1;
    if ((r == Direction::down && y2 == y1 - 1) || (r == Direction::up && y2 == y1 + 1))
    {
      return StateTransition::forward;
    }
  }
  else if (x1 == x2   &&   y1 == y2   &&   r1 != r2)
  {
    if (r2 == ccwDirection(r1)) return StateTransition::turnLeft;
    if (r2 == cwDirection(r1)) return StateTransition::turnRight;
  }
  
  return StateTransition::turnLeft;
};


bool operator==(const State& lhs, const State& rhs)
{
  return lhs.position == rhs.position && lhs.orientation == rhs.orientation;
}

template <int X, int Y>
FFSolver<X, Y>::FFSolver(const IMaze& maze)
: m_maze(maze), m_seeds{false}
{
}

template <int X, int Y>
void FFSolver<X, Y>::addGoal(Vector2Int position)
{
  for (int i = 0; i < 4; ++i)
  {
    m_seeds[position.x][position.y][i] = true;  
  }
}

template <int X, int Y>
void FFSolver<X, Y>::resetGoals()
{
  memset(m_seeds, 0, sizeof(m_seeds));
}

int indexFromDirection(Direction direction)
{
  switch(direction)
  {
    case Direction::left:
      return 0;
      break;
    case Direction::down:
      return 1;
      break;
    case Direction::right:
      return 2;
      break;
    case Direction::up:
      return 3;
      break;
    default:
      return -1;
      break;
  }
}

Direction direction2FromInt(int orientation)
{
  switch(orientation)
  {
    case 0:
      return Direction::left;
      break;
    case 1:
      return Direction::down;
      break;
    case 2:
      return Direction::right;
      break;
    case 3:
      return Direction::up;
      break;
  }
  return Direction::left; //not supposed to happen
}

State stateFromVars(int x, int y, int r)
{
  State s;
  s.position = Vector2Int{.x = x, .y = y};
  s.orientation = direction2FromInt(r);
  return s;
}

template <int X, int Y>
bool FFSolver<X, Y>::isSeed(State state) const
{
  int x = state.position.x;
  int y = state.position.y;
  int r = indexFromDirection(state.orientation);
  return m_seeds[x][y][r];
}

template <int X, int Y>
int FFSolver<X, Y>::cost(State state) const
{
  int x = state.position.x;
  int y = state.position.y;
  int r = indexFromDirection(state.orientation);
  return m_costs[x][y][r];
}


template <int X, int Y>
void FFSolver<X, Y>::calculateCosts()
{
  //resets cost array;
  int initialCost = std::numeric_limits<int>::max();
  for (int i = 0; i < X; ++i)
  {
    for(int j = 0; j < Y; ++j)
    {
      for(int k = 0; k < 4; ++k)
      {
        m_costs[i][j][k] = initialCost;
      }
    }
  }
  
  //assigns seeds costs 0; enqueues their backward neighbors;
  for (int i = 0; i < X; ++i)
  {
    for (int j = 0; j < Y; ++j)
    {
      for (int k = 0; k < 4; ++k)
      {
        if (m_seeds[i][j][k])
        {
          m_costs[i][j][k] = 0;
          State s = stateFromVars(i, j, k);
          etl::vector<State, 3> bwdNeighbors = backwardNeighbors(s);
		  etl::for_each_if(
			bwdNeighbors.begin(),
			bwdNeighbors.end(),
			[this](const State& s) -> void { q.emplace(s); },
			[this](const State& s) -> bool { return !isSeed(s); }
		  );
        }
      }
    }
  }
  // processes all 
  while (!q.empty())
  {
    State elem = q.front();
    q.pop();
    etl::vector<State, 3> fwdNeighs = forwardNeighbors(elem);
	
  	State& first = fwdNeighs[0];
  	auto fCost = cost(first);
  	for(auto n : fwdNeighs)
  	{
  		auto nc = cost(n);
  		if (nc < fCost)
  		{
  			first = n;
  			fCost = nc;
  		}
  	}
  	int lowestForwardCost = fCost;
  	/*
  	auto lowestCostNeighbor = etl::min_element(
  		fwdNeighs.begin(), 
  		fwdNeighs.end(),
  		[this] (const State& a, const State& b) -> bool { return cost(a) < cost(b); }
  	);
  	*/
	  //int lowestForwardCost = cost(*lowestCostNeighbor);
    int candidateSelfCost = lowestForwardCost + 1;
    int currentSelfCost = cost(elem);
    if (candidateSelfCost >= currentSelfCost)
    {
      continue;
    }
    
    setCost(elem, candidateSelfCost);
    etl::vector<State, 3> bwdNeighs = backwardNeighbors(elem);
  	etl::for_each_n(
  		bwdNeighs.begin(),
  		bwdNeighs.size(), 
  		[this](const State& s) -> void { q.emplace(s); }
  	);
  }
}

template <int X, int Y>
void FFSolver<X, Y>::setCost(State s, int cost)
{
  int x = s.position.x;
  int y = s.position.y;
  int r = indexFromDirection(s.orientation);
  m_costs[x][y][r] = cost;
}

template <int X, int Y>
void FFSolver<X, Y>::calculateCostsOptimized(State seedState)
{
  calculateCosts();
}

template <int X, int Y>
State FFSolver<X, Y>::bestMovement(State current) const
{
  etl::vector<State, 3> fwdNeighbors = forwardNeighbors(current);
//  State& first = fwdNeighbors[0];
//	auto fCost = cost(first);
//	for(auto n : fwdNeighbors)
//	{
//		auto nc = cost(n);
//		if (nc < fCost)
//		{
//			first = n;
//			fCost = nc;
//		}
//	}
//	State& lowestCostNeighbor = first;
//	int lowestForwardCost = fCost;
	
	
  auto res = etl::min_element(
	fwdNeighbors.begin(),
	fwdNeighbors.end(),
	[this](const State& a, const State& b) -> bool { return cost(a) < cost(b); }
  );

  return *res;
}

template <int X, int Y>
etl::vector<State, 3> FFSolver<X, Y>::forwardNeighbors(State s) const
{
  etl::vector<State, 3> neighbors;
  State ccwNeighbor {.position = s.position, .orientation = ccwDirection(s.orientation)};
  State cwNeighbor {.position = s.position, .orientation = cwDirection(s.orientation)};

  if (!m_maze.hasWall(s.position, s.orientation))
  {
    State fwdNeighbor = s;
    switch(s.orientation)
    {
      case Direction::left:
        fwdNeighbor.position.x -= 1;
        break;
      case Direction::down:
        fwdNeighbor.position.y -= 1;
        break;
      case Direction::right:
        fwdNeighbor.position.x += 1;
        break;
      case Direction::up:
        fwdNeighbor.position.y += 1;
        break;
    }
    neighbors.push_back(fwdNeighbor);
  }
    neighbors.push_back(ccwNeighbor);
  neighbors.push_back(cwNeighbor);

  return neighbors;
}

template <int X, int Y>
etl::vector<State, 3> FFSolver<X, Y>::backwardNeighbors(State s) const
{
  etl::vector<State, 3> neighbors;
  State ccwNeighbor {.position = s.position, .orientation = ccwDirection(s.orientation)};
  State cwNeighbor {.position = s.position, .orientation = cwDirection(s.orientation)};
  neighbors.push_back(ccwNeighbor);
  neighbors.push_back(cwNeighbor);
  
  if (!m_maze.hasWall(s.position, oppositeDirection(s.orientation)))
  {
    State bwdNeighbor = s;
    switch(s.orientation)
    {
      case Direction::left:
        bwdNeighbor.position.x += 1;
        break;
      case Direction::down:
        bwdNeighbor.position.y += 1;
        break;
      case Direction::right:
        bwdNeighbor.position.x -= 1;
        break;
      case Direction::up:
        bwdNeighbor.position.y -= 1;
        break;
    }
    neighbors.push_back(bwdNeighbor);
  }
  
  return neighbors;
}

template <int X, int Y>
void FFSolver<X, Y>::setGoalToHome()
{
	resetGoals();
	addGoal(m_maze.homePosition());
}

template <int X, int Y>
void FFSolver<X, Y>::setGoalToMazeGoal()
{
	resetGoals();
	etl::array_view<const Vector2Int> goals{m_maze.goalPositions()};
	for(const Vector2Int& goal : goals)
	{
		addGoal(goal);
	}
}


template <int X, int Y>
PathManager<X, Y>::PathManager(const IMaze& maze)
: m_solver(maze), m_maze(maze), m_goingToCenter{false}, m_robotState{.position= {.x = 0, .y = 0}, .orientation= Direction::up}
{
}


template <int X, int Y>
StateTransition PathManager<X, Y>::nextMovement()
{
  if (m_goingToCenter && isGoal(m_robotState))
  {
    m_goingToCenter = false;
    m_solver.setGoalToHome();
//    m_solver.calculateCosts();
  }
  else if (!m_goingToCenter && isHome(m_robotState))
  {
    m_goingToCenter = true;
	m_solver.setGoalToMazeGoal();
//    m_solver.calculateCosts();
  }
  m_solver.calculateCosts();
  State nextState = m_solver.bestMovement(m_robotState);
  StateTransition nextMovement = stateFromTo(m_robotState, nextState);
  m_robotState = nextState;
//  p(nextMovement);
  return nextMovement;
}

template <int X, int Y>
bool PathManager<X, Y>::isGoal(State s) const
{
  return m_solver.isSeed(s);
}

template <int X, int Y>
bool PathManager<X, Y>::isHome(State s) const
{
  static Vector2Int home = Vector2Int{.x = 0, .y = 0};
  return s.position == home;
}

template <int X, int Y>
void PathManager<X, Y>::updatedWalls()
{  
}

template <int X, int Y>
State PathManager<X, Y>::getRobotState() const
{
  return m_robotState;
}
