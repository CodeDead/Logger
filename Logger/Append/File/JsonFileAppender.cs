using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using CodeDead.Logger.Configuration;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Sealed class that contains logic to store Log objects as JSON
    /// </summary>
    public sealed class JsonFileAppender : FileAppender
    {
        #region Variables
        private readonly JavaScriptSerializer _serializer;
        #endregion

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        public JsonFileAppender()
        {
            LogLevels = DefaultLogLevels;
            _serializer = new JavaScriptSerializer(new SimpleTypeResolver());
        }

        /// <summary>
        /// Initialize a new JsonFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        public JsonFileAppender(string path)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            _serializer = new JavaScriptSerializer(new SimpleTypeResolver());
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
            _serializer = new JavaScriptSerializer(new SimpleTypeResolver());
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
            _serializer = new JavaScriptSerializer(new SimpleTypeResolver());
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
            _serializer = new JavaScriptSerializer(new SimpleTypeResolver());
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
            return log != null && LogLevels.Contains(log.LogLevel);
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

            LogRoot root;
            try
            {
                root = string.IsNullOrEmpty(readContents) ? new LogRoot() : _serializer.Deserialize<LogRoot>(readContents);
            }
            catch (ArgumentException)
            {
                if (ThrowErrors) throw;
                return;
            }

            // Add a Log to the LogRoot object
            root.Logs.Add(log);

            try
            {
                using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    // Convert it back into a JSON
                    string json = _serializer.Serialize(root);

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(json);
                        sw.Flush();
                    }
                }
            }
            catch (Exception)
            {
                if (ThrowErrors) throw;
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
            if (!ValidLog(log)) return;
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

                LogRoot root;
                try
                {
                    root = string.IsNullOrEmpty(readContents) ? new LogRoot() : _serializer.Deserialize<LogRoot>(readContents);
                }
                catch (ArgumentException)
                {
                    if (ThrowErrors) throw;
                    return;
                }

                // Add a Log to the LogRoot object
                root.Logs.Add(log);

                try
                {
                    using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                    {
                        // Convert it back into a JSON
                        string json = _serializer.Serialize(root);

                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            await sw.WriteAsync(json);
                            await sw.FlushAsync();
                        }
                    }
                }
                catch (Exception)
                {
                    if (ThrowErrors) throw;
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
