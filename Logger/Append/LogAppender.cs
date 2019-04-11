using System;
using System.Collections.Generic;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract class that can be used to export Log objects
    /// </summary>
    public abstract class LogAppender : ILogAppender
    {
        #region Variables
        private List<LogLevel> _logLevels;
        private string _format;
        #endregion

        #region Properties
        /// <summary>
        /// Property that displays whether the LogAppender is enabled or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Property that sets whether Log objects should be written asynchronously or not
        /// </summary>
        public bool Asynchronous { get; set; }

        /// <summary>
        /// Property that sets the format in which Log objects should be displayed in the console
        /// </summary>
        public string Format
        {
            get => _format;
            set => _format = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The List of log levels that should be exported
        /// </summary>
        public List<LogLevel> LogLevels
        {
            get => _logLevels;
            set => _logLevels = value ?? throw new ArgumentNullException(nameof(value));
        }
        #endregion

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        public LogAppender()
        {
            LogLevels = new List<LogLevel>();
        }

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        /// <param name="enabled">True if exporting logs should be enabled, otherwise false</param>
        public LogAppender(bool enabled)
        {
            LogLevels = new List<LogLevel>();
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public LogAppender(List<LogLevel> logLevels)
        {
            LogLevels = logLevels;
        }

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting logs should be enabled, otherwise false</param>
        public LogAppender(List<LogLevel> logLevels, bool enabled)
        {
            LogLevels = logLevels;
            Enabled = enabled;
        }

        /// <summary>
        /// Add a LogLevel to the List of log levels that should be exported
        /// </summary>
        /// <param name="logLevel">The LogLevel that should be added</param>
        public void AddLogLevel(LogLevel logLevel)
        {
            _logLevels.Add(logLevel);
        }

        /// <summary>
        /// Remove a LogLevel from the List of log levels that should be exported
        /// </summary>
        /// <param name="logLevel">The LogLevel that should be removed</param>
        public void RemoveLogLevel(LogLevel logLevel)
        {
            _logLevels.Remove(logLevel);
        }

        /// <summary>
        /// Export a Log object
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public abstract void ExportLog(Log log);
    }
}
