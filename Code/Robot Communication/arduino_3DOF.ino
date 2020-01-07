// by J-99
void setup() {
  // put your setup code here, to run once:

}

void loop() {
  // put your main code here, to run repeatedly:
  readSerial();
  delay(100);
}

void readSerial() 
{
  if (Serial.available()) 
  {
    String serialData = Serial.readString();
    Serial.println("I received ");
    //Serial.println(serialData);
  }
}
