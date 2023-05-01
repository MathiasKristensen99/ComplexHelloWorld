using System.Data;
using EasyNetQ;
using MySql.Data.MySqlClient;

namespace Helpers;

public static class ConnectionHelper
{
    public static IBus GetRabbitMQConnection()
    {
        return RabbitHutch.CreateBus($"host=rmq;username=application;password=pepsi");
    }

    public static IDbConnection GetMySQLConnection()
    {
        return new MySqlConnection($"Server=mysqldb;Database=HelloWorld;Uid=application;Pwd=pepsi");
    }
}