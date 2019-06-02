using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.Console
{
    /// <inheritdoc cref="ConsoleAppender" />
    /// <summary>
    /// Sealed class that can be used to write Log objects to the console using a default implementation
    /// </summary>
    public sealed class DefaultConsoleAppender : ConsoleAppender
    {
        #region Variables
        private const string DefaultFormat = "[%d]\t[%l](%C)\t-\t%c";
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

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        public DefaultConsoleAppender()
        {
            Format = DefaultFormat;
            LogLevels = DefaultLogLevels;
            Enabled = true;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="enabled">True if exporting Log objects to the console should be enabled, otherwise false</param>
        public DefaultConsoleAppender(bool enabled)
        {
            LogLevels = DefaultLogLevels;
            Format = DefaultFormat;
            Enabled = enabled;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public DefaultConsoleAppender(List<LogLevel> logLevels)
        {
            Format = DefaultFormat;
            LogLevels = logLevels;
            Enabled = true;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to the console should be enabled, otherwise false</param>
        public DefaultConsoleAppender(List<LogLevel> logLevels, bool enabled)
        {
            LogLevels = logLevels;
            Format = DefaultFormat;
            Enabled = enabled;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="format">The format in which the Log object should be outputted to the console</param>
        public DefaultConsoleAppender(string format)
        {
            LogLevels = DefaultLogLevels;
            Format = format;
            Enabled = true;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="format">The format in which the Log object should be outputted to the console</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public DefaultConsoleAppender(string format, List<LogLevel> logLevels)
        {
            Format = format;
            LogLevels = logLevels;
            Enabled = true;
            SetDefaultAppends();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="format">The format in which the Log object should be outputted to the console</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to the console should be enabled, otherwise false</param>
        public DefaultConsoleAppender(string format, List<LogLevel> logLevels, bool enabled)
        {
            Format = format;
            LogLevels = logLevels;
            Enabled = enabled;
            SetDefaultAppends();
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

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to the console
        /// </summary>
        /// <param name="log">The Log object that should be exported to the console</param>
        public override void ExportLog(Log log)
        {
            if (!ValidExport(log)) return;

            string content = FormatLog(log);
            switch (log.LogLevel)
            {
                case LogLevel.Trace:
                    System.Diagnostics.Trace.WriteLine(content);
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine(content);
                    break;
                case LogLevel.Info:
                case LogLevel.Warning:
                case LogLevel.Error:
                    System.Console.WriteLine(content);
                    break;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to the console asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported to the console</param>
        /// <returns>The Task that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            if (!ValidExport(log)) return;

            await Task.Run(async () =>
            {
                string content = FormatLog(log);
                switch (log.LogLevel)
                {
                    case LogLevel.Trace:
                        System.Diagnostics.Trace.WriteLine(content);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine(content);
                        break;
                    case LogLevel.Warning:
                    case LogLevel.Info:
                    case LogLevel.Error:
                        await System.Console.Out.WriteLineAsync(content);
                        break;
                }
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose of the DefaultConsoleAppender instance
        /// </summary>
        public override void Dispose()
        {
            // Nothing needs to be disposed
        }
    }
}
