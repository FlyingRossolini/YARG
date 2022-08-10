# YARG
Yet Another Robotic Garden. Adventures in architecting a minimalistic approach to an automated ebb and flow hydroponic garden.

## Design Considerations
* Completely autonomous, where it can be left without human intervention for weeks at a time.
* Works without internet connection (no cloud based services)
* Schedule entire growing season on per week basis including feeding schedule, lighting schedule, environment and reservior upper / lower control limits, ebb and flow frequency and duration.
* pH and EC readings taken automatically via Atlas Scientific sensors.
* Up to 4 plants total, independantly ebb'd and flowed schedule from a 20L reservoir (one plant is watered at a time). Each pot is a 3" net cup in a 20L plastic pail. Mathematically, it should be possible to water each pot individually. In practice, this is not the norm - a reservoir is usually much bigger in order to more easily maintain pH stabilty. 

## Grow Room
* Current setup is a 4' x 5' physical room - each pot (up to 4) will house a plant that will occupy a 2' x 2' area. Each plant will have a Spider Farmer SF1000 led grow light mounted directly above it. 
* Environmental elements (temperature & humidity) are monitored and adjusted if they fall outside of control limits.
![room](https://user-images.githubusercontent.com/104273089/183456428-f9fa0bb5-8f7b-4a57-be50-5460d07d6436.jpg)

## Buckets
* 20L plastic bucket, complete with 3" net pot top and with a false bottom. The bottom is plumbed with a 1/2" double threaded bulkhead (bucket side has a strainer and a protetive filter to prevent roots clogging the drain). In the false bottom is a 12v pump, water flow senser and solenoid switch.
![hnp8pk0v9fm81](https://user-images.githubusercontent.com/104273089/183784625-c858e48f-5400-41cb-9646-e33b1f01a324.jpg)
![e1dasoyw9fm81](https://user-images.githubusercontent.com/104273089/183784642-e61c8769-ef9a-457f-86ef-1f67565d1fe0.jpg)
