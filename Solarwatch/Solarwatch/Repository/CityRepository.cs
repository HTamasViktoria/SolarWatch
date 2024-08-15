using SolarWatch.Data;

namespace SolarWatch.Service.Repository;

public class CityRepository : ICityRepository
{
    private SolarWatchApiContext dbContext;

    public CityRepository(SolarWatchApiContext context)
    {
        dbContext = context;
    }

    public IEnumerable<CityData> GetAll()
    {
        return dbContext.Cities.ToList();
    }

    public CityData? GetByName(string name)
    {
        return dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void Add(CityData city)
    {
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(CityData city)
    {
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void Update(CityData city)
    {  
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}