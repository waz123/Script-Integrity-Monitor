
using Microsoft.AspNetCore.Components.Forms;
using Security_App__Blazor.Data.Models;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Security_App__Blazor.Data.Services;

public class ScriptCheckerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiDirectory;

    public ScriptCheckerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _apiDirectory = configuration["ApiDirectory"] ?? "API";
    }
    public async Task<List<ScriptModel>> GetScriptAsync(string url)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsJsonAsync("http://127.0.0.1:8000/check-url/", new { url });

        response.EnsureSuccessStatusCode();

        // Deserialize to the wrapper class
        var scriptResponse = await response.Content.ReadFromJsonAsync<ScriptResponse>();
        return scriptResponse?.Scripts;
    }

    public async Task<List<ScriptModel>> UploadFileAsync(IBrowserFile file)
    {
        var content = new MultipartFormDataContent();
        var stream = file.OpenReadStream(maxAllowedSize: 2048 * 2048); // Set a limit to the file size
        content.Add(new StreamContent(stream), "file", file.Name);

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.PostAsync("http://127.0.0.1:8000/check-url-file/", content);
        SaveFileAsync(file);

        if (response.IsSuccessStatusCode)
        {
            // Deserialize using the ScriptResponse class that matches the JSON structure
            var scriptResponse = await response.Content.ReadFromJsonAsync<ScriptResponse>();
            return scriptResponse?.Scripts; // Access the Scripts property
        }
        else
        {

            return new List<ScriptModel>();
        }
    }

    //overload used for background service
    public async Task<List<ScriptModel>> UploadFileAsync(string fileContent)
    {
        var httpClient = _httpClientFactory.CreateClient();

        using var content = new MultipartFormDataContent();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        using var fileContents = new StreamContent(stream);
        content.Add(fileContents, "file", "URLs.txt");

        var response = await httpClient.PostAsync("http://127.0.0.1:8000/check-url-file/", content);

        if (response.IsSuccessStatusCode)
        {
            // Deserialize using the ScriptResponse class that matches the JSON structure
            var scriptResponse = await response.Content.ReadFromJsonAsync<ScriptResponse>();
            return scriptResponse?.Scripts; // Access the Scripts property
        }
        else
        {
            return new List<ScriptModel>();
        }
    }

    public async Task<string> SaveFileAsync(IBrowserFile file)
    {
        var path = Path.Combine(_apiDirectory, file.Name);
        using var stream = file.OpenReadStream(maxAllowedSize: 2048 * 2048);
        using var fs = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fs);
        return path; //this will return the path where the file will be saved
    }
}


public class ScriptResponse
{
    public List<ScriptModel> Scripts { get; set; }
}
