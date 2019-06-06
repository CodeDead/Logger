using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc cref="FileAppender" />
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
        /// Gets or sets the delimiter character that can be used to split columns
        /// </summary>
        [XmlElement("Delimiter")]
        public char Delimiter { get; set; }

        /// <summary>
        /// Gets or sets whether the date should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendDate")]
        public bool AppendDate { get; set; }

        /// <summary>
        /// Gets or sets whether the content should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendContent")]
        public bool AppendContent { get; set; }

        /// <summary>
        /// Gets or sets whether the context should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendContext")]
        public bool AppendContext { get; set; }

        /// <summary>
        /// Gets or sets whether the LogLevel should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendLogLevel")]
        public bool AppendLogLevel { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        public CsvFileAppender()
        {
            LogLevels = DefaultLogLevels;
            Delimiter = DefaultDelimiter;
            TextEncoding = Encoding.Default;
            
            SetDefaultAppends();
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
            TextEncoding = Encoding.Default;
            Enabled = true;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="encoding">The encoding that should be ued to write the data</param>
        public CsvFileAppender(string path, Encoding encoding)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            Delimiter = DefaultDelimiter;
            TextEncoding = encoding;
            Enabled = true;

            SetDefaultAppends();
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
            TextEncoding = Encoding.Default;
            Enabled = enabled;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="enabled">True if Log objects should be exported, otherwise false</param>
        /// <param name="encoding">The encoding that should be used to write the data</param>
        public CsvFileAppender(string path, bool enabled, Encoding encoding)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            Delimiter = DefaultDelimiter;
            TextEncoding = encoding;
            Enabled = enabled;

            SetDefaultAppends();
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
            TextEncoding = Encoding.Default;
            Enabled = true;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="encoding">The encoding that should be used to write the data</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, Encoding encoding)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = DefaultDelimiter;
            TextEncoding = encoding;
            Enabled = true;

            SetDefaultAppends();
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
            TextEncoding = Encoding.Default;
            Enabled = true;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="delimiter">The delimiter that should be used to split the columns</param>
        /// <param name="encoding">The encoding that should be used to write the data</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, char delimiter, Encoding encoding)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = delimiter;
            TextEncoding = encoding;
            Enabled = true;

            SetDefaultAppends();
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
            TextEncoding = Encoding.Default;
            Enabled = enabled;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if Log objects should be exported, otherwise false</param>
        /// <param name="encoding">The encoding that should be used to write the data</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, bool enabled, Encoding encoding)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = DefaultDelimiter;
            TextEncoding = encoding;
            Enabled = enabled;

            SetDefaultAppends();
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
            TextEncoding = Encoding.Default;
            Enabled = enabled;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Initialize a new CsvFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="delimiter">The delimiter that should be used to split the columns</param>
        /// <param name="enabled">True if Log objects should be exported, otherwise false</param>
        /// <param name="encoding">The encoding that should be used to write the data</param>
        public CsvFileAppender(string path, List<LogLevel> logLevels, char delimiter, bool enabled, Encoding encoding)
        {
            FilePath = path;
            LogLevels = logLevels;
            Delimiter = delimiter;
            TextEncoding = encoding;
            Enabled = enabled;

            SetDefaultAppends();
            LoadStream();
        }

        /// <summary>
        /// Set the default append properties
        /// </summary>
        private void SetDefaultAppends()
        {
            AppendDate = true;
            AppendContent = true;
            AppendContext = true;
            AppendLogLevel = true;
        }

        /// <summary>
        /// Load the FileStream and StreamWriter for writing logs
        /// </summary>
        private void LoadStream()
        {
            try
            {
                _fs = System.IO.File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                _sw = new StreamWriter(_fs, TextEncoding);
            }
            catch (Exception)
            {
                if (ThrowErrors) throw;
            }
        }

        /// <summary>
        /// Validate whether a Log object can be exported
        /// </summary>
        /// <param name="log">The Log object that should be validated</param>
        /// <returns>True if the Log object can be exported, otherwise false</returns>
        private bool ValidLog(Log log)
        {
            if (!Enabled) return false;
            return log != null && LogLevels.Contains(log.LogLevel);
        }

        /// <summary>
        /// Format a Log object
        /// </summary>
        /// <param name="log">The Log object that needs to be formatted</param>
        /// <returns>The formatted string containing the contents of a Log object</returns>
        private string FormatLog(Log log)
        {
            string content = "";
            if (AppendDate) content += log.LogDate;

            if (AppendLogLevel)
            {
                if (content.Length > 0) content += Delimiter;
                content += log.LogLevel;
            }

            if (AppendContext)
            {
                if (content.Length > 0) content += Delimiter;
                content += "\"" + log.Context + "\"";
            }

            if (!AppendContent) return content;
            if (content.Length > 0) content += Delimiter;
            content += "\"" + log.Content + "\"";

            return content;
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
            try
            {
                if (_sw == null && !string.IsNullOrEmpty(FilePath))
                {
                    LoadStream();
                }

                if (_sw == null) return;
                _sw.WriteLine(FormatLog(log));
                _sw.Flush();
            }
            catch (Exception)
            {
                if (ThrowErrors) throw;
            }
            finally
            {
                _writing = false;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object in CSV format asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            if (!ValidLog(log)) return;
            await Task.Run(async () =>
            {
                _writing = true;
                try
                {
                    if (_sw != null && !string.IsNullOrEmpty(FilePath))
                    {
                        LoadStream();
                    }

                    if (_sw == null) return;
                    await _sw.WriteLineAsync(FormatLog(log));
                    await _sw.FlushAsync();
                }
                catch (Exception)
                {
                    if (ThrowErrors) throw;
                }
                finally
                {
                    _writing = false;
                }
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
