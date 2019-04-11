using System;
using System.Collections.Generic;
using CodeDead.Logger.Append;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger
{
    /// <summary>
    /// Sealed class that contains the Log logic
    /// </summary>
    public sealed class LogManager
    {
        #region Variables

        private bool _enabled;
        private readonly LogRepository _logRepository;
        private List<LogAppender> _logAppenders;

        private int _maxInMemory;
        #endregion

        #region Properties
        /// <summary>
        /// Property that sets whether adding Log objects is enabled or not
        /// </summary>
        internal bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                //TODO: log exporters status
            }
        }

        /// <summary>
        /// Property that displays the maximum amount of Log objects that should be kept in memory.
        /// If the maximum number is reached, the oldest Log object will be cleared from memory
        /// </summary>
        internal int MaxInMemory
        {
            get => _maxInMemory;
            set
            {
                if (value < 1) throw new ArgumentException(nameof(value));
                _maxInMemory = value;
            }
        }

        /// <summary>
        /// Property that sets whether logs should be removed from memory or not after a certain threshold is reached. See <see cref="MaxInMemory"/>
        /// </summary>
        internal bool ClearMemory { get; set; }
        #endregion

        #region Delegates
        public delegate void LogAdded(Log log);
        public delegate void LogRemoved(Log log);
        public delegate void LogsCleared();
        public delegate void LogsClearedByLogLevel(LogLevel? logLevel);
        public delegate void LogsClearedByContext(string context);
        public delegate void LogsClearedByLogLevelAndContext(LogLevel? logLevel, string context);
        public delegate void LogsRetrieved(List<Log> logs);
        #endregion

        #region Events
        /// <summary>
        /// Event that is called when a Log object is added to the list of Log objects
        /// </summary>
        public event LogAdded LogAddedEvent;
        /// <summary>
        /// Event that is called when Log objects have been retrieved
        /// </summary>
        public event LogsRetrieved LogsRetrievedEvent;
        /// <summary>
        /// Event that is called when a Log object was removed
        /// </summary>
        public event LogRemoved LogRemovedEvent;
        /// <summary>
        /// Event that is called when all Log objects were cleared
        /// </summary>
        public event LogsCleared LogsClearedEvent;
        /// <summary>
        /// Event that is called when Log objects were cleared by context
        /// </summary>
        public event LogsClearedByContext LogsClearedByContextEvent;
        /// <summary>
        /// Event that is called when Log objects were cleared by LogLevel
        /// </summary>
        public event LogsClearedByLogLevel LogsClearedByLogLevelEvent;
        /// <summary>
        /// Event that is called when Log objects were cleared by LogLevel and context
        /// </summary>
        public event LogsClearedByLogLevelAndContext LogsClearedByLogLevelAndContextEvent;
        #endregion

        /// <summary>
        /// Initialize a new LogManager object
        /// </summary>
        public LogManager()
        {
            _logRepository = new LogRepository();
            _logAppenders = new List<LogAppender>();
        }

        /// <summary>
        /// Set the List of LogAppender objects that can be used to export Log objects
        /// </summary>
        /// <param name="logAppenders">The List of LogAppender objects that can be used to export Log objects</param>
        public void SetLogAppenders(List<LogAppender> logAppenders)
        {
            _logAppenders = logAppenders;
        }

        /// <summary>
        /// Add a LogAppender object to the List of LogAppender objects
        /// </summary>
        /// <param name="logAppender">The LogAppender object that should be added to the List of LogAppender objects</param>
        public void AddLogAppender(LogAppender logAppender)
        {
            if (logAppender == null) throw new ArgumentNullException(nameof(logAppender));
            _logAppenders.Add(logAppender);
        }

        /// <summary>
        /// Remove a LogAppender object from the List of LogAppender objects
        /// </summary>
        /// <param name="logAppender">The LogAppender object that should be removed</param>
        public void RemoveLogAppender(LogAppender logAppender)
        {
            _logAppenders.Remove(logAppender);
        }

        /// <summary>
        /// Get the List of LogAppender objects
        /// </summary>
        /// <returns>The List of LogAppender objects</returns>
        public List<LogAppender> GetLogAppenders()
        {
            return _logAppenders;
        }

        /// <summary>
        /// Get a List of all Log objects
        /// </summary>
        /// <returns>The List of all Log objects</returns>
        public List<Log> GetLogs()
        {
            List<Log> retrieved = _logRepository.GetLogs();
            LogsRetrievedEvent?.Invoke(retrieved);
            return retrieved;
        }

        /// <summary>
        /// Get a List of Log objects having a specified LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having a specified LogLevel</returns>
        public List<Log> GetLogs(LogLevel? logLevel)
        {
            List<Log> retrieved = _logRepository.GetLogs(logLevel);
            LogsRetrievedEvent?.Invoke(retrieved);
            return retrieved;
        }

        /// <summary>
        /// Get a List of Log objects having a specified context
        /// </summary>
        /// <param name="context">The context for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having a specified context</returns>
        public List<Log> GetLogs(string context)
        {
            List<Log> retrieved = _logRepository.GetLogs(context);
            LogsRetrievedEvent?.Invoke(retrieved);
            return retrieved;
        }

        /// <summary>
        /// Get a List of Log objects having a specified LogLevel and context
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be retrieved. Can be null</param>
        /// <param name="context">The context for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having a specified LogLevel and context</returns>
        public List<Log> GetLogs(LogLevel? logLevel, string context)
        {
            List<Log> retrieved = _logRepository.GetLogs(logLevel, context);
            LogsRetrievedEvent?.Invoke(retrieved);

            return retrieved;
        }

        /// <summary>
        /// Add a Log object to the log repository
        /// </summary>
        /// <param name="log">The Log that should be added to the log repository</param>
        public void AddLog(Log log)
        {
            if (ClearMemory && _logRepository.GetLogs().Count + 1 > MaxInMemory)
            {
                _logRepository.RemoveLog(_logRepository.GetLogs()[0]);
            }

            _logRepository.AddLog(log);

            foreach (LogAppender exporter in _logAppenders)
            {
                exporter.ExportLog(log);
            }

            LogAddedEvent?.Invoke(log);
        }

        /// <summary>
        /// Remove a Log object from the log repository
        /// </summary>
        /// <param name="log">The Log object that should be removed from the log repository</param>
        public void RemoveLog(Log log)
        {
            _logRepository.RemoveLog(log);
            LogRemovedEvent?.Invoke(log);
        }

        /// <summary>
        /// Clear all Log objects
        /// </summary>
        public void ClearLogs()
        {
            _logRepository.ClearLogs();
            LogsClearedEvent?.Invoke();
        }

        /// <summary>
        /// Clear all Log objects having a specified LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be removed. Can be null</param>
        public void ClearLogs(LogLevel? logLevel)
        {
            _logRepository.ClearLogs(logLevel);
            LogsClearedByLogLevelEvent?.Invoke(logLevel);
        }

        /// <summary>
        /// Clear the Log objects having a specified context
        /// </summary>
        /// <param name="context">The context for which Log objects should be removed. Can be null</param>
        public void ClearLogs(string context)
        {
            _logRepository.ClearLogs(context);
            LogsClearedByContextEvent?.Invoke(context);
        }

        /// <summary>
        /// Clear the Log objects having a specified LogLevel and context
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be removed. Can be null</param>
        /// <param name="context">The context for which Log objects should be removed. Can be null</param>
        public void ClearLogs(LogLevel? logLevel, string context)
        {
            _logRepository.ClearLogs(logLevel, context);
            LogsClearedByLogLevelAndContextEvent?.Invoke(logLevel, context);
        }
    }
}
