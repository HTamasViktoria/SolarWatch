using SolarWatch.Data;

namespace SolarWatch.Service.Repository;


public interface ICityRepository
{
    IEnumerable<CityData> GetAll();
    CityData? GetByName(string name);

    void Add(CityData city);
    void Delete(CityData city);
    void Update(CityData city);
}