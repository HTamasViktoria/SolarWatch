using SolarWatch.Data;

namespace SolarWatch.Service.Repository;

public interface ISunDataRepository
{
    IEnumerable<SunData> GetAll();
    SunData? GetSunData(int cityId, string date);
    public SunData? GetSunDataById(int sunDataId);

    void Add(SunData sunData);
    void Delete(SunData sunData);
    void Update(SunData sunData);
}