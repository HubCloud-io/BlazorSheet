using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Company.WebApplication1.DataProviders;

public class RequestHelper
{
     private readonly HttpClient _http;

    public RequestHelper(HttpClient http)
    {
        _http = http;
    }

    public async Task<T> MakeGetAsync<T>(string url) where T : new()
    {
        T result = default(T);

#if DEBUG
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: {url} Request");
#endif

        var response = await _http.GetAsync(url);

#if DEBUG
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: {url} Request.Done");
#endif

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

#if DEBUG
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Content.Read");
#endif

            try
            {
                result = JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Parse error. Message: {e.Message}. Content: {responseContent}");
            }

#if DEBUG
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Parse.Done");
#endif

        }
        else
        {
            Console.WriteLine($@"Request error. {response.StatusCode} {response.ReasonPhrase}");
        }

        if (result == null)
        {
            result = new T();
        }

        return result;
    }


    public async Task<T> MakePostAsync<T>(string url, object payload) where T : new()
    {

#if DEBUG
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: {url} POST.Request");
#endif

        T result = default(T);

        var json = JsonConvert.SerializeObject(payload, Formatting.None,
            new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

        var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, jsonContent);

#if DEBUG
        Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: {url} POST.Request.Done");
#endif

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

#if DEBUG
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Content.Read");
#endif

            try
            {
                result = JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Parse error. Message: {e.Message}. Content: {responseContent}");
            }

#if DEBUG
            Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd hh:mm:ss.fff}: Parse.Done");
#endif

        }
        else
        {
            Console.WriteLine($@"Cannot get view model. {response.StatusCode} {response.ReasonPhrase}");
        }

        if (result == null)
        {
            result = new T();
        }

        return result;
    }
}