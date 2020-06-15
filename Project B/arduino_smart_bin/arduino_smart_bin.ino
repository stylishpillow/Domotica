#include <Servo.h>
#include <SPI.h>
#include <Ethernet.h>

byte mac[] = { 0x40, 0x6c, 0x8f, 0x36, 0x84, 0x8a };
EthernetServer server(80);
bool connected = false;

const int trigPin = 12;
const int echoPin = 8;

float duration;
float distance;


Servo servo;  // create a servo object

int pos = 0;

bool accuraat = true;

//check distance from bin
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
  if (distance <= 20)
   { 
    while (pos < 180){
      openBin();
      }
    }
   else if (pos == 180 && distance < 1000){
    closeBin();
   }
}

void openBin (){
  for (pos = 0; pos < 180; pos++){
//      Serial.println(pos);
      servo.write(pos);
      delay(10);
 }
}

void closeBin (){
    delay (5000);  
  for (pos = 180; pos > 0; pos--){
//     Serial.println(pos);
    servo.write(pos);
    delay(10);
 }
}

void runServer()
{
  if(!connected) return;
  
  EthernetClient ethernetClient = server.available();
  if(!ethernetClient) return;
  
  while(ethernetClient.connected())
  {
    char buffer[128];
    int count = 0;
    while(ethernetClient.available())
    {
      buffer[count++] = ethernetClient.read();
    }
    buffer[count] = '\0';
    if(count > 0)
    {
      Serial.println(buffer);
      if(String(buffer) == String("force"))
      {
        ethernetClient.print("gelukt");
      } else {
        ethernetClient.print(buffer);
      }
    }
  }
}

void setup() {
  Serial.begin(115200);

if(Ethernet.begin(mac) == 0) return;
Serial.print("Listening on address: ");
Serial.println(Ethernet.localIP());
server.begin();
connected= true;

  
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  servo.attach(7); 
  servo.write(0);
}

void loop() {
  distances();
//  openclose();
  runServer();
}
    
