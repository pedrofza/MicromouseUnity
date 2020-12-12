#ifndef BOARD_H
#define BOARD_H

namespace Board
{
  static constexpr int LED_PIN {13};
  void init();
  void ledOn();
  void ledOff();
  void ledToggle();
}

#include "board.hpp"

#endif
