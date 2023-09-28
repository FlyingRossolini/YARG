#include <Arduino.h>
#include <WiFi.h>
#include <WiFiGeneric.h>
#include <WiFiClientSecure.h>
#include <esp_wifi.h>
#include <MQTT.h>
#include <PubSubClient.h>
#include <stdio.h>
#include <stdlib.h>
#include <FS.h>
#include <esp_task_wdt.h>
#define BUILTIN_LED 2
#define NUM_ELEMENTS(x) (sizeof(x) / sizeof((x)[0]))

// This bot is a Fertigation Event tester - it will mock the ebbFlowMeter job

#define EbbFlowSENSOR  27 //real world pin 27 to signal of post-res flow meter.

String hostname = "ESP32_Garden_170";
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
const char* willTopic = "yargBot/ESTOP";
const char* willMessage = "{}"hostname.c_str();
const int willQoS = 2; 
const bool willRetain = false; // Retain flag for the LWT message

const unsigned int keepAliveInterval = 1;
// Interval set to 1 second with hopes that LWT message will propogate the 
// the system as quickly as possible; acting as an estop.


const char* MQTT_TOPIC_HELLO = "yargbot/hello";
const char* MQTT_TOPIC_ERR = "yargbot/error";
const char* MQTT_TOPIC_HRTBT = "yargbot/heartbeat";

const char* MQTT_TOPIC_FE_EFM_ACK = "yargbot/FE_ebbFlowMeter_ACK"; //ebb flowmeter acknowledge fertigationEvent
const char* MQTT_TOPIC_FE_EFM_DN = "yargbot/FE_ebbFlowMeter_DN"; //ebb flowmeter command done
const char* MQTT_TOPIC_FE_EP_STOP = "yargbot/FE_ebbPump_STOP"; // command to stop ebb pump
const char* MQTT_TOPIC_FE_OVER = "yargbot/FE_potOverflow_ACK"; //ebb flowmeter acknowledges overflow condition (used in part to reset moisture sensor overflow logic)
const char* MQTT_TOPIC_ESTOP = "yargbot/ESTOP"; // not used in this bot? (so far)

struct FertigationEventMessage {
	char CommandID[12];
	uint8_t EbbSpeed;
	uint16_t EbbAmount;
	uint16_t AntiShockRamp;
	uint32_t SoakDuration;
	uint8_t FlowSpeed;
	float ExpectedEbbFlowRate;
	float ExpectedFlowFlowRate;
	float PumpErrorThreshold;
	// above are populated by parsing the fertigationEvent payload
	uint32_t EbbStartMillis;
	bool IsEbbCounting;
	bool IsEbbOverflow;
	bool IsCalculatingFlowRate;
	bool IsStopSent;
	volatile uint16_t EbbCurrentAmount;
};

struct FertigationEventMessage thisFertigationEvent;

byte flowRateErrorCount = 0;

unsigned long previousHeartbeatMillis = 0;

unsigned long previousFlowRateMillis = 0;
uint16_t previousEbbAmount = 0; 

unsigned long intervalHeartbeat = 15 * 60 * 1000; // 15 minutes in milliseconds - for first boot and randomized +/- 2% after
unsigned long intervalOneSecond = 1000;

float flowRate; // Flow rate in some units per second
bool belowThreshold = false; // Flag to track if flow rate is below threshold
int consecutiveReadings = 0; // Count of consecutive readings below threshold

WiFiClientSecure espClient;
PubSubClient client(espClient);

bool flgFirstBoot = true;
bool flgESTOP = false;
bool flgAuthorized = false;

