using System.Collections.Generic;

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
        /// Get the Logger object using a specified key
        /// </summary>
        /// <param name="key">The key that can be used to retrieve a Logger</param>
        /// <returns>The Logger object that is associated with the specified key</returns>
        public static Logger GetLogger(string key)
        {
            return Loggers[key];
        }

        /// <summary>
        /// Remove a Logger from the Dictionary of Logger objects
        /// </summary>
        /// <param name="key">The key of the Logger object that should be removed</param>
        public static void RemoveLogger(string key)
        {
            Loggers.Remove(key);
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
        /// Clear the key-value pair of loggers
        /// </summary>
        public static void ClearLoggers()
        {
            Loggers.Clear();
        }
    }
}
