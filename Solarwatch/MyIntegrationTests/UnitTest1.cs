using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
using Xunit;

namespace MyIntegrationTests;

public class UnitTest1
{
    [Collection("IntegrationTests")]
    public class MyControllerIntegrationTest
    {
        private readonly SolarWatchWebApplicationFactory _app;
        private readonly HttpClient _client;
        
        public MyControllerIntegrationTest()
        {
            _app = new SolarWatchWebApplicationFactory();
            _client = _app.CreateClient();
        }
    
        [Fact]
        public async Task TestEndPoint()
        {
            
            string cityName = "Budapest";
            string date = DateTime.Today.ToString("yyyy-MM-dd");

            var response = await _client.GetAsync($"/SolarWatch/{cityName}/{date}");

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<SunData>();
            Assert.NotNull(data);
            Assert.IsType<SunData>(data);
            
            Assert.NotNull(data.Id);
            Assert.NotNull(data.Date);
            Assert.NotNull(data.SunriseDate);
            Assert.NotNull(data.SunsetDate);

        }
        
        
        
        [Fact]
        public async Task GetSolarData_ReturnsExpectedSunData()
        {
            // Arrange
            string cityName = "Budapest";
            string date = DateTime.Today.ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/SolarWatch/{cityName}/{date}");

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<SunData>();
            Assert.NotNull(data);
            Assert.IsType<SunData>(data);
            
            Assert.NotNull(data.CityId);
            Assert.Equal(date, data.Date);
            Assert.NotNull(data.SunriseDate);
            Assert.NotNull(data.SunsetDate);
        }
        
        
        
        [Fact]
        public async Task GetSolarData_CreatesCityIfNotExists()
        {
            // Arrange
            string cityName = "Miskolc";
            string date = DateTime.Today.ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/SolarWatch/{cityName}/{date}");

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<SunData>();
            Assert.NotNull(data);
            Assert.IsType<SunData>(data);

            Assert.NotNull(data.CityId);
            Assert.Equal(date, data.Date);
            Assert.NotNull(data.SunriseDate);
            Assert.NotNull(data.SunsetDate);
            
        }
        
        
        
        [Fact]
        public async Task GetSolarData_CallsExternalServiceIfSunDataNotFound()
        {
            // Arrange
            string cityName = "Brescia";
            string date = DateTime.Today.ToString("yyyy-MM-dd");

            // Act
            var response = await _client.GetAsync($"/SolarWatch/{cityName}/{date}");

            // Assert
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<SunData>();
            Assert.NotNull(data);
            Assert.IsType<SunData>(data);

            Assert.NotNull(data.CityId);
            Assert.Equal(date, data.Date);
            Assert.NotNull(data.SunriseDate);
            Assert.NotNull(data.SunsetDate);
            
        }
        
        
    }
}