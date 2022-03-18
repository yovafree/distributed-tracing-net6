using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AppService1.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculoController : ControllerBase
{
    private readonly ILogger<CalculoController> _logger;
    private static readonly ActivitySource Activity = new(nameof(ControllerBase));
    private readonly IHttpClientFactory _clientFactory;

    public CalculoController(ILogger<CalculoController> logger, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    [HttpGet("log")]
    public async Task<ActionResult> ComputeLog(int n, int x)
    {
        var activity = Activity.StartActivity("Inicia Cómputo", ActivityKind.Server);
        _logger.LogInformation($"Se computan n {n} x {x}");

        var client = _clientFactory.CreateClient("logService");
        //var response = await client.GetAsync($"http://localhost:5289/log/compute?n={n}&x={x}");
        var response = await client.GetAsync($"http://localhost:5062/log/compute?n={n}&x={x}");
        return response.IsSuccessStatusCode
            ? Ok(Convert.ToDouble(await response.Content.ReadAsStringAsync()))
            : Problem("Falló el servicio");
    }
}
