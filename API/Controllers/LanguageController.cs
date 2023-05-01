using API.Models;
using EasyNetQ;
using Helpers;
using Logging;
using Messages;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    private static readonly ILogger L = LogContainer.Log;
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        L.Debug("Received HTTP request");
        
        var bus = ConnectionHelper.GetRabbitMQConnection();
        try
        {
            L.Debug("RMQ connected");
            L.Debug("Sending request for languages");
            var language = await bus.Rpc.RequestAsync<LanguageRequest, LanguageResponse>(new LanguageRequest());
            L.Debug("Received languages: " + string.Join(", ", language.Languages));
            L.Debug("Received default language: " + language.DefaultLanguage);
            return Ok(new GetLanguageModel.Response { DefaultLanguage = language.DefaultLanguage, Languages = language.Languages });
        }
        catch(TaskCanceledException ex)
        {
            L.Error(ex.Message, ex);
            return BadRequest();
        }
    }
}