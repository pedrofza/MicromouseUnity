#ifndef MATH_UTILS_H
#define MATH_UTILS_H

#include "position.h"

Vector2 rayCast(Vector2 origin, float angle, float length);

bool isWithinRectangle(Vector2 point, Vector2 recSize);
bool isWithinRectangle(Vector2 point, Vector2 recCenter, Vector2 recSize);
bool isWithinCircle(Vector2 point, float radius);


#include "math_utils.hpp"

#endif