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
