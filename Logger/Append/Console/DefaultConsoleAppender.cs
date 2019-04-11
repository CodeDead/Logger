using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        public DefaultConsoleAppender()
        {
            Format = DefaultFormat;
            LogLevels = new List<LogLevel>();
        }

        /// <summary>
        /// Initialize a new DefaultConsoleAppender object
        /// </summary>
        /// <param name="enabled">True if exporting Log objects to the console should be enabled, otherwise false</param>
        public DefaultConsoleAppender(bool enabled)
        {
            LogLevels = new List<LogLevel>();
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
            Format = format;
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

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to the console
        /// </summary>
        /// <param name="log">The Log object that should be exported to the console</param>
        public override void ExportLog(Log log)
        {
            if (!Enabled) return;
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (!LogLevels.Contains(log.LogLevel)) return;

            if (Asynchronous)
            {
                Task.Run(async () =>
                {
                    await System.Console.Out.WriteLineAsync(FormatLog(log));
                });
            }
            else
            {
                System.Console.Out.WriteLine(FormatLog(log));
            }
        }
    }
}
