using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcordiaAppTestLayer.SchedulerTests;

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> GetAsync(string requestUri);
}

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient httpClient;

    public HttpClientWrapper()
        => httpClient = new HttpClient();

    public Task<HttpResponseMessage> GetAsync(string requestUri)
        => httpClient.GetAsync(requestUri);
}
