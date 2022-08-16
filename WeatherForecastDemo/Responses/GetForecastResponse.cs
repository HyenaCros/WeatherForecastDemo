using WeatherForecastDemo.Models;

namespace WeatherForecastDemo.Responses;

public class GetForecastResponse
{
    public ForecastWrapper Properties { get; set; }
}