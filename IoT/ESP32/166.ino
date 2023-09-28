#include <Arduino.h>
#include <WiFi.h>
#include <WiFiGeneric.h>
#include <WiFiClientSecure.h>
#include <esp_wifi.h>
#include <AsyncElegantOTA.h>
#include <MQTT.h>
#include <PubSubClient.h>
#include <stdio.h>
#include <stdlib.h>
#include <FS.h>
#include <esp_task_wdt.h>
#define BUILTIN_LED 2
#define NUM_ELEMENTS(x) (sizeof(x) / sizeof((x)[0]))

// This bot controls mixing fans 8 & 9

// Mixing fans are 4 pin PWM Arctic F8 with rare earth
// magnets affixed to the body. These attach to and drive 
// the magnetic stirrer in each 4L jar of chemicals.

// The 4th pin for PWM fans is for measuring and setting the rpm.
// In general, when any PWM is > 0, the code below reads the pulses for 0.1 seconds 
// and writes the rpm desired speed for the remained of the second. I think...

// The command to stir the jar includes duration and intensity
// TODO : compare to overspeed, error if > (magnetic stirrer came off)


String hostname = "ESP32_Garden_166";
IPAddress local_IP();
IPAddress gateway();
IPAddress subnet();

const char* root_ca PROGMEM = \
"-----BEGIN CERTIFICATE-----\r\n" \
"-----END CERTIFICATE-----\r\n";

const char* ssid = "";
const char* password = "";

const char* MQTT_SERVER = "";
const int MQTT_PORT = 8883;
const char* MQTT_USERNAME = "";
const char* MQTT_PASSWORD = "";
const char* MQTT_CLIENT_NAME = "";
const unsigned int keepAliveInterval = 60; // Set the keep-alive interval in seconds


const char* MQTT_TOPIC_HELLO = "yargbot/hello";
const char* MQTT_TOPIC_ACK = "yargbot/acknowledge";
const char* MQTT_TOPIC_DN = "yargbot/done";
const char* MQTT_TOPIC_ERR = "yargbot/error";
const char* MQTT_TOPIC_MEAS = "yargbot/measurement";
const char* MQTT_TOPIC_HRTBT = "yargbot/heartbeat";

unsigned long lastTime = 0;
// Timer set to 10 minutes (600000)
//unsigned long timerDelay = 600000;
// Set timer to 5 seconds (5000)
unsigned long timerDelay = 5000;

unsigned long previousMillis = 0;
unsigned long intervalHeartbeat = 15 * 60 * 1000; // 15 minutes in milliseconds - for first boot and randomized +/- 2% after

bool flgCountingPulses = false;

WiFiClientSecure espClient;
PubSubClient client(espClient);
WiFiClient apiClient;

AsyncWebServer server(80);

bool flgFirstBoot = true;

// PWM properties
const int freq = 25000;
const int resolution = 8;

int mF_LastRPM[2]
	{
		0, 0
	};
	
int mF_PWMSpeed[2]
	{
		0, 0
	};
	
int mF_OverSpeed[2]
	{
		0, 0
	};
	
volatile unsigned long mF_PulseCount[2]
	{
		0, 0
	};
	
unsigned long mF_millisecondsUntilDone[2]
	{
		0, 0
	};
	
String mF_CommandID[2]
	{
		"", ""
	};
	
bool flgMfRunning[2]
	{
		false, false
	};

const int mF_RelayPin[2]
	{
		17, 18
	};

const int mF_PWMChannel[2]
	{
		0, 1
	};
const int mF_PWMPin[2]
	{
		16, 12
	};
const int mF_RPMPin[2]
	{
		5, 36
	};

unsigned long millisecondsLastTachoMeasurement = 0;
const int tachoUpdateCycle = 1000; // how often tacho speed shall be determined, in milliseconds

void IRAM_ATTR rpm_fan_1() {
	if (flgCountingPulses) {
		mF_PulseCount[0]++;
	}
}

void IRAM_ATTR rpm_fan_2() {
	if (flgCountingPulses) {
		mF_PulseCount[1]++;
	}
}

void updateTacho(void) {
	if ((unsigned long)(millis() - millisecondsLastTachoMeasurement) >= tachoUpdateCycle)
	{
		for (int i = 0; i < 2; i++) {
			ledcWrite(mF_PWMChannel[i],0);
		}

		flgCountingPulses = true;
		delay(100);
		flgCountingPulses = false;

		for (int i = 0; i < 2; i++) {
			ledcWrite(mF_PWMChannel[i],mF_PWMSpeed[i]);

			mF_LastRPM[i] = mF_PulseCount[i] * (float)160;
			mF_PulseCount[i] = 0;
			
			// Logic to stop mixing fan if in overspeed and publish error topic
			
		}
		
		millisecondsLastTachoMeasurement = millis();
	}
}

float getVccVoltage() {
  return (analogRead(35) / 4095.0) * 3.3; // Pin 35 is internal VCC voltage reference
}

float readInternalTemperature() {
  return (temperatureRead() - 32) / 1.8; // Convert from Fahrenheit to Celsius
}

