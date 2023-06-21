namespace Scheduler;

public class ConnectionChecker : IConnectionChecker
{
    private readonly HttpClient _httpClient;

    public ConnectionChecker(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckConnection()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://api.trello.com");
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}