# SolarWatch

SolarWatch is a web API designed to manage and provide data about sunrise and sunset times for various cities. This project allows users to retrieve, add, edit, and delete sunrise and sunset data for different locations, making it easier to track solar events across the globe.

## Table of Contents

- [About The Project](#about-the-project)
- [Key Features](#key-features)
- [Built With](#built-with)
- [Usage](#usage)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [Contact](#contact)

## About The Project

### SolarWatch

SolarWatch provides a robust API to manage and retrieve solar data (sunrise and sunset times) for cities. It allows the storage, updating, and deletion of city data and their respective solar data. This project is essential for applications that need to track or display solar information based on location and date.

## Key Features

- **Retrieve Solar Data:** Get sunrise and sunset times for a given city and date.
- **Add New City:** Add new cities to the database, along with their coordinates and other relevant information.
- **Edit City Data:** Update city details such as name, state, country, latitude, and longitude.
- **Delete City Data:** Remove cities and their associated solar data from the database.
- **Manage Solar Data:** Add, edit, or delete sunrise and sunset times for specific cities and dates.

## Built With

- **Backend:**
  - C#
  - ASP.NET Core Web API
- **Data Management:**
  - Entity Framework Core
  - Microsoft SQL Server (MSSQL)
- **Logging:**
  - ILogger

## Usage

SolarWatch provides endpoints to interact with city data and their associated solar information.

1. **Retrieve Solar Data:** Send a GET request to retrieve sunrise and sunset times for a specific city and date.
2. **Add City Data:** Use the POST endpoint to add new cities to the database, ensuring that the solar data can be stored and retrieved for those locations.
3. **Edit City Data:** Modify existing city information using the PUT endpoint.
4. **Delete City Data:** Remove cities from the database using the DELETE endpoint.
5. **Manage Solar Data:** Add or update sunrise and sunset times for specific cities using the provided endpoints.

## Roadmap

- [ ] Implement advanced search features for solar data based on coordinates.
- [ ] Develop a frontend interface for easier interaction with the API.
- [ ] Integrate with weather data providers for enhanced solar predictions.
- [ ] Add support for different time zones when retrieving solar data.

## Contributing

### Top Contributor:

- **[Hajduné Tamás Viktória]** (Sole Contributor)

## Contact

[Hajduné Tamás Viktória] - [tmsvktr@gmail.com](mailto:tmsvktr@gmail.com)

Project Link: [https://github.com/HTamasViktoria/SolarWatch](https://github.com/HTamasViktoria/SolarWatch)
