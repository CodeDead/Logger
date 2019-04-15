using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Sealed class containing the default file appender logic
    /// </summary>
    public sealed class DefaultFileAppender : FileAppender
    {
        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        public DefaultFileAppender()
        {
            LogLevels = new List<LogLevel>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public DefaultFileAppender(bool enabled)
        {
            LogLevels = new List<LogLevel>();
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
        }

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new DefaultFileAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public DefaultFileAppender(List<LogLevel> logLevels, bool enabled)
        {
            LogLevels = logLevels;
            Enabled = enabled;
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to a file
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
            if (!Enabled) return;
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (!LogLevels.Contains(log.LogLevel)) return;
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object to a file asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            throw new NotImplementedException();
        }
    }
}
