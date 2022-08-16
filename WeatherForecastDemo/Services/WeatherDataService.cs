using System.Net.Http.Json;
using System.Reactive.Subjects;
using WeatherForecastDemo.Models;
using WeatherForecastDemo.Responses;

namespace WeatherForecastDemo.Services;

public class WeatherDataService
{
    private const string BaseUri = "https://api.weather.gov";
    
    private readonly HttpClient _httpClient;
    private readonly BehaviorSubject<Dictionary<State, List<Zone>>> _zones = new(new());
    private readonly BehaviorSubject<Dictionary<string, List<Forecast>>> _forecasts = new(new());

    public IObservable<Dictionary<State, List<Zone>>> Zones => _zones;
    public IObservable<Dictionary<string, List<Forecast>>> Forecasts => _forecasts;

    public WeatherDataService()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(BaseUri)
        };
    }

    public async void GetZones(State state)
    {
        if (_zones.Value.ContainsKey(state))
            return;
        try
        {
            var response = await _httpClient.GetFromJsonAsync<GetZoneResponse>($"/zones/public?area={Enum.GetName(state)}");
            _zones.Value[state] = response!.Features.Select(x => x.Properties).OrderBy(x => x.Name).ToList();
            _zones.OnNext(_zones.Value);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
        
    }

    public async void GetForecasts(string zone)
    {
        if (_forecasts.Value.ContainsKey(zone))
            return;
        try
        {
            var response = await _httpClient.GetFromJsonAsync<GetForecastResponse>($"/zones/public/{zone}/forecast");
            _forecasts.Value[zone] = response!.Properties.Periods;
            _forecasts.OnNext(_forecasts.Value);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}