void IRAM_ATTR ebbFlowMeterCounter()
{
	if (thisFertigationEvent.IsEbbCounting)
	{
		thisFertigationEvent.EbbCurrentAmount++;
		
		if (thisFertigationEvent.EbbCurrentAmount >= thisFertigationEvent.EbbAmount && !thisFertigationEvent.IsStopSent)
		{
			client.publish(MQTT_TOPIC_FE_EP_STOP, "");
			thisFertigationEvent.IsStopSent = true;
		}
	}
}
		
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
	WiFi.setHostname(hostname.c_str()); 
	WiFi.begin(ssid, password);
	
	int wifi_connect_times = 0;
	const int max_wifi_connect_attempts = 20;

	while (WiFi.status() != WL_CONNECTED && wifi_connect_times <= max_wifi_connect_attempts) {
        delay(1000);
		wifi_connect_times++;
		
		if (wifi_connect_times == 10)
		{
			WiFi.disconnect(true, true);
			WiFi.config(INADDR_NONE, INADDR_NONE, INADDR_NONE);
			WiFi.setHostname(hostname.c_str());
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

	attachInterrupt(digitalPinToInterrupt(EbbFlowSENSOR), ebbFlowMeterCounter, FALLING); //Ebb flow meter 

	setup_wifi();
	delay(10);
	espClient.setCACert(root_ca);

	client.setServer(MQTT_SERVER, MQTT_PORT);
	client.setKeepAlive(keepAliveInterval);
	client.setCallback(callback);

	delay(10);

	previousHeartbeatMillis = millis();

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
				WiFi.macAddress(mac); 
      
				TickType_t tickCount = xTaskGetTickCount();
				uint32_t uptimeMillis = (uint32_t)tickCount * portTICK_PERIOD_MS;

				char payload[500];
				snprintf(payload, sizeof(payload), "{\"UptimeMillis\": %u, \"MacAddress\": \"%02X:%02X:%02X:%02X:%02X:%02X\", \"Hostname\": \"%s\", \"CpuFreqMHz\": %d, \"ChipRevision\": %u, \"FlashChipSize\": %u, \"FlashChipSpeed\": %u, \"VccVoltage\": %.2f, \"FreeFlash\": %u}",
				uptimeMillis, mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], hostname.c_str(), ESP.getCpuFreqMHz(), ESP.getChipRevision(), ESP.getFlashChipSize(), ESP.getFlashChipSpeed(), getVccVoltage(), ESP.getFreeSketchSpace());

				client.publish(MQTT_TOPIC_HELLO, payload);

				delay(100);

				// Listen for broadcast messages
				client.subscribe("fertigationEvent");
				client.subscribe("fertigationEvent/Authorized");
				client.subscribe("yargbot/FE_ebbPump_DONE");
				client.subscribe("yargbot/FE_ebbPump_RUN");
				client.subscribe("yargbot/FE_potOverflow");
				client.subscribe("yargbot/ESTOP");
				client.subscribe("yargRPI/ESTOP");
				
				client.subscribe("yargbot/Enable");

			}
			else
			{
				digitalWrite(BUILTIN_LED, LOW);
				mqttFailCount++;
				delay(5000);
			}
		}
		else
		{
			WiFi.disconnect();
			delay(5000);
			setup_wifi();
			mqttFailCount = 0;
		}
	}
}

void callback(char* topic, byte* payload, unsigned int length){
	char message[length + 1];
	memcpy(message, payload, length);
	message[length] = '\0';

	// fertigationEvent {CommandID}:{EbbSpeed}:{EbbAmount}:{AntiShockRamp}:{SoakDuration}:{FlowSpeed}:{ExpectedEbbFlowRate}:{ExpectedFlowFlowRate}:{PumpErrorThreshold}
	
    if (strcmp(topic, "fertigationEvent") == 0) {
		// On your mark - Call from YARG server with fertigationEvent information
		parseFertigationEvent(message, &thisFertigationEvent);
		client.publish(MQTT_TOPIC_FE_EFM_ACK, "");
    }
	
	if (strcmp(topic, "fertigationEvent/Authorized") == 0) {
		// Starting gun - Authorization from YARG app that everyone ACK'd the fertigationEvent call. 
		thisFertigationEvent.IsEbbCounting = true;
		thisFertigationEvent.EbbStartMillis = millis();
	}

	if (strcmp(topic, "yargbot/FE_ebbPump_DONE") == 0) {
		// Ebb pump completed ramp down and is stopped.
		thisFertigationEvent.IsEbbCounting = false;
		// send pumpWorkLog
		client.publish(MQTT_TOPIC_FE_EFM_DN, "");
		clearFertigationEvent(&thisFertigationEvent);
	}
	
	if (strcmp(topic, "yargbot/FE_ebbPump_RUN") == 0) {
		// Ebb pump completed ramp up and is being commanded to run @ 100% of requested speed.
		thisFertigationEvent.IsCalculatingFlowRate = true;
	}

	if (strcmp(topic, "yargbot/ESTOP") == 0) {
		// Something terrible happened. Leak in cabinet, leak in grow room, ESP lost comms 
		if (!flgESTOP) {
			flgESTOP = true;
			clearFertigationEvent(&thisFertigationEvent);
		}
	}
	if (strcmp(topic, "yargRPI/ESTOP") == 0) {
		// Something terrible happened, RPI lost comms 
		if (!flgESTOP) {
			flgESTOP = true;
			clearFertigationEvent(&thisFertigationEvent);
		}
	}
	
	if (strcmp(topic, "yargbot/FE_potOverflow") == 0) {
		// Overflowed the pot. 
		thisFertigationEvent.IsEbbCounting = false;
		
		char payload[500];
		snprintf(payload, sizeof(payload), "{\"CommandID\": \"%s\", \"AdjustedEbbAmount\": %u}", thisFertigationEvent.CommandID, (unsigned int)(thisFertigationEvent.EbbCurrentAmount * 0.9));
		
		client.publish(MQTT_TOPIC_FE_OVER, payload);
	}
}

