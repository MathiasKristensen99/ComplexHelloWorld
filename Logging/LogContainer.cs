using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace Logging;

public static class LogContainer
{
    private static ILogger? _log;
    public static ILogger Log
    {
        get
        {
            if (_log == null)
            {
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                var config = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.WithProperty("Layer", Assembly.GetEntryAssembly()?.GetName()?.Name ?? "Unknown")
                    .WriteTo.Console();
                _log = config.CreateLogger();
            }

            return _log;
        }
    }
}