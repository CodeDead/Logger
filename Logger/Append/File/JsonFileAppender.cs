using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeDead.Logger.Configuration;
using CodeDead.Logger.Logging;
using Newtonsoft.Json;

namespace CodeDead.Logger.Append.File
{
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
            LogLevels = new List<LogLevel>();
            _settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path where the Log objects should be stored</param>
        public JsonFileAppender(string path)
        {
            FilePath = path;
            LogLevels = new List<LogLevel>();
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path where the Log objects should be stored</param>
        /// <param name="enabled">True if Log objects should be written to a file, otherwise false</param>
        public JsonFileAppender(string path, bool enabled)
        {
            FilePath = path;
            LogLevels = new List<LogLevel>();
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path where the Log objects should be stored</param>
        /// <param name="logLevels">The List of log levels that should be written to the file</param>
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
        /// <param name="path">The path where the Log objects should be stored</param>
        /// <param name="logLevels">The List of log levels that should be written to the file</param>
        /// <param name="enabled">True if Log objects should be written to a file, otherwise false</param>
        public JsonFileAppender(string path, List<LogLevel> logLevels, bool enabled)
        {
            FilePath = path;
            LogLevels = logLevels;
            _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            Enabled = enabled;
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object as a JSON string
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
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
                    sw.WriteLine(json);
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
                        await sw.WriteLineAsync(json);
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
            // Nothing needs to be done here
        }
    }
}
