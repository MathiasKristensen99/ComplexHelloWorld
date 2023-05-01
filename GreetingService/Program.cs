using Dapper;
using EasyNetQ;
using Helpers;
using Logging;
using Messages;
using MySql.Data.MySqlClient;
using Serilog;

namespace GreetingService;

static class Program
{
    private static readonly ILogger L = LogContainer.Log;
    private static readonly IBus Bus = ConnectionHelper.GetRabbitMQConnection();
    private static IDisposable responder;
    
    static void Main(string[] args)
    {
        L.Debug("Up and running");
        Bus.Rpc.RespondAsync<GreetingRequest, GreetingResponse>(request =>
        {
            L.Debug("Received a request for a greeting with language: " + request.LanguageCode);
            using (var con = ConnectionHelper.GetMySQLConnection())
            {
                L.Debug("Connected to MySQL server");
                var response = new GreetingResponse();
                response.Greeting = con.QuerySingleOrDefault<string>("SELECT `Word` FROM Greetings WHERE `Language`=?", new { Language = request.LanguageCode });
                L.Debug("Found a greeting: " + response.Greeting);
                return response;
            }
        }).AsTask().ContinueWith(t =>
        {
            if (t.Status != TaskStatus.RanToCompletion)
            {
                L.Error("Unable to connect to RabbitMQ");
                Environment.Exit(1);
            }
            else
            {
                responder = t.Result;
                L.Information("Listening for greeting requests");
            }
        });

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}
