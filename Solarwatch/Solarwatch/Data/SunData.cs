namespace SolarWatch.Data;

public class SunData
{
    public int Id { get; init; }
    public int CityId { get; init; }
    public string Date { get; init; }
    public string SunriseDate { get; set; }
    public string SunsetDate { get; set; }
    
}