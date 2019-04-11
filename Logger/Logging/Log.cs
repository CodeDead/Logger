using System;

namespace CodeDead.Logger.Logging
{
    /// <summary>
    /// Sealed class containing the Log object logic
    /// </summary>
    public sealed class Log
    {
        #region Variables
        private string _logContent;
        #endregion

        #region Properties
        /// <summary>
        /// The DateTime at which the Log object was created
        /// </summary>
        public DateTime LogDate { get; }

        /// <summary>
        /// The content of this Log object
        /// </summary>
        public string Content
        {
            get => _logContent;
            set => _logContent = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The LogLevel of this Log object
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// The context of this Log object
        /// </summary>
        public string Context { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        public Log(string content)
        {
            Content = content;
            LogDate = DateTime.Now;
        }

        /// <summary>
        /// Initialize a new Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context for which the content applies</param>
        public Log(string content, string context)
        {
            Content = content;
            Context = context;
            LogDate = DateTime.Now;
        }

        /// <summary>
        /// Initialize a new Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="logLevel">The LogLevel that is associated with the Log object</param>
        public Log(string content, LogLevel logLevel)
        {
            Content = content;
            LogLevel = logLevel;
            LogDate = DateTime.Now;
        }

        /// <summary>
        /// Initialize a new Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context for which the content applies</param>
        /// <param name="logLevel">The LogLevel that is associated with the Log object</param>
        public Log(string content, string context, LogLevel logLevel)
        {
            Content = content;
            Context = context;
            LogLevel = logLevel;
            LogDate = DateTime.Now;
        }
    }
}
