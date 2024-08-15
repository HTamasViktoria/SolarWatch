using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Service
{
    public class SolarWatchApiContext : DbContext
    {
        public DbSet<CityData> Cities { get; set; }
        public DbSet<SunData> SunDatas { get; set; }

        public SolarWatchApiContext(DbContextOptions<SolarWatchApiContext> options)
            : base(options)
        {
        }
        
        
  
    }
}