using SolarWatch.Controllers;
using SolarWatch.Data;

namespace SolarWatch;

public interface ISunsetSunriseProvider
{
    Task<SunData> GetSunDataAsync
        (CityData cityData, string date);
}