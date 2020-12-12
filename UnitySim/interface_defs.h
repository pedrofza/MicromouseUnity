#ifndef INTERFACE_DEFS_H
#define INTERFACE_DEFS_H

template <typename T>
class IMotor
{
  public:
  virtual void setVoltage(T voltage) = 0;
  virtual T getVoltage() const = 0;
};

class IDistanceSensor
{
	virtual float distance() const = 0;
};

#endif
