namespace SkeletonGameManager.Module.LogViewer
{
    public class Log
    {
        public string Line { get; }

        public LogLevel Level { get; }

        public Log(string line)
        {
            Line = line;
            Level = GetLogLevel(line);
        }

        private LogLevel GetLogLevel(string line)
        {
            if (line.Contains("DBG") || line.Contains("DEBUG"))
                return LogLevel.Debug;

            if (line.Contains("ERR") || line.Contains("EXCEPTION"))
                    return LogLevel.Exception;

            if (line.Contains("WRN") || line.Contains("WARNING"))
                return LogLevel.Warn;

            return LogLevel.Info;
        }
    }
}
