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



String hostname = "";
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
const char* MQTT_TOPIC_ERR = "yargbot/error";
const char* MQTT_TOPIC_HRTBT = "yargbot/heartbeat";
const char* MQTT_TOPIC_MEAS = "measuredResult";

unsigned long lastTime = 0;
// Timer set to 10 minutes (600000)
//unsigned long timerDelay = 600000;
// Set timer to 5 seconds (5000)
unsigned long timerDelay = 5000;

unsigned long previousMillis = 0;
unsigned long intervalHeartbeat = 15 * 60 * 1000; // 15 minutes in milliseconds - for first boot and randomized +/- 2% after


WiFiClientSecure espClient;
PubSubClient client(espClient);
WiFiClient apiClient;

AsyncWebServer server(80);

bool flgFirstBoot = true;

String postCmd[4]
	{
		"", "", "", ""
	};

unsigned long mF_millisecondsUntilDone[4]
	{
		0, 0, 0, 0
	};
	
		
float getVccVoltage() {
  return (analogRead(35) / 4095.0) * 3.3; // Pin 35 is internal VCC voltage reference
}

float readInternalTemperature() {
  return (temperatureRead() - 32) / 1.8; // Convert from Fahrenheit to Celsius
}

float generateRandomDecimalValue(float lowerLimit, float upperLimit) {
  float randomValue = (float)esp_random() / UINT32_MAX; // Generate a random float between 0 and 1
  float range = upperLimit - lowerLimit;

  return lowerLimit + (randomValue * range);
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

	pinMode(BUILTIN_LED, OUTPUT);

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

        delay(500);

				client.publish(MQTT_TOPIC_HELLO, payload);

        delay(10);

				// Listen for broadcast messages
				client.subscribe("takeMeasurement/1H3");
				client.subscribe("takeMeasurement/2W9");
				client.subscribe("takeMeasurement/2T2");
				client.subscribe("takeMeasurement/1T1");
				
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

	if (strcmp(topic, "takeMeasurement/1T1") == 0){
		char* strtokIndx;

		strtokIndx = strtok(payloadStr,":");
		String locationID = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		String measurementTypeID = atoi(strtokIndx);

		mF_millisecondsUntilDone[0] = millis() + generateRandomValue(250, 1750);
		
		postCmd[0] = "{\"locationID\":\"" + locationID + "\",\"measurementTypeID\":\"" + measurementTypeID + "\",\"measuredValue\":\"" + generateRandomDecimalValue(15,40) + "\"}";
	}

	if (strcmp(topic, "takeMeasurement/2T2") == 0){
		char* strtokIndx;

		strtokIndx = strtok(payloadStr,":");
		String locationID = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		String measurementTypeID = atoi(strtokIndx);

		mF_millisecondsUntilDone[1] = millis() + generateRandomValue(250, 1750);
		
		postCmd[1] = "{\"locationID\":\"" + locationID + "\",\"measurementTypeID\":\"" + measurementTypeID + "\",\"measuredValue\":\"" + generateRandomDecimalValue(15,40) + "\"}";
	}
	if (strcmp(topic, "takeMeasurement/2W9") == 0){
		char* strtokIndx;

		strtokIndx = strtok(payloadStr,":");
		String locationID = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		String measurementTypeID = atoi(strtokIndx);

		mF_millisecondsUntilDone[2] = millis() + generateRandomValue(250, 1750);
		
		postCmd[2] = "{\"locationID\":\"" + locationID + "\",\"measurementTypeID\":\"" + measurementTypeID + "\",\"measuredValue\":\"" + generateRandomDecimalValue(25,75) + "\"}";
	}
	if (strcmp(topic, "takeMeasurement/1H3") == 0){
		char* strtokIndx;

		strtokIndx = strtok(payloadStr,":");
		String locationID = atoi(strtokIndx);

		strtokIndx = strtok(NULL,":");
		String measurementTypeID = atoi(strtokIndx);

		mF_millisecondsUntilDone[3] = millis() + generateRandomValue(250, 1750);
		
		postCmd[3] = "{\"locationID\":\"" + locationID + "\",\"measurementTypeID\":\"" + measurementTypeID + "\",\"measuredValue\":\"" + generateRandomDecimalValue(22,77) + "\"}";
	}
}



void loop(void) {
	if (!client.connected())
	{
		reconnect();
	}
	
	for (int i = 0; i < 4; i++)
	{
		if (millis() >= mF_millisecondsUntilDone[i])
		{
			client.publish(MQTT_TOPIC_MEAS, postCmd[0].c_str());
		}
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
