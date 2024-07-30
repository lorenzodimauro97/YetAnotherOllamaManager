namespace YetAnotherOllamaManager.Services;

using Components;
using Mapster;
using Microsoft.Extensions.Configuration;
using Models;
using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OllamaService: IDisposable
{

    private readonly HttpClient _httpClient;
    private readonly OllamaApiClient _ollamaApiClient;
    public OllamaService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        var ollamaAddress = new Uri(configuration["OllamaUri"]
                                           ?? throw new InvalidOperationException("Cannot find property 'OllamaUri' in configuration!"));
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(configuration["OllamaUri"]
                                          ?? throw new InvalidOperationException("Cannot find property 'OllamaUri' in configuration!"));
        _ollamaApiClient = new OllamaApiClient(ollamaAddress);
    }

    public async Task<List<ExtendedModel>> GetModelsAsync()
    {
        var models = await _ollamaApiClient.ListLocalModels();
        return models.Adapt<IEnumerable<ExtendedModel>>().ToList();
    }

    public async Task<DateTime?> GetLastUpdateAsync(ExtendedModel model)
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
    public async Task DeleteModelAsync(ExtendedModel model)
    {
        await _ollamaApiClient.DeleteModel(model.Name);
    }
    public async Task<ShowModelResponse> GetModelDetailsAsync(ExtendedModel model)
    {
        return await _ollamaApiClient.ShowModelInformation(model.Name);
    }

    public async Task<Stream> UpdateModelAsync(ExtendedModel model)
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
    public async Task CloneModelAsync(ExtendedModel model, string name, string parameters)
    {
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            name = name, modelfile = $"FROM {model.Name}\n{parameters}"
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/create", content);

        response.EnsureSuccessStatusCode();
    }

    public Task<Chat> GetChatInstanceAsync(string model)
    {
        _ollamaApiClient.SelectedModel = model;
        return Task.FromResult(_ollamaApiClient
            .Chat(_ => {}));
    }
    
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
