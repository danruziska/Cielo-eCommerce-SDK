using NLog;
using System.Collections.Generic;
using System.Diagnostics;

namespace RLabs.Cielo.SDK.Util
{
    internal sealed class LogTools
    {
        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private Stopwatch watch;
        private string methodName;
        
        internal void Start(string methodName)
        {
            this.methodName = methodName;
            watch = Stopwatch.StartNew();
        }

        internal void End()
        {            
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            logger.DebugWithMetadata(new Dictionary<string, string>() { { "Message", "Execução do método " + this.methodName + ":" + elapsedMs.ToString() + "ms" } });
        }
    }
}
