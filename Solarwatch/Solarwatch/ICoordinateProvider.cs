using System.Threading.Tasks;
using SolarWatch.Data;

namespace SolarWatch
{
    public interface ICoordinateProvider
    {
        Task<CityData> GetData(string city);
    }
}