#ifndef SOLVER_H
#define SOLVER_H

#include "maze.h"
#include "etl/vector.h"
#include "etl/queue.h"
#include "etl/array_view.h"
#include "position.h"

struct State
{
  Vector2Int position;
  Direction orientation;
};

bool operator==(const State& lhs, const State& rhs);

enum class StateTransition
{
	forward, turnLeft, turnRight
};

StateTransition stateFromTo(State from, State to);

template <int X, int Y>
class FFSolver
{
  private:
  const IMaze& m_maze;
  bool m_seeds[X][Y][4];
  int m_costs[X][Y][4];
  etl::queue<State, X * Y * 4> q;

  void setCost(State s, int cost);
  void enqueueBackwardNeighborsIfNotSeed(State s);
  etl::vector<State, 3> backwardNeighbors(State s) const;
  etl::vector<State, 3> forwardNeighbors(State s) const;
  
  public:
  FFSolver(const IMaze& maze);
  void addGoal(Vector2Int position);
  void setGoalToHome();
  void setGoalToMazeGoal();
  void resetGoals();
  bool isSeed(State s) const;
  int cost(State s) const;
  void calculateCosts();
  void calculateCostsOptimized(State seedState);
  State bestMovement(State current) const;
};

template <int X, int Y>
class PathManager
{
  private:
  FFSolver<X, Y> m_solver;
  const IMaze& m_maze;
  bool m_goingToCenter;
  State m_robotState;
  bool isGoal(State s) const;
  bool isHome(State s) const;
  
  public:
  PathManager(const IMaze& maze);
  StateTransition nextMovement();
  void updatedWalls();
  State getRobotState() const;
};


#include "solver.hpp"

#endif
