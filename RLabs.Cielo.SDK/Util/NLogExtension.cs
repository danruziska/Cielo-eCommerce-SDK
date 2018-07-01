using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RLabs.Cielo.SDK.Util
{
    public static class NLogExtension
    {
        private const string dataMask = "data: {@value}";

        public static void InfoWithMetadata(this ILogger logger, Dictionary<string, string> values)
        {
            AddMetadataInfo(values);
            logger.Info(dataMask, values);
        }

        public static void DebugWithMetadata(this ILogger logger, Dictionary<string, string> values)
        {
            AddMetadataInfo(values);
            logger.Debug(dataMask, values);
        }

        public static void ErrorWithMetadata(this ILogger logger, Dictionary<string, string> values)
        {
            AddMetadataInfo(values);
            logger.Error(dataMask, values);
        }

        private static void AddMetadataInfo(Dictionary<string, string> values)
        {
            values.Add("Env", "dev");
            values.Add(dataMask, System.Environment.MachineName);
        }
    }
}
