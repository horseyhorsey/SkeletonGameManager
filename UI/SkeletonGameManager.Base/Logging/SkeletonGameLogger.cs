using Microsoft.Extensions.Logging;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameManager.Base
{
    public class SkeletonGameLogger : ILoggerFacade
    {
        //Microsoft.Extensions.Logging.ILogger
        private ILogger _logger;

        public SkeletonGameLogger(string name = null)
        {
            if (_logger == null)
                CreateLogger(name);
        }

        //Log with the Extensions logger    
        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger?.LogDebug($"{message}", category, priority);
                    break;
                case Category.Exception:
                    _logger?.LogError($"{message}", category, priority);
                    break;
                case Category.Info:
                    _logger?.LogInformation($"{message}", category, priority);
                    break;
                case Category.Warn:
                    _logger?.LogWarning($"{message}", category, priority);
                    break;
                default:
                    break;
            }
        }

        private void CreateLogger(string name = null)
        {
            var fileName = string.Empty;
            if (string.IsNullOrWhiteSpace(name))
            {
                fileName = "Log.log";
            }
            else
            {
                fileName = $"{name}.log";
            }
#if DEBUG
            ILoggerFactory logFactory = new LoggerFactory()
            .AddConsole(LogLevel.Trace)
            .AddFile("Logs\\" + fileName, LogLevel.Debug);
#else
        ILoggerFactory logFactory = new LoggerFactory()
        .AddFile(fileName, LogLevel.Error);
#endif
            //Create the logger from incoming name.
            if (!string.IsNullOrWhiteSpace(name))
                _logger = logFactory.CreateLogger(name);
            else
                _logger = logFactory.CreateLogger("Default Logger");
        }
    }
}
