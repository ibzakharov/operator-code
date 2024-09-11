using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperatorCode.Web.Models;

namespace OperatorCode.Web.Clients;

public class OperatorHttpClient
{
    private readonly HttpClient _client;

    public OperatorHttpClient(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _client.BaseAddress = new Uri(configuration.GetValue<string?>("ApiServiceUrl"));
    }

    public async Task<ResponseData<IEnumerable<Operator>>> GetAllAsync()
    {
        var result = new ResponseData<IEnumerable<Operator>>();
        var response = await _client.GetAsync("/operators");
        result.Success = response.IsSuccessStatusCode;

        if (result.Success)
        {
            var json = await response.Content.ReadAsStringAsync();
            result.Data = JsonConvert.DeserializeObject<IEnumerable<Operator>>(json);
        }
        else
        {
            result.Errors = await HandleErrors(response);
        }

        return result;
    }

    public async Task<Response> Create(Operator model)
    {
        var result = new Response();

        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/operators", content);
        result.Success = response.IsSuccessStatusCode;

        if (!result.Success)
        {
            result.Errors = await HandleErrors(response);
        }

        return result;
    }

    [HttpGet]
    public async Task<ResponseData<Operator>> Update(int code)
    {
        var result = new ResponseData<Operator>();
        var response = await _client.GetAsync($"/operators/{code}");
        result.Success = response.IsSuccessStatusCode;

        if (result.Success)
        {
            var json = await response.Content.ReadAsStringAsync();
            result.Data = JsonConvert.DeserializeObject<Operator>(json);
        }
        else
        {
            result.Errors = await HandleErrors(response);
        }

        return result;
    }

    public async Task<ResponseData<Operator>> Update(Operator model)
    {
        var result = new ResponseData<Operator>();
        var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        var response = await _client.PutAsync($"/operators/{model.Code}", content);
        result.Success = response.IsSuccessStatusCode;

        if (!result.Success)
        {
            result.Errors = await HandleErrors(response);
        }

        return result;
    }

    public async Task Delete(int code)
    {
        await _client.DeleteAsync($"/operators/{code}");
    }
    
    private async Task<Dictionary<string, string>> HandleErrors(HttpResponseMessage response)
    {
        var result = new Dictionary<string, string>();

        var errorContent = await response.Content.ReadAsStringAsync();
        var responseJson = JsonConvert.DeserializeObject<ValidationProblemDetails>(errorContent);

        if (responseJson is not null)
        {
            foreach (var error in responseJson.Errors)
            {
                foreach (var message in error.Value)
                {
                    result.Add(error.Key, message);
                }
            }
        }

        return result;
    }
}