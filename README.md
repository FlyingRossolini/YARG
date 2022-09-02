# YARG
**Y**et **A**nother **R**obotic **G**arden. Not that there are many out there mind you. 'Adventures in architecting a minimalistic approach to an automated ebb and flow hydroponic garden' seemed too long.

## General Design Considerations (aka incoherant rambling)
* Completely autonomous, where it can be left without human intervention for weeks at a time.
* Works without internet connection (no cloud based services)
* Schedule entire growing season on per week basis including feeding, lighting, environment (temp and humidity) and reservior upper / lower control limits, ebb and flow frequency, duration and amount.
* pH and EC readings taken automatically via Atlas Scientific sensors.
* Up to 4 plants total, independantly ebb'd and flowed schedule from a 20L reservoir (one plant is watered at a time). Each pot is a 3" net cup in a 20L plastic pail. Mathematically, it should be possible to water each pot individually. In practice, this is not the norm - a reservoir is usually much bigger in order to more easily maintain pH stabilty. 
* Flood detection for each pot - I expect that the roots will grow sufficiently that it will cause water to overflow the pot. This will be detected via a moisture sensor and will signal the server to adjust the nutrient amount. Warning notification to Admin role; email? Email to text? TBD
* Maintenance tracking for pumps.
* Inventory tracking of chemicals; interface for automatic procurement of chems when in-stock amount reaches threshold?

## Automation Considerations
* Grow room environmental conditions (temp and humidity) recorded and adjusted if they are outside parameters. If humidity too high, turn on dehumidifier. If humidity too low, turn on humidifier. Same idea with the room temp. All external applicances will be controlled via Sonoff S31 Lite reflased with Tasmota, controlled via MQTT. (TO-DO) 
* Reservoir temperature recorded and adjusted, same principle as above except pump reservoir contents through external aquiarium heater or water chiller (TO-DO) 
* Chemical jars are mixed on individual schedule. 4 wire Arctic F8 fans are commanded to spin at certain rpm (depandant on amount of chemical remaining in jar) rpm measured back.

## Software Design Considerations
* MVC design pattern, C# (VS2019 .Net5), Bootstrap, JQuery, Plotly javascript libraries.
* Compiled for ARM64 linux and installed on RPI3 + nginx. (RPI3 is what I had laying around at the time. And yes, a .net5 app can be deployed on it and it runs reasonably well!) 
* Database - MariaDB on RPI4
* Authentication - local user account + Google sign-on via Micorosft Identity. Add other 3rd party sign-ons (fb, microsoft, etc)
* No Entity Framework tyvm. If that's your bag, that's awesome. My beef with EF is purely personal. 
* At the end of the day, I needed a project where I could ... keep relevant I guess? Trying to keep the old skills honed up and learn a few new things along the way.

## Microcontroller Design Considerations
* ESP32s will be interfacing with the outisde world; controlling relays for solenoids, perstaltic pumps, circulatory pumps.
* ATMega will measure pH and EC via Atlas Scientific sensors, isolation board. ATMega serial comms to ESP32, relay info back to server.
* YARG server communicates to ESP32 via MQTT (SSL)
* ESP32 communicates to YARG server via webapi (SSL part is TO-DO)

## Grow Room
* Current setup is a 4' x 5' physical room - a pot (up to 4) will house a plant that will occupy a 2' x 2' area. Each plant will have a Spider Farmer SF1000 led grow light mounted directly above it. Grow lights will be turned on/off via the app at prescribed intervals. 
* Environmental elements (temperature & humidity) are monitored and adjusted if they fall outside of control limits.
![room](https://user-images.githubusercontent.com/104273089/183456428-f9fa0bb5-8f7b-4a57-be50-5460d07d6436.jpg)

## Buckets
* 20L plastic bucket, complete with 3" net pot top and with a false bottom. The bottom is plumbed with a 1/2" double threaded bulkhead (bucket side has a strainer and a protetive filter to prevent roots clogging the drain). In the false bottom is a 12v pump, water flow senser and solenoid switch.
![hnp8pk0v9fm81](https://user-images.githubusercontent.com/104273089/183784625-c858e48f-5400-41cb-9646-e33b1f01a324.jpg)
![e1dasoyw9fm81](https://user-images.githubusercontent.com/104273089/183784642-e61c8769-ef9a-457f-86ef-1f67565d1fe0.jpg)
