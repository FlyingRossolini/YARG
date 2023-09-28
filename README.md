# YARG: Yet Another Robotic Garden

Welcome to YARG. Inspired by LED Gardener, this project aims to design a fully automated ebb and flow hydroponic garden system.

## Project Goals
- Create a garden that operates independently for extended periods, reducing the need for frequent human interaction.
- Develop a system that doesn't rely on cloud-based services, ensuring data privacy and security.
- Implement a comprehensive scheduling system for the entire growing season, managing feeding, lighting, and environmental conditions.
- Integrate automatic pH, EC, DO and ORP measurements using Atlas Scientific sensors.

## Technologies
- **Software**: This project employs C# web applications, Bootstrap, JQuery, and Plotly JavaScript libraries to provide a user-friendly interface.
- **Hosting**: The application is compiled for ARM64 Linux and runs on a Raspberry Pi 3 with NGINX. A separate Raspberry Pi 4 hosts MariaDB for data storage and Mosquitto broker for secure MQTT communications.
- **Authentication**: Users can access the system with local accounts via Microsoft Identity.
- **Database**: The project follows a "No Entity Framework" policy, allowing for fine-tuned control over the database layer.
- **Multi-language Support**: YARG supports multiple languages, starting with English, French, Spanish and Ukrainian and plans to expand to other languages in the future.
- **IoT**: Real world interfacing with this proof of concept will be achieved via ESP32 microprocessors and reflashed Sonoff S31 Lite wifi smart plug.

## Hydroponics
- Ebb and flow design, completely scalable to individual requirements.
- Proof of concept here designed with a minimalistic approach - 25L reservoir for 4 individual 20L buckets to promote water conservation and return spillway for aeration.

## Disclaimer
- Don't plant your corn too early.

## Getting Started
To explore the YARG project in detail and follow its progress, visit our blog-in-progress at [yarg.chrisross.name](https://yarg.chrisross.name).
