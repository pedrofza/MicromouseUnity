#ifndef MAZE_UTILS_H
#define MAZE_UTILS_H

#include "maze.h"

static constexpr Vector2 CLASSIC_MICROMOUSE_MAZE_SIZE = Vector2{0.18, 0.18};

Maze<16, 16, 4> classicMicromouseMaze();

template <int X, int Y, int N>
void surroundMazeWithWalls(Maze<X, Y, N>& maze);


#include "maze_utils.hpp"

#endif
