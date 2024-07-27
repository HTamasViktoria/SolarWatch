using SolarWatch.Data;

namespace SolarWatch
{
    public class CoordinateProviderApi : ICoordinateProvider
    {
        private readonly ILogger<CoordinateProviderApi> _logger;
        private readonly IJsonProcessor _jsonProcessor;
        private readonly IConfiguration _configuration;
       

        public CoordinateProviderApi(ILogger<CoordinateProviderApi> logger, IJsonProcessor jsonProcessor, IConfiguration configuration)
        {
            _logger = logger;
            _jsonProcessor = jsonProcessor;
            _configuration = configuration;

        }

        public async Task<CityData> GetData(string city)
        {
            try
            {
                Console.WriteLine("a coordinateproviderapi-ban elindul a kérés");
                //var apiKey = _configuration.GetConnectionString("WeatherApiKey");
                var apiKey = "0384aa85abf51dc6f9bc012b6d6839e5";
                var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";
                

                _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
                using var client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    CityData data = _jsonProcessor.ProcessCityData(jsonResult);
                    Console.WriteLine("in the coordinateprovider,after receiving the data from the jsonprocessor:");
                    Console.WriteLine($"name: {data.Name}");
                    Console.WriteLine($"formattedlat: {data.FormattedLatitude}");
                    Console.WriteLine($"formattedlong: {data.FormattedLongitude}");
                
                    return data;
                }
                else
                {
                    _logger.LogError($"Error while retrieving data from coordinate provider API. Status code: {response.StatusCode}");
                    throw new Exception($"Error while retrieving data from coordinate provider API. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving data from coordinate provider API");
                throw;
            }
        }
    }
}