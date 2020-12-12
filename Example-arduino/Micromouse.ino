// Chrono v1.2.0 https://github.com/SofaPirate/Chrono
#include <Chrono.h> 
#include <LightChrono.h>

#include "robot.h"
#include <sim_robot.h>

#include "solver.h"
#include "board.h"
#include "angle_utils.h"
#include "maze_utils.h"
#include "solver_utils.h"
#include "math_utils.h"

SimRobot sr;

// Robot Wheel specifications (used for odometry)
constexpr RobotConstructionSpecification robotSpecs = sr.specs;

SimpleRobotPoseManager poseManager(robotSpecs);

constexpr unsigned long odometryUpdateTime = 1; // In ms.

Chrono odometryChrono(Chrono::MILLIS, false);

//Maze and solvers
Maze<16, 16, 4> maze {classicMicromouseMaze()};
PathManager<16, 16> pm(maze);

Pose2 startingPose;
Pose2 goalPose;

void setup() {
  // Setup sensors (for simulation communication purposes)
  Serial.begin(115200);
  poseManager.setPose({0, 0, PI/2});
  startingPose = poseManager.getPose();
  Board::init();
  Board::ledOn();
  sr.initialize();
  odometryChrono.start(odometryUpdateTime);
}

void updateOdometryOnTimer()
{
  if (odometryChrono.hasPassed(odometryUpdateTime, true))
  {
    int16_t leftWheelPulses = sr.leftEncoder();
    int16_t rightWheelPulses = sr.rightEncoder();
    poseManager.updatePose(leftWheelPulses, rightWheelPulses);
  }
}

bool awaitingNextMovement = true;
StateTransition movement;

int16_t fwdSpeed = 250;
int16_t turningSpeed = 122;
float k = 1300;

bool didRead = false;

float leftMeasurement;
float rightMeasurement;
float frontMeasurement;

Pose2 currentPose;

State currentMouseState;
Pose2 currentIdealPose;

State goalMouseState;
Pose2 goalIdealPose;

bool wasForward = false;
bool shouldUpdate = false;

void loop() {
  // Updating sensors (serial input). This command processes the simulation output.
  sr.update();
  
  updateOdometryOnTimer();
  
  if (awaitingNextMovement)
  {
    // Current mouse position
    currentMouseState = pm.getRobotState();
    currentIdealPose = stateToPose(currentMouseState);
    if (movement == StateTransition::forward)
    {
      wasForward = true;
      shouldUpdate = true;
    }
    movement = pm.nextMovement();
//    Board::ledToggle();
    goalMouseState = pm.getRobotState();
    goalIdealPose = stateToPose(goalMouseState);
    
    awaitingNextMovement = false;
    didRead = false;
    //Correct one axis of pose
    if (shouldUpdate && wasForward)
    {
      shouldUpdate = false;
      currentPose = poseManager.getPose();
      switch (currentMouseState.orientation)
      {
        case Direction::up:
        case Direction::down:
          currentPose.position.x = currentIdealPose.position.x;
        break;
        case Direction::right:
        case Direction::left:
          currentPose.position.y = currentIdealPose.position.y;
        break;
      }
      currentPose.angle = currentIdealPose.angle;
      poseManager.setPose(currentPose);
    }
//    Board::ledToggle();
  }
  currentPose = poseManager.getPose();
  Pose2 deltaPoseEnd = goalIdealPose - currentPose;
  float dap = signedAngleDifference(currentIdealPose.normalizedAngle(), currentPose.normalizedAngle());
  
  leftMeasurement = sr.leftDistance();
  rightMeasurement = sr.rightDistance();
  frontMeasurement = sr.frontDistance();
  
  float error = 0;
  if( leftMeasurement < 0.1 && rightMeasurement < 0.1 )
  {
    error = leftMeasurement - rightMeasurement;
  }
  else
  {
    if (leftMeasurement < rightMeasurement && leftMeasurement < 0.1)
    {
     error = leftMeasurement - 0.050912;
    }
    
    else if (rightMeasurement < leftMeasurement && rightMeasurement < 0.1)
    {
      error = 0.050912 - rightMeasurement;
    }
  }

  float dist = 0;

  switch (movement)
  {
    case StateTransition::forward:
      switch(currentMouseState.orientation)
      {
        case Direction::up:
        case Direction::down:
          dist = abs(deltaPoseEnd.position.y);
        break;
        case Direction::right:
        case Direction::left:
          dist = abs(deltaPoseEnd.position.x);
        break;
      }
      sr.leftMotor(fwdSpeed - k * error);
      sr.rightMotor(fwdSpeed + k * error);

      if(dist <= 0.023)
      {
        awaitingNextMovement = true;
      }
      
      if (frontMeasurement <= 0.036 + 0.01)
      {
        awaitingNextMovement = true;
        Board::ledToggle();
      }

      //Detect walls when entering the cell
      if (!didRead && dist >= 0.10 && dist <= .12)
      {
        didRead = true;
        if(leftMeasurement < .1)
        {
          maze.placeWall(goalMouseState.position, localToGlobalDirection(goalMouseState.orientation, Direction::left));
        }
        
        if (rightMeasurement < .1)
        {
           maze.placeWall(goalMouseState.position, localToGlobalDirection(goalMouseState.orientation, Direction::right));
        }

        if(frontMeasurement < .25)
        {
          maze.placeWall(goalMouseState.position, localToGlobalDirection(goalMouseState.orientation, Direction::up));
        }
      }
      
      break;
      
    case StateTransition::turnLeft:
      sr.leftMotor(-turningSpeed);
      sr.rightMotor(turningSpeed);
      if (dap >= PI / 2 - PI/14)
      {
        awaitingNextMovement = true;
      }
      break;
      
    case StateTransition::turnRight:
      sr.leftMotor(turningSpeed);
      sr.rightMotor(-turningSpeed);
      if (dap <= - PI / 2 + PI/14)
      {
        awaitingNextMovement = true;
      }
      break;
      
    default:
      sr.leftMotor(0);
      sr.rightMotor(0);
      break;
  }
}
