using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Sealed class that contains the logic for exporting Log objects to a CSV file
    /// </summary>
    public sealed class CsvFileAppender : FileAppender
    {
        #region Variables
        private FileStream _fs;
        private StreamWriter _sw;
        private bool _writing;
        private const char DefaultDelimiter = ',';
        #endregion

        #region Properties
        /// <summary>
        /// Property that contains the delimiter character that can be used to split columns
        /// </summary>
        [XmlElement("Delimiter")]
        public char Delimiter { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        public CsvFileAppender()
        {
            LogLevels = DefaultLogLevels;
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        public CsvFileAppender(string path)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            Delimiter = DefaultDelimiter;
            Enabled = true;

            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="enabled">True if Log objects should be exported, otherwise false</param>
        public CsvFileAppender(string path, bool enabled)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            Delimiter = DefaultDelimiter;
            Enabled = enabled;

            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = DefaultDelimiter;
            Enabled = true;

            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="delimiter">The delimiter that should be used to split the columns</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, char delimiter)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = delimiter;
            Enabled = true;

            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if Log objects should be exported, otherwise false</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, bool enabled)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = DefaultDelimiter;
            Enabled = enabled;

            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="delimiter">The delimiter that should be used to split the columns</param>
        /// <param name="enabled">True if Log objects should be exported, otherwise false</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, char delimiter, bool enabled)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = delimiter;
            Enabled = enabled;

            LoadStream();
        }

        /// <summary>
        /// Load the FileStream and StreamWriter for writing logs
        /// </summary>
        private void LoadStream()
        {
            while (_writing)
            {
                // Wait
            }
            _fs = System.IO.File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            _sw = new StreamWriter(_fs);
        }

        /// <summary>
        /// Validate whether a Log object can be exported
        /// </summary>
        /// <param name="log">The Log object that should be validated</param>
        /// <returns>True if the Log object can be exported, otherwise false</returns>
        private bool ValidLog(Log log)
        {
            if (!Enabled) return false;
            if (log == null) throw new ArgumentNullException(nameof(log));
            return LogLevels.Contains(log.LogLevel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object in CSV format
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
            if (!ValidLog(log)) return;

            _writing = true;
            _sw.WriteLine(log.LogDate + Delimiter.ToString() + log.LogLevel + Delimiter + "\"" + log.Context + "\"" + Delimiter + "\"" + log.Content + "\"");
            _sw.Flush();
            _writing = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object in CSV format asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            await Task.Run(async () =>
            {
                if (!ValidLog(log)) return;

                _writing = true;
                await _sw.WriteLineAsync(log.LogDate + Delimiter.ToString() + log.LogLevel + Delimiter + "\"" + log.Context + "\"" + Delimiter + "\"" + log.Content + "\"");
                await _sw.FlushAsync();
                _writing = false;
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose all resources
        /// </summary>
        public override void Dispose()
        {
            while (_writing)
            {
                // Wait
            }

            _sw?.Dispose();
            _fs?.Dispose();
        }
    }
}
