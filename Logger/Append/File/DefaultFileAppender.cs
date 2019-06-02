using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc cref="FileAppender" />
    /// <summary>
    /// Sealed class containing the default file appender logic
    /// </summary>
    public sealed class DefaultFileAppender : FileAppender
    {
        #region Variables
        private const string DefaultFormat = "[%d]\t[%l](%C)\t-\t%c";
        private FileStream _fileStream;
        private StreamWriter _streamWriter;
        private bool _writing;
        #endregion

        #region Properties
        /// <summary>
        /// Property that sets the format in which Log objects should be displayed in the console
        /// </summary>
        [XmlElement("Format")]
        public string Format { get; set; }

        /// <summary>
        /// Property that sets whether the date should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendDate")]
        public bool AppendDate { get; set; }

        /// <summary>
        /// Property that sets whether the content should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendContent")]
        public bool AppendContent { get; set; }

        /// <summary>
        /// Property that sets whether the context should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendContext")]
        public bool AppendContext { get; set; }

        /// <summary>
        /// Property that sets whether the LogLevel should be appended when exporting a Log object
        /// </summary>
        [XmlElement("AppendLogLevel")]
        public bool AppendLogLevel { get; set; }
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        public DefaultFileAppender()
        {
            LogLevels = DefaultLogLevels;
            Format = DefaultFormat;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        public DefaultFileAppender(string path)
        {
            LogLevels = DefaultLogLevels;
            Format = DefaultFormat;
            FilePath = path;

            SetDefaultAppends();
            LoadStream(path);

            Enabled = true;
        }

        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public DefaultFileAppender(string path, bool enabled)
        {
            LogLevels = DefaultLogLevels;
            Format = DefaultFormat;
            FilePath = path;

            SetDefaultAppends();
            LoadStream(path);

            Enabled = enabled;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public DefaultFileAppender(List<LogLevel> logLevels)
        {
            LogLevels = logLevels;
            Format = DefaultFormat;

            SetDefaultAppends();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public DefaultFileAppender(string path, List<LogLevel> logLevels, bool enabled)
        {
            LogLevels = logLevels;
            Format = DefaultFormat;
            FilePath = path;

            SetDefaultAppends();
            LoadStream(path);

            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="format">The format in which the Log object should be outputted to the file</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public DefaultFileAppender(string path, List<LogLevel> logLevels, string format, bool enabled)
        {
            LogLevels = logLevels;
            Format = format;
            FilePath = path;

            SetDefaultAppends();
            LoadStream(path);

            Enabled = enabled;
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
        /// Load the FileStream and StreamWriter
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        private void LoadStream(string path)
        {
            try
            {
                _fileStream = System.IO.File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read);
                _streamWriter = new StreamWriter(_fileStream);
            }
            catch (Exception)
            {
                if (ThrowErrors) throw;
            }
        }

        /// <summary>
        /// Check whether a Log object can be exported or not
        /// </summary>
        /// <param name="log">The Log object that should be validated</param>
        /// <returns>True if the Log object can be exported, otherwise false</returns>
        private bool ValidExport(Log log)
        {
            if (!Enabled) return false;
            return log != null && LogLevels.Contains(log.LogLevel);
        }

        /// <summary>
        /// Format a Log object
        /// </summary>
        /// <param name="log">The Log object that should be formatted</param>
        /// <returns>The string representation of a Log object using the given format</returns>
        private string FormatLog(Log log)
        {
            string tempFormat = Format;

            if (!AppendDate) tempFormat = tempFormat.Replace("%d", "");
            if (!AppendLogLevel) tempFormat = tempFormat.Replace("%l", "");
            if (!AppendContent) tempFormat = tempFormat.Replace("%c", "");
            if (!AppendContext) tempFormat = tempFormat.Replace("%C", "");

            string output = tempFormat
                .Replace("%d", log.LogDate.ToString(CultureInfo.InvariantCulture))
                .Replace("%l", Enum.GetName(typeof(LogLevel), log.LogLevel))
                .Replace("%c", log.Content)
                .Replace("%C", log.Context);

            return output;
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to a file
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
            if (!ValidExport(log)) return;
            _writing = true;
            try
            {
                if (_streamWriter == null) return;
                _streamWriter.WriteLine(FormatLog(log));
                _streamWriter.Flush();
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
        /// Export a Log object to a file asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            if (!ValidExport(log)) return;
            await Task.Run(async () =>
            {
                _writing = true;
                try
                {
                    if (_streamWriter != null)
                    {
                        await _streamWriter.WriteLineAsync(FormatLog(log));
                        await _streamWriter.FlushAsync();
                    }
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
        /// Dispose of all resources used by this DefaultFileAppender instance
        /// </summary>
        public override void Dispose()
        {
            while (_writing)
            {
                // Wait until the write is complete
            }
            _streamWriter?.Dispose();
            _fileStream?.Dispose();
        }
    }
}
