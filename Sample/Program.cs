using System.Collections.Generic;
using CodeDead.Logger;
using CodeDead.Logger.Append.Console;
using CodeDead.Logger.Logging;

namespace Sample
{
    /// <summary>
    /// Static class that can be used as a sample for the Logger project
    /// </summary>
    internal static class Program
    {
        static void Main()
        {
            // Logger objects can be generated using the LogFactory using either a key:
            // Logger logger = LogFactory.GenerateLogger("MAIN");
            // Or a default Logger object can be retrieved
            Logger logger = LogFactory.GenerateLogger();

            // The list of log levels that would have to be appended
            List<LogLevel> logLevels = new List<LogLevel>
            {
                LogLevel.Trace,
                LogLevel.Debug,
                LogLevel.Info,
                LogLevel.Warning,
                LogLevel.Error
            };

            // This is the default console appender, but you can implement the LogAppender or ConsoleAppender to write your own logic
            DefaultConsoleAppender consoleWriter = new DefaultConsoleAppender(logLevels, true);
            // You can have as many appenders as your system allows
            logger.GetLogManager().AddLogAppender(consoleWriter);

            // Defaults
            logger.Trace("Hello trace!");
            logger.Debug("Hello debug!");
            logger.Info("Hello info!");
            logger.Warn("Hello warn!");
            logger.Error("Hello error!");

            // Warnings will no longer be written to the console but they'll still be saved in the LogRepository
            consoleWriter.RemoveLogLevel(LogLevel.Warning);
            logger.Warn("This will disable logs from being exported to the console. They'll still be saved in the log repository though!");
            logger.Info("This will still output to the console!");

            // Info will no longer be written to the console and will not be saved in the LogRepository
            logger.InfoEnabled = false;
            logger.Info("This will no longer save this type of log or output it to the console");
            logger.Debug("This, on the other hand, is still active");

            // Writing logs will now happen asynchronously
            consoleWriter.Asynchronous = true;
            for (int i = 0; i < 50; i++)
            {
                logger.Debug("This is asynchronous write #" + i);
            }
            // Disable async logging
            consoleWriter.Asynchronous = false;
            logger.Debug("Hello!");

            // Example logger that uses event handling
            Logger logger2 = LogFactory.GenerateLogger();

            logger2.GetLogManager().LogAddedEvent += OnLogAddedEvent;
            logger2.GetLogManager().LogRemovedEvent += OnLogRemovedEvent;

            Log toDelete = new Log("This is going to be deleted", LogLevel.Debug);
            logger2.GetLogManager().AddLog(toDelete);
            logger2.GetLogManager().RemoveLog(toDelete);

            System.Console.ReadLine();
        }

        /// <summary>
        /// Method that is called when a Log was removed
        /// </summary>
        /// <param name="log">The Log that was removed</param>
        private static void OnLogRemovedEvent(Log log)
        {
            // If you're working in a UI environment, you might want to use Dispatcher.Invoke
            System.Console.WriteLine("The log with content " + log.Content + " was removed!");
        }

        /// <summary>
        /// Method that is called when a Log has been added
        /// </summary>
        /// <param name="log">The Log that was added</param>
        private static void OnLogAddedEvent(Log log)
        {
            // If you're working in a UI environment, you might want to use Dispatcher.Invoke
            System.Console.WriteLine(log.Content);
        }
    }
}
