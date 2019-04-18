using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeDead.Logger;
using CodeDead.Logger.Append.Console;
using CodeDead.Logger.Append.File;
using CodeDead.Logger.Configuration;
using CodeDead.Logger.Logging;

namespace Sample
{
    /// <summary>
    /// Static class that can be used as a sample for the Logger project
    /// </summary>
    internal static class Program
    {
        private static async Task Main()
        {
            // Logger objects can be generated using the LogFactory using either a key:
            // Logger logger = LogFactory.GenerateLogger("MAIN");
            // Or a default Logger object can be retrieved
            Logger logger = LogFactory.GenerateLogger();
            string defaultName = logger.Name;

            // This is the default console appender but you can implement the LogAppender or ConsoleAppender to write your own logic
            DefaultConsoleAppender consoleWriter = new DefaultConsoleAppender();
            // This is the default file appender but you can implement the LogAppender or FileAppender to write your own logic
            DefaultFileAppender fileAppender = new DefaultFileAppender(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\exampleLog.log");
            // You can have as many appenders as your system allows
            logger.LogManager.AddLogAppender(consoleWriter);
            logger.LogManager.AddLogAppender(fileAppender);

            // Defaults
            logger.Trace("Hello trace!");
            logger.Debug("Hello debug!");
            logger.Info("Hello info!");
            logger.Warn("Hello warn!");
            logger.Error("Hello error!");

            // Warnings will no longer be written to the console but they'll still be saved in the LogRepository
            consoleWriter.LogLevels.Remove(LogLevel.Warning);
            logger.Warn("This will disable logs from being exported to the console. They'll still be saved in the log repository though!");
            logger.Info("This will still output to the console!");

            // Info will no longer be written to the console and will not be saved in the LogRepository
            logger.InfoEnabled = false;
            logger.Info("This will no longer save this type of log or output it to the console");
            logger.Debug("This, on the other hand, is still active");

            // Write some logs asynchronously
            for (int i = 0; i < 50; i++)
            {
                await logger.DebugAsync("This is asynchronous write #" + i);
            }

            // Example of Exception logging
            try
            {
                throw new Exception("I'm an error!");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            // Example logger that uses event handling
            Logger logger2 = LogFactory.GenerateLogger();

            logger2.LogManager.LogAddedEvent += OnLogAddedEvent;
            logger2.LogManager.LogRemovedEvent += OnLogRemovedEvent;

            Log toDelete = new Log("This is going to be deleted", LogLevel.Debug);
            logger2.LogManager.AddLog(toDelete);
            logger2.LogManager.RemoveLog(toDelete);

            // Example of saving configuration to an XML file
            LogFactory.SaveConfiguration(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\settings.xml", SaveFormats.Xml);
            // Example of saving configuration to a JSON file
            LogFactory.SaveConfiguration(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\settings.json", SaveFormats.Json);

            // Removing the loggers
            LogFactory.RemoveLogger(logger);
            LogFactory.RemoveLogger(logger2);

            // Reloading the previously saved configuration
            // Events etc. will have to be reapplied though after loading from a configuration file
            LogFactory.LoadFromConfiguration(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\settings.json");
            List<Logger> loadedLogger = LogFactory.GetLoggersByName(defaultName).ToList();

            loadedLogger[0].Info("This still won't work");
            loadedLogger[0].InfoEnabled = true;
            loadedLogger[0].Info("But this will!");

            Console.ReadLine();
        }

        /// <summary>
        /// Method that is called when a Log was removed
        /// </summary>
        /// <param name="log">The Log that was removed</param>
        private static void OnLogRemovedEvent(Log log)
        {
            // If you're working in a UI environment, you might want to use Dispatcher.Invoke
            Console.WriteLine("The log with content " + log.Content + " was removed!");
        }

        /// <summary>
        /// Method that is called when a Log has been added
        /// </summary>
        /// <param name="log">The Log that was added</param>
        private static void OnLogAddedEvent(Log log)
        {
            // If you're working in a UI environment, you might want to use Dispatcher.Invoke
            Console.WriteLine(log.Content);
        }
    }
}
