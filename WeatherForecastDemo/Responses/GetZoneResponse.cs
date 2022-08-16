using WeatherForecastDemo.Models;

namespace WeatherForecastDemo.Responses;

public class GetZoneResponse
{
    public List<ZoneWrapper> Features { get; set; }
}