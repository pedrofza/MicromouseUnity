#ifndef ANGLE_ULTILS_H
#define ANGLE_ULTILS_H

// returns b - a, constrained to [-PI, PI]
float signedAngleDifference(float a, float b);

//returns angle constrained to [0, 2*PI[
float angleNormalize(float angle);

float rad2deg(float rad);
float deg2rad(float deg);

#include "angle_utils.hpp"

#endif