unsigned long generateRandomValue(unsigned long lowerLimit, unsigned long upperLimit){
  
  unsigned long range = upperLimit - lowerLimit + 1;

  unsigned long randomValue = lowerLimit + (unsigned long)random(range);

  return randomValue;

}

void setup_wifi(){
	WiFi.disconnect(true, true);
	WiFi.mode(WIFI_STA);
	WiFi.config(INADDR_NONE, INADDR_NONE, INADDR_NONE);
	WiFi.setHostname(hostname.c_str()); //define hostname
	WiFi.begin(ssid, password);
	int wifi_connect_times = 0;

	while (WiFi.status() != WL_CONNECTED && wifi_connect_times <= 20)
	{
		delay(1000);
		wifi_connect_times++;
		
		if (wifi_connect_times == 10)
		{
			WiFi.disconnect(true, true);
			WiFi.config(INADDR_NONE, INADDR_NONE, INADDR_NONE);
			WiFi.setHostname(hostname.c_str()); //define hostname
			WiFi.begin(ssid, password);
		}
	}
	
	if (WiFi.status() != WL_CONNECTED)
	{
		ESP.restart();
	}

}

void setup(void) {
	for (int i = 0; i < NUM_ELEMENTS(mF_RelayPin); i++)
	{
		pinMode(mF_RelayPin[i], OUTPUT);
		digitalWrite(mF_RelayPin[i], HIGH);
	}

	pinMode(BUILTIN_LED, OUTPUT);

	for (int i = 0; i < NUM_ELEMENTS(mF_PWMPin); i++)
	{
		ledcSetup(mF_PWMChannel[i], freq, resolution);
		ledcAttachPin(mF_PWMPin[i], mF_PWMChannel[i]);
	}

	//Configure tachs
	for (int i = 0; i < NUM_ELEMENTS(mF_RPMPin); i++)
	{
		pinMode(mF_RPMPin[i], INPUT);
		digitalWrite(mF_RPMPin[i], HIGH);
	}


	//5, 36, 39, 34, 35, 32, 33
	attachInterrupt(digitalPinToInterrupt(5), rpm_fan_1, FALLING);
	attachInterrupt(digitalPinToInterrupt(36), rpm_fan_2, FALLING);

    // Set up ADC to measure VCC voltage
  analogSetPinAttenuation(35, ADC_0db); // Pin 35 has 0dB attenuation (0-1.1V measurement range)


	setup_wifi();
	delay(10);
	espClient.setCACert(root_ca);

	client.setServer(MQTT_SERVER, MQTT_PORT);
	client.setKeepAlive(keepAliveInterval);
	client.setCallback(callback);

	delay(10);

	server.on("/", HTTP_GET, [](AsyncWebServerRequest *request) {
	request->send(200, "text/plain", "Hi! I am yargbot 165.");
	});

	AsyncElegantOTA.begin(&server); // Start ElegantOTA
	delay(1000);
	server.begin();

  previousMillis = millis();

}

void reconnect(){

	digitalWrite(BUILTIN_LED, LOW);
	byte mqttFailCount = 0;
	byte tooManyFailures = 10;
	// Loop until we're reconnected
	while (!client.connected())
	{
		if (mqttFailCount <= tooManyFailures)
		{
			// Attempting MQTT connection
			if (client.connect(MQTT_CLIENT_NAME,MQTT_USERNAME,MQTT_PASSWORD))
			{
				digitalWrite(BUILTIN_LED, HIGH);
				

				uint8_t mac[6];
				WiFi.macAddress(mac); // Get the MAC address

        
				TickType_t tickCount = xTaskGetTickCount();
				uint32_t uptimeMillis = (uint32_t)tickCount * portTICK_PERIOD_MS;


        char payload[500];
        snprintf(payload, sizeof(payload), "{\"UptimeMillis\": %u, \"MacAddress\": \"%02X:%02X:%02X:%02X:%02X:%02X\", \"Hostname\": \"%s\", \"CpuFreqMHz\": %d, \"ChipRevision\": %u, \"FlashChipSize\": %u, \"FlashChipSpeed\": %u, \"VccVoltage\": %.2f, \"FreeFlash\": %u}",
          uptimeMillis, mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], hostname.c_str(), ESP.getCpuFreqMHz(), ESP.getChipRevision(), ESP.getFlashChipSize(), ESP.getFlashChipSpeed(), getVccVoltage(), ESP.getFreeSketchSpace());

        delay(100);

				client.publish(MQTT_TOPIC_HELLO, payload);

        delay(10);

				// Listen for broadcast messages
				client.subscribe("mixingFan/MF1");
				client.subscribe("mixingFan/MF2");
				client.subscribe("mixingFan/MF3");
				client.subscribe("mixingFan/MF4");
				client.subscribe("mixingFan/MF5");
				client.subscribe("mixingFan/MF6");
				client.subscribe("mixingFan/MF7");
				
				client.subscribe("yargbot/Enable");
			}
			else
			{
				digitalWrite(BUILTIN_LED, LOW);
				mqttFailCount++;
				// Wait 5 seconds before retrying
				delay(5000);
			}
		}
		else
		{
			//MQTT failures in a row. Resetting WiFi connection.");
			WiFi.disconnect();
			delay(5000);
			setup_wifi();
			mqttFailCount = 0;
		}
	}
}

