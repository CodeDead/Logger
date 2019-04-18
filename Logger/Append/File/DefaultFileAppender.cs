﻿using System;
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
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        public DefaultFileAppender()
        {
            LogLevels = DefaultLogLevels;
            Format = DefaultFormat;
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
            LoadStream(path);
            Enabled = enabled;
        }

        /// <summary>
        /// Load the FileStream and StreamWriter
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        private void LoadStream(string path)
        {
            _fileStream = System.IO.File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            _streamWriter = new StreamWriter(_fileStream);
            _writing = false;
        }

        /// <summary>
        /// Check whether a Log object can be exported or not
        /// </summary>
        /// <param name="log">The Log object that should be validated</param>
        /// <returns>True if the Log object can be exported, otherwise false</returns>
        private bool ValidExport(Log log)
        {
            if (!Enabled) return false;
            if (log == null) throw new ArgumentNullException(nameof(log));
            return LogLevels.Contains(log.LogLevel);
        }

        /// <summary>
        /// Format a Log object into a printable string
        /// </summary>
        /// <param name="log">The Log object that should be converted into a printable string</param>
        /// <returns>The string representation of a Log object using the given format</returns>
        private string FormatLog(Log log)
        {
            string output = Format
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
            _streamWriter?.WriteLine(FormatLog(log));
            _streamWriter?.Flush();
            _writing = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to a file asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            await Task.Run(async () =>
            {
                if (!ValidExport(log)) return;
                if (_streamWriter != null)
                {
                    _writing = true;
                    await _streamWriter.WriteLineAsync(FormatLog(log));
                    await _streamWriter.FlushAsync();
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
