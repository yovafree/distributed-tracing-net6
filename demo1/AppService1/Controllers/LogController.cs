using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AppService1.Controllers;

[ApiController]
[Route("[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogger<LogController> _logger;
    private static readonly ActivitySource Activity = new(nameof(ControllerBase));
    private readonly IHttpClientFactory _clientFactory;
    public LogController(ILogger<LogController> logger, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    [HttpGet("compute")]
    public ActionResult<double> Compute(int n, int x)
    {
        var activity = Activity.StartActivity("Cómputo", ActivityKind.Server);
        _logger.LogInformation($"Requested log compute of #{n}, base {x}");
        
        return Ok(Math.Log(n) / Math.Log(x));
    }
}