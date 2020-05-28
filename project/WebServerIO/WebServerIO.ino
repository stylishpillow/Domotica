#include <Servo.h>

/*
 Web Server
 A simple web server that shows the value of the analog input pins.
 using an Arduino Wiznet Ethernet shield.
 created 18 Dec 2009 by David A. Mellis modified 9 Apr 2012, by Tom Igoe
  
 Circuit:
 * Ethernet shield attached to pins 10, 11, 12, 13 (use Ethernet2.h for Ethernet2 shield)
 * Analog inputs attached to pins A0 through A5 (optional)

 v1.1 modified nov. 2015, by S. Oosterhaven (support GET-variables to set/unset digital pins)
 v1.2 modified dec. 2016, by S. Oosterhaven (minor bugs fixed)
 v1.3 modified 6 dec. 2016, by S. Oosterhaven (stability problems, due to less memory, fixed)
 v1.4 modified 16 dec. 2016, by S. Oosterhaven (stability problems, due to less memory, fixed)
 v1.5 modified 16 may. 2019, by S =. Oosterhaven (fix for favicon-problem)
 */

// Onderstaande regels worden gebruikt om relatief veel tekst te verwerken. Aangezien de Arduino maar weinig intern geheugen heeft (1 KB)
// worden deze teksen opgeslagen en verwerkt vanuit het programmageheugen. Je wordt niet geacht dit te begrijpen (maar dat mag wel).
//----------
const char cs0[] PROGMEM = "<STRONG>Opdracht 17 van het vak embedded systems 1</STRONG>"; 
const char cs1[] PROGMEM = "Dit voorbeeld is gebaseerd op het script in Voorbeelden->Ethernet->Webserver";
const char cs2[] PROGMEM = "De website is dynamische gemaakt door sensorwaarden van kanaal 0 toe te voegen.";
const char cs3[] PROGMEM = "<B>Breid het programma uit</B> met de mogelijkheid om variabelen mee te geven.";
const char cs4[] PROGMEM = "Dit kan o.a. door GET-variabelen, via de URL (192.168.1.3/?p8=1).";
const char cs5[] PROGMEM = "Gebruik de functie <STRONG style='color:Black'>parseHeader(httpHeader, arg, val))</STRONG>";
const char* const string_table[] PROGMEM = {cs0, cs1, cs2, cs3, cs4, cs5};
char buffer[100];  
//----------

//Defines
#define maxLength     20  // header length, don't make it to long; Arduino doesn't have much memory
#define sensorPin     0   // sensor on channel A0 
#define ledPin        8
#define infoPin       9  

Servo servo;  // create a servo object

//Includes
#include <SPI.h>
#include <Ethernet.h>

// Enter a MAC address and IP address for your controller below. The IP address will be dependent on your local network:
//byte mac[] = { 0x40, 0x6C, 0x8F, 0x36, 0x84, 0x8A }; 
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };   // Ethernet adapter shield S. Oosterhaven
IPAddress ip(192, 168, 1, 9);

// Initialize the Ethernet server library (port 80 is default for HTTP):
EthernetServer server(80);

String httpHeader;           // = String(maxLength);
int arg = 0, val = 0;        // to store get/post variables from the URL (argument and value, http:\\192.168.1.3\website?p8=1)

void setup() {
  servo.write(0);
   //Init I/O-pins
   DDRD = 0xFC;              // p7..p2: output
   DDRB = 0x3F;              // p14,p15: input, p13..p8: output
   pinMode(ledPin, OUTPUT);
   pinMode(infoPin, OUTPUT);
   
   //Default states
   digitalWrite(ledPin, LOW);
   digitalWrite(infoPin, LOW);
  
   // Open serial communications and wait for port to open:
   Serial.begin(9600);

   // Start the Ethernet connection and the server:
   // Try to get an IP address from the DHCP server
   // if DHCP fails, use static address
   if (Ethernet.begin(mac) == 0) {
     Serial.println("No DHCP");
     Ethernet.begin(mac, ip);
   }
  
   //Start the ethernet server and give some debug info
   server.begin();
   Serial.println("Embedded Webserver with I/O-control v1.5");
   Serial.println("Ethernetboard connected (pins 10, 11, 12, 13 and SPI)");
   Serial.print("Server is at "); Serial.println(Ethernet.localIP());
   Serial.print("ledpin at pin "); Serial.println(ledPin);
   Serial.print("infoPin at pin "); Serial.println(ledPin);

   
}


