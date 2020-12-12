#ifndef BIT_UTILS_H
#define BIT_UTILS_H

constexpr int BITS_PER_BYTE = 8;

unsigned int genMask(int bits)
{
  unsigned int mask;
  int sizeInBits = sizeof(mask) * BITS_PER_BYTE;
  mask = bits >= sizeInBits ? ~0u : (1u << bits) - 1;
  return mask;
}

#endif
