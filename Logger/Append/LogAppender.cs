using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Append.Console;
using CodeDead.Logger.Append.Event;
using CodeDead.Logger.Append.File;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract class that can be used to export Log objects
    /// </summary>
    [XmlInclude(typeof(FileAppender))]
    [XmlInclude(typeof(ConsoleAppender))]
    [XmlInclude(typeof(EventAppender))]
    public abstract class LogAppender : IDisposable
    {
        #region Variables
        private List<LogLevel> _logLevels;
        internal static readonly List<LogLevel> DefaultLogLevels = new List<LogLevel>{ LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error};
        #endregion

        #region Properties
        /// <summary>
        /// Property that displays whether the LogAppender is enabled or not
        /// </summary>
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// The List of log levels that should be exported
        /// </summary>
        [XmlArray("LogLevels"), XmlArrayItem(typeof(LogLevel), ElementName = "LogLevel")]
        public List<LogLevel> LogLevels
        {
            get => _logLevels;
            set => _logLevels = value ?? throw new ArgumentNullException(nameof(value));
        }
        #endregion

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        protected LogAppender()
        {
            LogLevels = new List<LogLevel>();
        }

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        /// <param name="enabled">True if exporting logs should be enabled, otherwise false</param>
        protected LogAppender(bool enabled)
        {
            LogLevels = new List<LogLevel>();
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        protected LogAppender(List<LogLevel> logLevels)
        {
            LogLevels = logLevels;
        }

        /// <summary>
        /// Initialize a new LogAppender object
        /// </summary>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting logs should be enabled, otherwise false</param>
        protected LogAppender(List<LogLevel> logLevels, bool enabled)
        {
            LogLevels = logLevels;
            Enabled = enabled;
        }

        /// <summary>
        /// Export a Log object
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public abstract void ExportLog(Log log);

        /// <summary>
        /// Export a Log object asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task that is associated with this asynchronous method</returns>
        public abstract Task ExportLogAsync(Log log);

        /// <inheritdoc />
        /// <summary>
        /// Dispose of all resources
        /// </summary>
        public abstract void Dispose();
    }
}
