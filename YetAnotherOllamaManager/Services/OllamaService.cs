namespace YetAnotherOllamaManager.Services;

using Components;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OllamaService: IDisposable
{

    private readonly HttpClient _httpClient;
    public OllamaService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(configuration["OllamaUri"]
                                          ?? throw new InvalidOperationException("Cannot find property 'OllamaUri' in configuration!"));
    }

    public async Task<List<Model>> GetModelsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<GetModelListResponse>("/api/tags");
        return result?.Models ?? [];
    }

    public async Task<DateTime?> GetLastUpdateAsync(Model model)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get, RequestUri = new Uri($"https://ollama.com/library/{model.Name}"),
        };
        var response = await _httpClient.SendAsync(request);

        switch (response.IsSuccessStatusCode)
        {
            case false when response.StatusCode != HttpStatusCode.NotFound:
                throw new InvalidOperationException($"Request failed with status code {response.StatusCode}");
            case false:
                return null;
            default:
            {
                var result = await response.Content.ReadAsStringAsync();

                var date = result.GetSecondUpdatedDateTime();

                return date;
            }
        }
    }
    public async Task DeleteModelAsync(Model model)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            name = model.Name
        }), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete, RequestUri = new Uri(_httpClient.BaseAddress ?? throw new InvalidOperationException("HttpClient Base Uri is null"), "api/delete"), Content = content
        };
        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }
    public async Task<OllamaModelInformationResult?> GetModelDetailsAsync(Model model)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            name = model.Name
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/show", content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<OllamaModelInformationResult>();
    }

    public async Task<Stream> UpdateModelAsync(Model model)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
        new Uri(_httpClient.BaseAddress ?? throw new InvalidOperationException("HttpClient Base Uri is null"),
        "api/pull"));
        request.Content = new StringContent(JsonSerializer.Serialize(new
        {
            name = model.Name
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }
    public async Task<ModelInfoFromInternet?> GetModelsFromInternetAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
        new Uri("https://ollama-models.zwz.workers.dev/"));


        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ModelInfoFromInternet>();
    }
    public async Task CloneModelAsync(Model model, string name, string parameters)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            name = name, modelfile = $"FROM {model.Name}\n{parameters}"
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/create", content);

        response.EnsureSuccessStatusCode();
    }
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
