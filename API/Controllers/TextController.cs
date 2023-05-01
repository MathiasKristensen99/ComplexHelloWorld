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
public class TextController : ControllerBase
{
    private static readonly ILogger L = LogContainer.Log;
    
    [HttpGet]
    public async Task<IActionResult> Get(string languageCode)
    { 
        L.Debug("Received HTTP request");
        
        var bus = ConnectionHelper.GetRabbitMQConnection();
        try
        {
            L.Debug("RMQ connected");
            L.Debug("Sending request for a greeting");
            var greetingTask = bus.Rpc.RequestAsync<GreetingRequest, GreetingResponse>(new GreetingRequest { LanguageCode = languageCode });
            L.Debug("Sending request for a planet");
            var planetTask = bus.Rpc.RequestAsync<PlanetRequest, PlanetResponse>(new PlanetRequest());

            await Task.WhenAll(greetingTask, planetTask);

            var response = new GetGreetingModel.Response
            {
                Greeting = greetingTask.Result.Greeting,
                Planet = planetTask.Result.Planet
            };
            L.Debug("Received greeting: " + response.Greeting);
            L.Debug("Received planet: " + response.Planet);
            return Ok(response);
        }
        catch(TaskCanceledException ex)
        {
            L.Error(ex.Message, ex);
            return BadRequest();
        }
    }
}