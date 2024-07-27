using System;
using System.Globalization;
using System.Text.Json;
using SolarWatch.Controllers;
using SolarWatch.Data;


namespace SolarWatch
{
    public class JsonProcessor : IJsonProcessor
    {
        public CityData ProcessCityData(string data)
        {
            
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement city = json.RootElement[0];
          
            JsonElement coordinates = city.GetProperty("lat");
            double latitude = coordinates.GetDouble();
          
            coordinates = city.GetProperty("lon");
            
            double longitude = coordinates.GetDouble();
          
            string state = string.Empty;
            if (city.TryGetProperty("state", out JsonElement stateElement))
            {
                state = stateElement.GetString();
            }
            

            
            CityData result = new CityData()
            {
                Name = city.GetProperty("name").GetString(),
                Latitude = latitude,
                Longitude = longitude,
                State = state,
                Country = city.GetProperty("country").GetString(),
                FormattedLatitude = latitude.ToString("0.0000000", CultureInfo.InvariantCulture),
                FormattedLongitude = longitude.ToString("0.0000000", CultureInfo.InvariantCulture),
                
            };
        
            return result;
        }
        
        public SunData ProcessSunData(string data, string date)
        {
            JsonDocument json = JsonDocument.Parse(data);

            JsonElement results = json.RootElement.GetProperty("results");

            string sunriseString = results.GetProperty("sunrise").GetString();
            string sunsetString = results.GetProperty("sunset").GetString();
            
      

            SunData result = new SunData()
            {
                Date = date,
                // Convert sunrise and sunset time strings to the desired format
                SunriseDate = DateTime.Parse(sunriseString).ToString("HH:mm:ss"),
                SunsetDate = DateTime.Parse(sunsetString).ToString("HH:mm:ss")
            };

            return result;
        }


    }
}