void Board::init()
{
  pinMode(LED_PIN, OUTPUT);
}
  
void Board::ledOn()
{
  digitalWrite(LED_PIN, HIGH);
}
  
void Board::ledOff()
{
  digitalWrite(LED_PIN, LOW);
}

void Board::ledToggle()
{
  if (digitalRead(LED_PIN) == LOW)
  {
    digitalWrite(LED_PIN, HIGH);
  }
  else
  {
    digitalWrite(LED_PIN, LOW);
  }
}
