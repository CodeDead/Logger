using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeDead.Logger.Configuration;

namespace CodeDead.Logger
{
    /// <summary>
    /// Static class that contains the logic for generating a Logger object
    /// </summary>
    public static class LogFactory
    {
        #region Variables
        private static readonly List<Logger> Loggers = new List<Logger>();
        #endregion

        /// <summary>
        /// Generate a new Logger object with a default key
        /// </summary>
        /// <returns>A newly created Logger object. Can throw an exception if the generated key for the Logger object already exists</returns>
        public static Logger GenerateLogger()
        {
            Logger logger = new Logger(System.Guid.NewGuid().ToString(), new LogManager());
            Loggers.Add(logger);

            return logger;
        }

        /// <summary>
        /// Add a Logger object to the Dictionary of Logger objects while generating a random GUID as the key
        /// </summary>
        /// <param name="logger">The Logger object that should be added to the Dictionary</param>
        public static void AddLogger(Logger logger)
        {
            Loggers.Add(logger);
        }

        /// <summary>
        /// Generate a new Logger object with a specified key
        /// </summary>
        /// <param name="name">The key that can be used to identify the Logger</param>
        /// <returns>The newly created Logger object</returns>
        public static Logger GenerateLogger(string name)
        {
            Logger logger = new Logger(name, new LogManager());
            Loggers.Add(logger);

            return logger;
        }

        /// <summary>
        /// Get all available Logger objects
        /// </summary>
        /// <returns>The List of Logger objects</returns>
        public static IEnumerable<Logger> GetLoggers()
        {
            return Loggers;
        }

        /// <summary>
        /// Find a List of Logger objects that match the given name
        /// </summary>
        /// <param name="name">The name that should be searched for</param>
        /// <returns>The List of Logger objects that have the given name</returns>
        public static IEnumerable<Logger> GetLoggersByName(string name)
        {
            return Loggers.Where(l => l.Name == name).ToList();
        }

        /// <summary>
        /// Remove a Logger from the Dictionary of logger objects
        /// </summary>
        /// <param name="logger">The Logger object that should be removed</param>
        public static void RemoveLogger(Logger logger)
        {
            Loggers.Remove(logger);
        }

        /// <summary>
        /// Remove a List of Logger objects from the Dictionary of Logger objects
        /// </summary>
        /// <param name="loggers">The List of Logger objects that should be removed from the Dictionary of Logger objects</param>
        public static void RemoveLoggers(List<Logger> loggers)
        {
            Loggers.RemoveAll(loggers.Contains);
        }

        /// <summary>
        /// Load Logger objects from a configuration file
        /// </summary>
        /// <param name="filePath">The path of the configuration file</param>
        public static void LoadFromConfiguration(string filePath)
        {
            LoggerRoot root = ConfigurationManager.LoadLoggerRoot(filePath);
            foreach (Logger logger in root.Loggers)
            {
                AddLogger(logger);
            }
        }

        /// <summary>
        /// Load Logger objects from a configuration file asynchronously
        /// </summary>
        /// <param name="filePath">The path of the configuration file</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public static async Task LoadFromConfigurationAsync(string filePath)
        {
            await Task.Run(async () =>
            {
                LoggerRoot root = await ConfigurationManager.LoadLoggerRootAsync(filePath);
                foreach (Logger logger in root.Loggers)
                {
                    AddLogger(logger);
                }
            });
        }

        /// <summary>
        /// Load Logger objects from a byte array that contains the configuration data
        /// </summary>
        /// <param name="configuration">The byte array that contains the configuration data</param>
        public static void LoadFromConfiguration(byte[] configuration)
        {
            LoggerRoot root = ConfigurationManager.LoadLoggerRoot(configuration);
            foreach (Logger logger in root.Loggers)
            {
                AddLogger(logger);
            }
        }

        /// <summary>
        /// Load Logger objects from a byte array that contains the configuration data asynchronously
        /// </summary>
        /// <param name="configuration">The byte array that contains the configuration data</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public static async Task LoadFromConfigurationAsync(byte[] configuration)
        {
            await Task.Run(async () =>
            {
                LoggerRoot root = await ConfigurationManager.LoadLoggerRootAsync(configuration);
                foreach (Logger logger in root.Loggers)
                {
                    AddLogger(logger);
                }
            });
        }

        /// <summary>
        /// Save Logger objects to a configuration file
        /// </summary>
        /// <param name="filePath">The path where the configuration file should be stored</param>
        /// <param name="saveFormat">The format in which the configuration data should be stored</param>
        public static void SaveConfiguration(string filePath, SaveFormats saveFormat)
        {
            LoggerRoot root = new LoggerRoot {Loggers = Loggers};
            ConfigurationManager.SaveLoggerRoot(filePath, root, saveFormat);
        }

        /// <summary>
        /// Save Logger objects to a configuration file asynchronously
        /// </summary>
        /// <param name="filePath">The path where the configuration file should be stored</param>
        /// <param name="saveFormat">The format in which the configuration data should be stored</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public static async Task SaveConfigurationAsync(string filePath, SaveFormats saveFormat)
        {
            await Task.Run(async () =>
            {
                LoggerRoot root = new LoggerRoot { Loggers = Loggers };
                await ConfigurationManager.SaveLoggerRootAsync(filePath, root, saveFormat);
            });
        }
    }
}
