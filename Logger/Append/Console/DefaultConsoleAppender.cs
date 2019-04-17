using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.Console
{
    /// <inheritdoc />
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
        #endregion

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        public DefaultConsoleAppender()
        {
            Format = DefaultFormat;
            LogLevels = new List<LogLevel> {LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error};
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="enabled">True if exporting Log objects to the console should be enabled, otherwise false</param>
        public DefaultConsoleAppender(bool enabled)
        {
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            Format = DefaultFormat;
            Enabled = enabled;
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
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="format">The format in which the Log object should be outputted to the console</param>
        public DefaultConsoleAppender(string format)
        {
            LogLevels = new List<LogLevel>();
            Format = format;
            Enabled = true;
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

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to the console
        /// </summary>
        /// <param name="log">The Log object that should be exported to the console</param>
        public override void ExportLog(Log log)
        {
            if (!ValidExport(log)) return;

            switch (log.LogLevel)
            {
                case LogLevel.Trace:
                    System.Diagnostics.Trace.WriteLine(FormatLog(log));
                    break;
                case LogLevel.Debug:
                    System.Diagnostics.Debug.WriteLine(FormatLog(log));
                    break;
                case LogLevel.Info:
                case LogLevel.Warning:
                case LogLevel.Error:
                    System.Console.WriteLine(FormatLog(log));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
                switch (log.LogLevel)
                {
                    case LogLevel.Trace:
                        System.Diagnostics.Trace.WriteLine(FormatLog(log));
                        break;
                    case LogLevel.Debug:
                        System.Diagnostics.Debug.WriteLine(FormatLog(log));
                        break;
                    case LogLevel.Warning:
                    case LogLevel.Info:
                    case LogLevel.Error:
                        await System.Console.Out.WriteLineAsync(FormatLog(log));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
