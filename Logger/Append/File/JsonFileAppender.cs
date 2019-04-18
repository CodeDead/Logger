using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeDead.Logger.Configuration;
using CodeDead.Logger.Logging;
using Newtonsoft.Json;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Sealed class that contains logic to store Log objects as JSON
    /// </summary>
    public sealed class JsonFileAppender : FileAppender
    {
        #region Variables
        private readonly JsonSerializerSettings _settings;
        #endregion

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        public JsonFileAppender()
        {
            LogLevels = DefaultLogLevels;
            _settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        public JsonFileAppender(string path)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public JsonFileAppender(string path, bool enabled)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public JsonFileAppender(string path, List<LogLevel> logLevels)
        {
            FilePath = path;
            LogLevels = logLevels;
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public JsonFileAppender(string path, List<LogLevel> logLevels, bool enabled)
        {
            FilePath = path;
            LogLevels = logLevels;
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = enabled;
        }

        /// <summary>
        /// Validate whether a Log can be exported or not
        /// </summary>
        /// <param name="log">The Log object that needs to be validated</param>
        /// <returns>True if a Log can be exported, otherwise false</returns>
        private bool ValidLog(Log log)
        {
            if (!Enabled) return false;
            if (log == null) throw new ArgumentNullException(nameof(log));
            return LogLevels.Contains(log.LogLevel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object as a JSON string
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
            if (!ValidLog(log)) return;
            string readContents = null;
            try
            {
                using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Read the contents of the file
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        readContents = sr.ReadToEnd();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // Ignored
            }

            LogRoot root = string.IsNullOrEmpty(readContents) ? new LogRoot() : JsonConvert.DeserializeObject<LogRoot>(readContents, _settings);

            // Add a Log to the LogRoot object
            root.Logs.Add(log);

            using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                // Convert it back into a JSON
                string json = JsonConvert.SerializeObject(root, _settings);

                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(json);
                    sw.Flush();
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object as a JSON string asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            await Task.Run(async () =>
            {
                if (!ValidLog(log)) return;
                string readContents = null;
                try
                {
                    using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read the contents of the file
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            readContents = await sr.ReadToEndAsync();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    // Ignored
                }

                LogRoot root = string.IsNullOrEmpty(readContents) ? new LogRoot() : JsonConvert.DeserializeObject<LogRoot>(readContents, _settings);

                // Add a Log to the LogRoot object
                root.Logs.Add(log);


                using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    // Convert it back into a JSON
                    string json = JsonConvert.SerializeObject(root, _settings);

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        await sw.WriteAsync(json);
                        await sw.FlushAsync();
                    }
                }
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose of any resources that are in use
        /// </summary>
        public override void Dispose()
        {
            // Nothing needs to be disposed
        }
    }
}
