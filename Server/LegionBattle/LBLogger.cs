using System;
using System.Collections.Generic;
using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;

namespace LegionBattle
{
    class LBLogger
    {
        private readonly static ILogger logImp = LogManager.GetCurrentClassLogger();
        public static void InitLogger(string applicationRootPath, string binaryPath)
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(applicationRootPath, "log");
            var configFileInfo = new FileInfo(Path.Combine(binaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }
        }

        [System.Diagnostics.Conditional("DebugLog")]
        public static void Info(string tag, string log)
        {
            logImp.Info(string.Format("[{0}] {1}", tag, log));
        }

        public static void Error(string tag, string log)
        {
            logImp.Error(string.Format("[{0}] {1}", tag, log));
        }
    }
}
