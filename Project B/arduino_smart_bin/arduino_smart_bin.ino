const int trigPin = 12;
const int echoPin = 8;

float duration;
float distance;

#include <Servo.h>

Servo servo;  // create a servo object

int pos = 0;

void setup() {
  Serial.begin(115200);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  servo.attach(7); 
  servo.write(0);
}

void loop() {
  distances();
  openclose();
}

void distances () {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);

  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  duration = pulseIn(echoPin, HIGH);
  distance = (duration * 0.0343) / 2;

  Serial.print("Afstand: ");
  Serial.println(distance);
}

void openclose ()
{
  if (distance <= 100)
  { 
    while (pos < 180){
  for (pos = 0; pos < 180; pos++){
  //Serial.println(pos);
  servo.write(pos);
  delay (10);
  }
 }
}
  else if (pos == 180) {
  delay (5000);  
  for (pos = 180; pos > 0; pos--){
     //Serial.println(pos);
    servo.write(pos);
    delay(10);
  }
 }
}
    
