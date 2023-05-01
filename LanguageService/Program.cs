// See https://aka.ms/new-console-template for more information

using Dapper;
using EasyNetQ;
using Helpers;
using Logging;
using Messages;
using Serilog;

namespace LanguageService;

static class Program
{
    private static readonly ILogger L = LogContainer.Log;
    private static readonly IBus Bus = ConnectionHelper.GetRabbitMQConnection();
    private static IDisposable responder;
    
    static void Main(string[] args)
    {
        L.Debug("Up and running");
        Bus.Rpc.RespondAsync<LanguageRequest, LanguageResponse>(request =>
        {
            L.Debug("Received a request for a list of languages");
            using (var con = ConnectionHelper.GetMySQLConnection())
            {
                L.Debug("Connected to MySQL server");
                var response = new LanguageResponse();
                response.Languages = con.Query<string>("SELECT DISTINCT `Language` FROM Greetings").ToArray();
                L.Debug("Found languages: " + string.Join(", ", response.Languages));
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