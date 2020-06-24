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

/**
 * This function calculates the distance that is coming from the ultrasone sensor.
 * And open and close the bin when the distance is on a specific number.
 */
void distances () {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);

  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  duration = pulseIn(echoPin, HIGH);
  distance = (duration * 0.0343) / 2;

  /**
   * If this distance is between the 1 and 20 cm the bin will be opened.
   */
  if (distance > 0 && distance <= 20)
  {
    while (pos < 180) {
      openBin();
    }
  }
  /**
   * When the position (pos) is 180 (opened) the bin will be closed.
   */
  else if (pos == 180 && (distance > 50 && distance < 1000)) {
    delay (5000);
    closeBin();
  }
} 


String Status = "Gesloten";

/**
 * This function open the bin with a for loop.
 */
void openBin () {
  for (pos = 0; pos < 180; pos++) {
    servo.write(pos);
    delay(10);
  }
  Status = "Geopend";
}

/**
 * This function close the bin with a for loop.
 */

void closeBin () {
  for (pos = 180; pos > 0; pos--) {
    servo.write(pos);
    delay(10);
  }
  Status = "Gesloten";
}

void runServer()
{
  if (!connected) return;

  EthernetClient ethernetClient = server.available();
  if (!ethernetClient) return;

  while (ethernetClient.connected())
  {
    char buffer[128];
    int count = 0;
    while (ethernetClient.available())
    {
      buffer[count++] = ethernetClient.read();
    }
    buffer[count] = '\0';
    if (count > 0)
    {
      Serial.println(buffer);
      if (String(buffer) == String("open"))
      {
        while (pos < 180) {
          openBin();
        }
        ethernetClient.print(Status);
      } 
      else if(String(buffer) == String("close"))
      {
        while (pos > 0) {
          closeBin();
        }
        ethernetClient.print(Status);
      } 
      else if (String(buffer) == String("checkIP"))
      {
        ethernetClient.print("correct");
      } else if (String(buffer) == String("status"))
      {
        ethernetClient.print(Status);
      }
    }
  }
}

void setup() {
  Serial.begin(115200);

  if (Ethernet.begin(mac) == 0) return;
  Serial.print("Listening on address: ");
  Serial.println(Ethernet.localIP());
  server.begin();
  connected = true;

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  servo.attach(7);
  servo.write(0);
}

void loop() {
  runServer();
  distances();
}
