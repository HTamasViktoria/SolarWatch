using SolarWatch.Data;
using System.Net;
namespace SolarWatch;

public class SunsetSunriseProvider : ISunsetSunriseProvider
{
    private readonly ILogger<SunsetSunriseProvider> _logger;
    private readonly IJsonProcessor _jsonProcessor;

    public SunsetSunriseProvider(ILogger<SunsetSunriseProvider> logger, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
    }
    
    public async Task<SunData> GetSunDataAsync(CityData cityData, string date)
    {
        try
        {
         
            var url = $"https://api.sunrise-sunset.org/json?lat={cityData.FormattedLatitude}&lng={cityData.FormattedLongitude}&date={date}";
    
            using var client = new HttpClient();
    
            _logger.LogInformation("Calling SunriseAndSunset API with url: {url}", url);
    
            var jsonResult = await client.GetAsync(url);
            var res = await jsonResult.Content.ReadAsStringAsync();
    
            SunData data = _jsonProcessor.ProcessSunData(res, date);
        
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving data from coordinate provider API");
            throw; 
        }
    }


}