void loop() {
  // listen for incoming clients 
  EthernetClient client = server.available(); 

//   digitalWrite(ledPin, HIGH);
//   delay(1000);
//   digitalWrite(ledPin, LOW);
//   delay(1000);
//   digitalWrite(infoPin, HIGH);
//   delay(1000);
//   digitalWrite(infoPin, LOW);
  
  //Webpage part
  if (client) {
    Serial.println("New client connected");
    // an http request ends with a blank line
    boolean currentLineIsBlank = true;
    
    while (client.connected()) {
      if (client.available()) {
        //read characters from client in the HTTP header
        char c = client.read();
        //store characters to string
        if (httpHeader.length() < maxLength) httpHeader += c;  // don't need to store the whole Header
        //Serial.write(c);                                     // for debug only
        
        // at end of the line (new line) and the line is blank, the http request has ended, so you can send a reply
        if (c == '\n' && currentLineIsBlank) {
          // client HTTP-request received
          httpHeader.replace(" HTTP/1.1", ";");                // clean Header, and put a ; behind (last) arguments
          httpHeader.trim();                                   // remove extra chars like space
          Serial.println(httpHeader);                          // first part of header, for debug only
             
          // send a standard http response header
          client.println("HTTP/1.1 200 OK");
          client.println("Content-Type: text/html");
          client.println("Connection: close");          // the connection will be closed after completion of the response
          //client.println("Refresh: 3");               // refresh the page automatically every 3 sec
          client.println();
          client.println("<!DOCTYPE HTML>");
          client.println("<HTML>");
          client.println("<meta http-equiv='refresh' content='3'>");
          client.println("<HEAD><TITLE>Embedded I/O-Webserver</TITLE><link rel=\"icon\" href=\"data:,\"></HEAD>");
          client.println("<STYLE> body{width:800px;font-family:verdana;background-color:LightBlue;} ");
          client.println("</STYLE>");
          client.println("<BODY>");
          client.println("<H4 style='color:DarkBlue'>Embedded I/O-Webserver</H4>"); 

       // show intro-text, it is OK to remove the following 7 lines
       client.println("<P style='font-size:80%; color:Gray'>");
       for (int i = 0; i <= 5; i++) 
       {
           strcpy_P(buffer, (char*)pgm_read_word(&(string_table[i])));   // Necessary casts and dereferencing, just copy
           client.println(buffer); client.println("<br>");
       }
       client.println("</P>");
          
          // output the value of analog input pin A0
          int sensorValue = analogRead(sensorPin);
          sensorValue = map(sensorValue, 0, 1023, 0, 180);
          client.println("<P style='color:DarkBlue'>");      
          client.print("Analog sensor, channel "); client.print(sensorPin); client.print(": ");
          if(sensorValue > 600){
            client.print("<b style='color:red'>");
          } else {
            client.print("<b>");
          }
           client.print(sensorValue); client.print("</b>");
          client.println("</P>");
          
          //grab commands from the url
          client.println("<P>");

          
          if (parseHeader(httpHeader, arg, val)) {   // search for argument and value, eg. p8=1
              //Serial.print(arg); Serial.print(" "); Serial.println(val);  // for debug only
              servo.attach(arg); // attaches the servo on pin 9 to the servo object
              servo.write(val);                // Recall: pins 10..13 used for the Ethernet shield
              client.print("Pin ");client.print(arg); client.print(" = "); client.println(val);
          }
          else client.println("No IO-pins to control");
          client.println("</P>");
          
          // end of website
          client.println("</BODY>");
          client.println("</HTML>");
          break;
        }
        
        if (c == '\n') {
          // you're starting a new line
          currentLineIsBlank = true;
        }
        else if (c != '\r') {
          // you've gotten a character on the current line
          currentLineIsBlank = false;
        }
      }
    }
    // give the web browser time to receive the data
    delay(1);
    // close the connection:
    client.stop();
    httpHeader = "";
    Serial.println("Client disconnected");
  }
}

// GET-vars after "?"   192.168.1.3/?p8=1
// parse header. Argument starts with p (only p2 .. p9)
// input:  header = HTTPheader from client
// output: a = argument (bijv. p8)  // let op a en v zijn uitvoerparameters, vandaar de &a en &v
// output: v = value (bijv. 1)
// result: true if arguments are valid
bool parseHeader(String header, int &a, int &v)
{
          char pinArray[0];
          
          pinArray[0] = header.charAt(header.indexOf("?")+2);
          a = atoi(pinArray);
          Serial.println(a);
          
          char valArray [2];
          
          valArray[0] = header.charAt(header.indexOf("=")+1);
          valArray[1] = header.charAt(header.indexOf("=")+2);
          valArray[2] = header.charAt(header.indexOf("=")+3);
          v = atoi(valArray);
          Serial.println(v);
          
          
          if ( (a == 7) && (v >= 0 && v <= 180) )
          {
            return true;
          } else {
            return false;
          }
}
