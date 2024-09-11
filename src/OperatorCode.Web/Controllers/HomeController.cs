using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperatorCode.Web.Clients;
using OperatorCode.Web.Models;

namespace OperatorCode.Web.Controllers;

public class HomeController : Controller
{
    private readonly OperatorHttpClient _operatorHttpClient;
    private readonly HttpClient _httpClient;

    public HomeController(OperatorHttpClient operatorHttpClient, IHttpClientFactory httpClientFactory)
    {
        _operatorHttpClient = operatorHttpClient;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<ActionResult<IEnumerable<Operator>>> Index()
    {
        var operators = await _operatorHttpClient.GetAllAsync();
        if (!operators.Success)
        {
            foreach (var error in operators.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        return View(operators.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Operator model)
    {
        if (ModelState.IsValid)
        {
            var response = await _operatorHttpClient.Create(model);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in response.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int code)
    {
        var operators = await _operatorHttpClient.Update(code);
        if (!operators.Success)
        {
            foreach (var error in operators.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        return View(operators.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Operator model)
    {
        if (ModelState.IsValid)
        {
            var response = await _operatorHttpClient.Update(model);

            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in response.Errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int code)
    {
        await _operatorHttpClient.Delete(code);
        return RedirectToAction(nameof(Index));
    }

    private async Task HandleErrors(HttpResponseMessage response)
    {
        var errorContent = await response.Content.ReadAsStringAsync();
        var responseJson = JsonConvert.DeserializeObject<ValidationProblemDetails>(errorContent);

        if (responseJson is not null)
        {
            foreach (var error in responseJson.Errors)
            {
                foreach (var message in error.Value)
                {
                    ModelState.AddModelError(error.Key, message);
                }
            }
        }
    }
}