void parseFertigationEvent(const char* message, struct FertigationEventMessage* event) {
    char copy[256];  // A temporary buffer to hold a copy of the message
    strcpy(copy, message);

    char* strtokIndx;
    strtokIndx = strtok(copy, ":");
    
    // Assuming the format of the message is as expected
    strcpy(event->CommandID, strtokIndx);
    event->EbbSpeed = atoi(strtok(NULL, ":"));
    event->EbbAmount = atoi(strtok(NULL, ":"));
    event->AntiShockRamp = atoi(strtok(NULL, ":"));
    event->SoakDuration = atoi(strtok(NULL, ":"));
    event->FlowSpeed = atoi(strtok(NULL, ":"));
    event->ExpectedEbbFlowRate = atof(strtok(NULL, ":"));
    event->ExpectedFlowFlowRate = atof(strtok(NULL, ":"));
    event->PumpErrorThreshold = atof(strtok(NULL, ":"));
}

void clearFertigationEvent(struct FertigationEventMessage* event) {
    memset(event, 0, sizeof(struct FertigationEventMessage));
	// Initialize the struct members
	strcpy(thisFertigationEvent.CommandID, "");
	thisFertigationEvent.EbbSpeed = 0;
	thisFertigationEvent.EbbAmount = 0;
	thisFertigationEvent.AntiShockRamp = 0;
	thisFertigationEvent.SoakDuration = 0;
	thisFertigationEvent.FlowSpeed = 0;
	thisFertigationEvent.ExpectedEbbFlowRate = 0.0;
	thisFertigationEvent.ExpectedFlowFlowRate = 0.0;
	thisFertigationEvent.PumpErrorThreshold = 0.0;
	thisFertigationEvent.EbbStartMillis = 0;
	thisFertigationEvent.IsEbbCounting = false;
	thisFertigationEvent.IsEbbOverflow = false;
	thisFertigationEvent.IsCalculatingFlowRate = false;
	thisFertigationEvent.IsStopSent = false;
	thisFertigationEvent.EbbCurrentAmount = 0;
}

void publishHeartbeat() {
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

void calculateFlowRate() {

}

void loop(void) {
	if (!client.connected()) {
		reconnect();
	}
	
	uint16_t currentEbbAmount = thisFertigationEvent.EbbCurrentAmount;
	unsigned long currentFlowRateMillis = millis();
  uint16_t flowPulses = currentEbbAmount - previousEbbAmount;
  flowRate = static_cast<float>(flowPulses) / intervalOneSecond; // Flow rate per second


	if (currentFlowRateMillis - previousFlowRateMillis >= intervalOneSecond && thisFertigationEvent.IsCalculatingFlowRate) {
		    // Compare flow rate with ExpectedEbbFlowRate and PumpErrorThreshold
    if (flowRate >= thisFertigationEvent.ExpectedEbbFlowRate) {
		// Flow rate is good
		belowThreshold = false;
		consecutiveReadings = 0;
		// Do whatever you need to do here
    } else if (flowRate < thisFertigationEvent.PumpErrorThreshold * thisFertigationEvent.ExpectedEbbFlowRate) {
		// Flow rate is below the threshold
		belowThreshold = true;
		consecutiveReadings++;

		if (consecutiveReadings >= 2) {
			// Trigger emergency stop
			// Broadcast an emergency stop message
			// Reset consecutiveReadings and belowThreshold
			belowThreshold = false;
			consecutiveReadings = 0;
		}
		
    } else {
		// Flow rate is between the threshold and the expected
		belowThreshold = false;
		consecutiveReadings = 0;
    }

    previousFlowRateMillis = currentFlowRateMillis;
    previousEbbAmount = currentEbbAmount;

	}

	unsigned long currentMillis = millis();
	if (currentMillis - previousHeartbeatMillis >= intervalHeartbeat) {
		previousHeartbeatMillis = currentMillis; // Reset the timer

		// Randomize next heartbeat time by 2% every time
		unsigned long newInterval = generateRandomValue((unsigned long)intervalHeartbeat * 0.98, (unsigned long)intervalHeartbeat * 1.02);
		
		intervalHeartbeat = newInterval;
		
		publishHeartbeat();

	}

	client.loop();
}
