using System.Collections.Generic;
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
        private static readonly Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();
        #endregion

        /// <summary>
        /// Generate a new Logger object with a default key
        /// </summary>
        /// <returns>A newly created Logger object. Can throw an exception if the generated key for the Logger object already exists</returns>
        public static Logger GenerateLogger()
        {
            Logger logger = new Logger(new LogManager());
            Loggers.Add(System.Guid.NewGuid().ToString(), logger);

            return logger;
        }

        /// <summary>
        /// Add a Logger object to the Dictionary of Logger objects while generating a random GUID as the key
        /// </summary>
        /// <param name="logger">The Logger object that should be added to the Dictionary</param>
        public static void AddLogger(Logger logger)
        {
            Loggers.Add(System.Guid.NewGuid().ToString(), logger);
        }

        /// <summary>
        /// Generate a new Logger object with a specified key
        /// </summary>
        /// <param name="key">The key that can be used to identify the Logger</param>
        /// <returns>The newly created Logger object</returns>
        public static Logger GenerateLogger(string key)
        {
            Logger logger = new Logger(new LogManager());
            Loggers.Add(key, logger);

            return logger;
        }

        /// <summary>
        /// Get all available Logger objects
        /// </summary>
        /// <returns>The List of Logger objects</returns>
        public static List<Logger> GetLoggers()
        {
            List<Logger> loggers = new List<Logger>();
            foreach (KeyValuePair<string, Logger> entry in Loggers)
            {
                loggers.Add(entry.Value);
            }
            return loggers;
        }

        /// <summary>
        /// Remove a Logger from the Dictionary of logger objects
        /// </summary>
        /// <param name="logger">The Logger object that should be removed</param>
        public static void RemoveLogger(Logger logger)
        {
            string foundKey = null;

            foreach (KeyValuePair<string, Logger> entry in Loggers)
            {
                if (entry.Value == logger) foundKey = entry.Key;
                break;
            }

            if (foundKey != null) Loggers.Remove(foundKey);
        }

        /// <summary>
        /// Remove a List of Logger objects from the Dictionary of Logger objects
        /// </summary>
        /// <param name="loggers">The List of Logger objects that should be removed from the Dictionary of Logger objects</param>
        public static void RemoveLoggers(List<Logger> loggers)
        {
            List<string> foundKeys = new List<string>();

            foreach (KeyValuePair<string, Logger> entry in Loggers)
            {
                foreach (Logger logger in loggers)
                {
                    if (entry.Value == logger) foundKeys.Add(entry.Key);
                }
            }

            foreach (string key in foundKeys)
            {
                Loggers.Remove(key);
            }
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
            LoggerRoot root = await ConfigurationManager.LoadLoggerRootAsync(filePath);
            foreach (Logger logger in root.Loggers)
            {
                AddLogger(logger);
            }
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
            LoggerRoot root = new LoggerRoot();
            foreach (KeyValuePair<string, Logger> entry in Loggers)
            {
                root.Loggers.Add(entry.Value);
            }

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
                LoggerRoot root = new LoggerRoot();
                foreach (KeyValuePair<string, Logger> entry in Loggers)
                {
                    root.Loggers.Add(entry.Value);
                }

                await ConfigurationManager.SaveLoggerRootAsync(filePath, root, saveFormat);
            });
        }
    }
}
