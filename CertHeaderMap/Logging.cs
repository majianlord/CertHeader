using NLog;
using NLog.Config;
using NLog.Targets;

namespace CertHeader.Logging
{
    internal static class Log
    {
        public static Logger Instance { get; private set; }
        static Log()
        {
            LogManager.ReconfigExistingLoggers();
            Instance = LogManager.GetCurrentClassLogger();
        }

        public static void LoggerSetup(NLog.LogLevel LogLevel, bool timings)
        {
            // If LogManager.Configuration Is Nothing Then
            LoggingConfiguration LoggingConfig = new LoggingConfiguration();
            // Setup Default File Log
            FileTarget FileTarget = new FileTarget();
            FileTarget.FileName = "${basedir}/log/logfile.log";
            FileTarget.Layout = "${longdate} : ${uppercase:${level}} : ${message} ${exception:format=tostring}";
            FileTarget.Name = "FileTarget";
            FileTarget.ArchiveFileName = "${basedir}/log/archives/log.{#}.log";
            FileTarget.ArchiveEvery = FileArchivePeriod.Hour;
            FileTarget.ArchiveNumbering = ArchiveNumberingMode.Date;
            FileTarget.MaxArchiveFiles = 168;
            LoggingConfig.AddTarget(FileTarget);
            LoggingRule Rule1 = new LoggingRule("*", LogLevel, FileTarget);
            LoggingConfig.LoggingRules.Add(Rule1);
            LogManager.Configuration = LoggingConfig;
        }

    }
}




