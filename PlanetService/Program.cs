// See https://aka.ms/new-console-template for more information

using System;
using System.Threading;
using Dapper;
using EasyNetQ;
using Helpers;
using Logging;
using MySql.Data.MySqlClient;
using Serilog;

static class Program
{
    private static readonly ILogger L = LogContainer.Log;
    private static readonly IBus Bus = ConnectionHelper.GetRabbitMQConnection();

    static void Main(string[] args)
    {
        L.Debug("Up and running");
        Bus.Rpc.RespondAsync<PlanetRequest, PlanetResponse>(request =>
        {
            L.Debug("Received a request for a planet");
            using (var con = ConnectionHelper.GetMySQLConnection())
            {
                L.Debug("Connected to MySQL server");
                var response = new PlanetResponse();
                response.Planet = con.QuerySingleOrDefault<string>("SELECT `Name` FROM Planets ORDER BY RAND() LIMIT 1");
                L.Debug("Found a greeting: " + response.Planet);
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
                L.Information("Listening for planet requests");
            }
        });

        while (true)
        {
            Thread.Sleep(1000);
        }
    }
}