using System.Runtime.InteropServices.JavaScript;
using SolarWatch.Data;

namespace SolarWatch.Service.Repository;

public class SunDataRepository : ISunDataRepository
{
    private SolarWatchApiContext dbContext;

    public SunDataRepository(SolarWatchApiContext context)
    {
        dbContext = context;
    }

    public IEnumerable<SunData> GetAll()
    {
        return dbContext.SunDatas.ToList();
    }

    public SunData? GetSunData(int cityId, string date)
    {
        return dbContext.SunDatas.FirstOrDefault(c => c.CityId == cityId && c.Date == date);
    }

    public SunData? GetSunDataById(int sunDataId)
    {
        return dbContext.SunDatas.FirstOrDefault(c => c.Id == sunDataId);
    }

    public void Add(SunData sunData)
    {
        dbContext.Add(sunData);
        dbContext.SaveChanges();
    }

    public void Delete(SunData sunData)
    {
        dbContext.Remove(sunData);
        dbContext.SaveChanges();
    }

    public void Update(SunData sunData)
    {  
        dbContext.Update(sunData);
        dbContext.SaveChanges();
    }
}