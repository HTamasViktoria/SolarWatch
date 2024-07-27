using SolarWatch.Controllers;
using SolarWatch.Data;
namespace SolarWatch;

public interface IJsonProcessor
{
    CityData ProcessCityData(string data);

    SunData ProcessSunData(string data, string date);
}