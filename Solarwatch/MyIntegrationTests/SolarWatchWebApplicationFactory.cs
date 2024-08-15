using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Model;
using SolarWatch.Service;


namespace MyIntegrationTests;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
       
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SolarWatchApiContext>));
            var usersDbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<UsersContext>));
            
          
            services.Remove(solarWatchDbContextDescriptor);
            services.Remove(usersDbContextDescriptor);
            
             
            services.AddDbContext<SolarWatchApiContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
        
            using var scope = services.BuildServiceProvider().CreateScope();
            
           
            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchApiContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            var userContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
            userContext.Database.EnsureDeleted();
            userContext.Database.EnsureCreated();

  
        });
    }
}