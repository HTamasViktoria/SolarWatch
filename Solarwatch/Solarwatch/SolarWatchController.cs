using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SolarWatch.Data;
using SolarWatch.Service.Repository;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarWatchController : ControllerBase
    {
        private readonly ICoordinateProvider _coordinateProvider;
        private readonly ISunsetSunriseProvider _sunsetSunriseProvider;
        private readonly ILogger<SolarWatchController> _logger;
        private ISunDataRepository _sunDataRepository;
        private ICityRepository _cityRepository;

        public SolarWatchController(ICoordinateProvider coordinateProvider, ISunsetSunriseProvider sunsetSunriseProvider,
            ILogger<SolarWatchController> logger, ISunDataRepository sunDataRepository, ICityRepository cityRepository)
        {
            _coordinateProvider = coordinateProvider;
            _sunsetSunriseProvider = sunsetSunriseProvider;
            _logger = logger;
            _sunDataRepository = sunDataRepository;
            _cityRepository = cityRepository;

        }
        
       
        [HttpGet("{cityName}/{date}", Name = "GetSolarData")]
        public async Task<ActionResult<SunData>> GetSolarData(string cityName, string date)
        {
            try
            {
                var city = _cityRepository.GetByName(cityName);

                if (city != null)
                {
                    var result = _sunDataRepository.GetSunData(city.Id, date);

                    if (result == null)
                    {
                        var sunriseAndSunset = await UseSunsetSunriseProvider(city, date);
                        return Ok(sunriseAndSunset);
                    }

                    return Ok(result);
                }
                else
                {
                    var cityObject = await CreateCityData(cityName);
                    var sunriseAndSunset = await UseSunsetSunriseProvider(cityObject, date);
                    return Ok(sunriseAndSunset);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("cities/addnew"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<CityData>> AddNewCityData(string cityName)
        {
            var isAlreadyExistingInDb = _cityRepository.GetByName(cityName);
            if (isAlreadyExistingInDb == null)
            {
                Console.WriteLine("hívjuk a private függvényt, mivel nincs ilyen a db-ben");
                var newCity = await this.CreateCityData(cityName);
                return Ok(newCity);
            }
            else
            {
                return BadRequest("This city is already in the db.");
            }
           
        }

        
        [HttpDelete("cities/delete"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<CityData>> DeleteCityData(string cityName)
        {
            var city =  _cityRepository.GetByName(cityName);
            if (city != null)
            {
               _cityRepository.Delete(city);
                return Ok(city);
            }
            else
            {
                return BadRequest("Non-existing city in db.");
            }
        }


        [HttpPut("cities/edit"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<CityData>> EditCityData(string cityName, string? state, string? country,
            double latitude, double longitude)
        {
            var city = _cityRepository.GetByName(cityName);

            if (city != null)
            {
                if (state != null) city.State = state;
                if (country != null) city.Country = country;
                city.Latitude = latitude;
                city.Longitude = longitude;
                city.FormattedLatitude = latitude.ToString("0.0000000", CultureInfo.InvariantCulture);
                city.FormattedLongitude = longitude.ToString("0.0000000", CultureInfo.InvariantCulture);

                _cityRepository.Update(city);
                return Ok(city);
            }
            else
            {
                return BadRequest("City not found in db.");
            }
        }

        
        
        
        [HttpPost("sundata/addnew"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<SunData>> AddNewSunData(string cityName, string date, string sunriseDate, string sunsetDate)
        {
           //először ellenőrizni, hogy van-e már olyan város a db-ben, ha van akkor az id-jával
           //hozzuk létre a new sundata-t
           //ha nincs, akkor először létrekell hozni a cityData-t, és utána a sundata-t
           var city = _cityRepository.GetByName(cityName);
           if (city != null)//ha volt már ilyen város
           {
               var sunriseAndSunset = await this.CreateNewSunData(city.Id, date, sunriseDate, sunsetDate);
               return Ok(sunriseAndSunset);
           }
           else
           {
               var newCity =  await this.CreateCityData(cityName);
               var sunriseAndSunset = await this.CreateNewSunData(newCity.Id, date, sunriseDate, sunsetDate);
               return Ok(sunriseAndSunset);
           }
        }
        
        
        
        [HttpPost("sundata/delete"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<SunData>> DeleteSunData(int sunDataId)
        {
           //ellenőrizni, hogy van-e olyan sundata
           var sunData = _sunDataRepository.GetSunDataById(sunDataId);
           if (sunData != null)
           {
               _sunDataRepository.Delete(sunData);
               return Ok(sunData);
           }
           else
           {
               return BadRequest("Sundata with this id not found");
           }
          
        }
        
        
        
        
        
                
        [HttpPost("sundata/edit"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<SunData>> EditSunData(string cityName, string date, string sunriseDate, string sunsetDate)
        {
            var city = _cityRepository.GetByName(cityName);
            if (city == null)
            {
                return BadRequest("no sun data found");
            }
            else
            {
                var sunData = _sunDataRepository.GetSunData(city.Id, date);
                if (sunData == null)
                {
                    return BadRequest("no sun data found");
                }
                else
                {
                    sunData.SunriseDate = sunriseDate;
                    sunData.SunsetDate = sunsetDate;
                    _sunDataRepository.Update(sunData);
                    return Ok(sunData);
                }
            }


        }
        
        


        private async Task<SunData> CreateNewSunData(int cityId, string date, string sunriseDate, string sunsetDate)
        {
            var sunriseAndSunset = new SunData
            {
                CityId = cityId,
                Date = date,
                SunriseDate = sunriseDate,
                SunsetDate = sunsetDate,
            };
            _sunDataRepository.Add(sunriseAndSunset);
            return sunriseAndSunset;
        }
        
        
        
        private async Task<CityData> CreateCityData(string cityName)
        {
            CityData cityData = await _coordinateProvider.GetData(cityName);
            var newCity = new CityData
            {
                Name = cityName,
                State = cityData.State,
                Country = cityData.Country,
                Latitude = cityData.Latitude,
                Longitude = cityData.Longitude,
                FormattedLatitude = cityData.FormattedLatitude,
                FormattedLongitude = cityData.FormattedLongitude,
            };
        
            _cityRepository.Add(newCity);
            return newCity;
        }
        
        
        private async Task<SunData> UseSunsetSunriseProvider(CityData city, string date)
        {
            SunData sunDataByProvider = await _sunsetSunriseProvider.GetSunDataAsync(city, date);
            var sunriseAndSunset = await this.CreateNewSunData(city.Id, date, sunDataByProvider.SunriseDate, sunDataByProvider.SunsetDate);
            return sunriseAndSunset;
        }


    
      
    }


    }