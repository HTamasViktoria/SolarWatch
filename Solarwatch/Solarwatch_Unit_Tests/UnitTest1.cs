using SolarWatch.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarWatch;
using NUnit.Framework;
using System;
using Microsoft.AspNetCore.Http;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Service.Repository;

namespace SolarWatchTests
{
    [TestFixture]
    public class SolarWatchControllerTests
    {
        private Mock<ILogger<SolarWatchController>> _loggerMock;
        private Mock<ICoordinateProvider> _coordinateProviderMock;
        private Mock<ISunsetSunriseProvider> _sunsetSunriseProviderMock;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private Mock<ISunDataRepository> _sundataRepositoryMock;
        private Mock<ICityRepository> _cityRepositoryMock;
        private SolarWatchController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<SolarWatchController>>();
            _coordinateProviderMock = new Mock<ICoordinateProvider>();
            _sunsetSunriseProviderMock = new Mock<ISunsetSunriseProvider>();
            _sundataRepositoryMock = new Mock<ISunDataRepository>();
            _cityRepositoryMock = new Mock<ICityRepository>();
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _controller = new SolarWatchController(_coordinateProviderMock.Object, _sunsetSunriseProviderMock.Object,
                _loggerMock.Object, _sundataRepositoryMock.Object, _cityRepositoryMock.Object);
        }

        [Test]
        public async Task GetReturnsInternalServerErrorIfCoordinateProviderFails()
        {
            // Arrange
            var city = "NonexistentCity";
            var date = "2024-10-10";
            _coordinateProviderMock.Setup(x => x.GetData(city)).Throws(new Exception());

            // Act
            var result = await _controller.GetSolarData(city, date);

            // Assert
            Assert.IsInstanceOf(typeof(ObjectResult), result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        }


        

        [Test]
        public async Task GetReturnsRightDatasIfEverythingOK()
        {
            // Arrange
            var city = "Rome";
            var date = "2022-10-10";
            var romeCityData = new SolarWatch.Data.CityData
            {
                Name = "Rome",
                State = "Lazio",
                Country = "Italy",
                Latitude = 41.902782,
                Longitude = 12.496366,
                FormattedLatitude = "41.902782",
                FormattedLongitude = "12.496366",
                Id = 1
            };
            var expectedSunData = new SunData
            {
                CityId = romeCityData.Id,
                Date = date,
                SunriseDate = "07:00 AM",
                SunsetDate = "07:00 PM"
            };

            _cityRepositoryMock.Setup(x => x.GetByName(city)).Returns(romeCityData);
            _sundataRepositoryMock.Setup(x => x.GetSunData(romeCityData.Id, date)).Returns(expectedSunData);

            // Act
            var result = await _controller.GetSolarData(city, date);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ActionResult<SunData>), result);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var actualSunData = okResult.Value as SunData;
            Assert.IsNotNull(actualSunData);
            Assert.AreEqual(expectedSunData.CityId, actualSunData.CityId);
            Assert.AreEqual(expectedSunData.Date, actualSunData.Date);
            Assert.AreEqual(expectedSunData.SunriseDate, actualSunData.SunriseDate);
            Assert.AreEqual(expectedSunData.SunsetDate, actualSunData.SunsetDate);
        }
        
        
        [Test]
        public async Task GetReturnsSunDataWhenCityExistsAndSunDataExists()
        {
            // Arrange
            var cityName = "Rome";
            var date = "2022-10-10";
            var romeCityData = new SolarWatch.Data.CityData
            {
                Name = "Rome",
                State = "Lazio",
                Country = "Italy",
                Latitude = 41.902782,
                Longitude = 12.496366,
                FormattedLatitude = "41.902782",
                FormattedLongitude = "12.496366",
                Id = 1
            };
            var expectedSunData = new SunData
            {
                CityId = romeCityData.Id,
                Date = date,
                SunriseDate = "07:00 AM",
                SunsetDate = "07:00 PM"
            };

            _cityRepositoryMock.Setup(x => x.GetByName(cityName)).Returns(romeCityData);
            _sundataRepositoryMock.Setup(x => x.GetSunData(romeCityData.Id, date)).Returns(expectedSunData);

            // Act
            var result = await _controller.GetSolarData(cityName, date);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf(typeof(SunData), okResult.Value);
            Assert.AreEqual(expectedSunData, okResult.Value);
        }


    }
    }