void callback(char* topic, byte* payload, unsigned int length){
	char payloadStr[length + 1]; // Create a char array that's 1 byte longer than the incoming payload to copy it to and make room for the null terminator so it can be treated as string.
	memcpy(payloadStr, payload, length);
	payloadStr[length + 1] = '\0';

	// Incoming mixingFan payload format will be <PUMPSPEED>:<OVERSPEED>:<DURATION>:<MIXINGFANSCHEDULE>

	if (strcmp(topic, "mixingFan/MF8") == 0){
		char* strtokIndx;

		strtokIndx = strtok(payloadStr,":");
		mF_PWMSpeed[0] = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		mF_OverSpeed[0] = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		mF_millisecondsUntilDone[0] = millis() + (atol(strtokIndx) * 60 * 1000);

		strtokIndx = strtok(NULL,":");
		mF_CommandID[0] = strtokIndx;

		String postCmd = "{\"RemoteHostname\":\"" + hostname + "\",\"CommandID\":\"" + mF_CommandID[0].substring(0,36) + "\"}";

		client.publish(MQTT_TOPIC_ACK, postCmd.c_str());
		flgMfRunning[0] = true;
		ledcWrite(mF_PWMChannel[0], mF_PWMSpeed[0]);
		digitalWrite(mF_RelayPin[0], LOW);
	}

	if (strcmp(topic, "mixingFan/MF9") == 0){
		char* strtokIndx;

		strtokIndx = strtok(payloadStr,":");
		mF_PWMSpeed[1] = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		mF_OverSpeed[1] = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		mF_millisecondsUntilDone[1] = millis() + (atol(strtokIndx) * 60 * 1000);

		strtokIndx = strtok(NULL,":");
		mF_CommandID[1] = strtokIndx;

		String postCmd = "{\"RemoteHostname\":\"" + hostname + "\",\"CommandID\":\"" + mF_CommandID[1].substring(0,36) + "\"}";

		client.publish(MQTT_TOPIC_ACK, postCmd.c_str());
		flgMfRunning[1] = true;
		ledcWrite(mF_PWMChannel[1], mF_PWMSpeed[1]);
		digitalWrite(mF_RelayPin[1], LOW);
	}
}



void loop(void) {
	if (!client.connected())
	{
		reconnect();
	}
	
	for (int i = 0; i < 2; i++)
	{
		if (millis() >= mF_millisecondsUntilDone[i] && flgMfRunning[i])
		{
			flgMfRunning[i] = false;
			ledcWrite(mF_PWMChannel[i], 0);
			mF_PWMSpeed[i] = 0;
			digitalWrite(mF_RelayPin[i], HIGH);
			String postCmd = "{\"RemoteHostname\":\"" + hostname + "\",\"CommandID\":\"" + mF_CommandID[i].substring(0,36) + "\"}";
			client.publish(MQTT_TOPIC_DN, postCmd.c_str());
		}
	}

	if (mF_PWMSpeed[0] > 0 && flgMfRunning[0] ||
	mF_PWMSpeed[1] > 0 && flgMfRunning[1]) {
		updateTacho();
	}
	else
	{
		delay(10);
	}

  unsigned long currentMillis = millis();
  if (currentMillis - previousMillis >= intervalHeartbeat) {
      previousMillis = currentMillis; // Reset the timer

      // Randomize next heartbeat time by 2% every time
      unsigned long newInterval = generateRandomValue((unsigned long)intervalHeartbeat * 0.98, (unsigned long)intervalHeartbeat * 1.02);
      intervalHeartbeat = newInterval;

      // Gather and publish task information
      TaskHandle_t currentTask = xTaskGetCurrentTaskHandle();
      const char* taskName = pcTaskGetTaskName(currentTask);
    
      UBaseType_t stackHighWaterMark = uxTaskGetStackHighWaterMark(currentTask);

      float vccVoltage = getVccVoltage();
      int rssi = WiFi.RSSI();
      uint32_t freeHeap = ESP.getFreeHeap();
      uint32_t heapSize = ESP.getHeapSize();

   		TickType_t tickCount = xTaskGetTickCount();
			uint32_t uptimeMillis = (uint32_t)tickCount * portTICK_PERIOD_MS;

      float temperature = readInternalTemperature();

      char payload[500];
      snprintf(payload, sizeof(payload), "{\"Hostname\": \"%s\", \"UptimeMillis\": %u, \"Task\": \"%s\", \"StackHighWaterMark\": %u, \"VccVoltage\": %.2f, \"RSSI\": %d, \"FreeHeap\": %u, \"HeapSize\": %u, \"Temperature\": %.2f}", 
        hostname.c_str(), uptimeMillis, taskName, stackHighWaterMark, vccVoltage, rssi, freeHeap, heapSize, temperature);

    client.publish(MQTT_TOPIC_HRTBT, payload);
  }

	client.loop();
}
