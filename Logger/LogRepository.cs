using System.Collections.Generic;
using System.Linq;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger
{
    /// <summary>
    /// Internal sealed class that contains the repository logic
    /// </summary>
    internal sealed class LogRepository
    {
        #region Variables
        private readonly List<Log> _logs;
        #endregion

        /// <summary>
        /// Initialize a new LogRepository object
        /// </summary>
        internal LogRepository()
        {
            _logs = new List<Log>();
        }

        /// <summary>
        /// Get the List of Log objects
        /// </summary>
        /// <returns>The List of Log objects</returns>
        internal List<Log> GetLogs()
        {
            return _logs;
        }

        /// <summary>
        /// Get the List of Log objects having a specified LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having a specified LogLevel</returns>
        internal List<Log> GetLogs(LogLevel? logLevel)
        {
            if (logLevel == null) return GetLogs();

            List<Log> retrieved = _logs.Where(l => l.LogLevel == logLevel).ToList();
            return retrieved;
        }

        /// <summary>
        /// Get the List of Log objects having a specified context
        /// </summary>
        /// <param name="context">The context for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having a specified context</returns>
        internal List<Log> GetLogs(string context)
        {
            if (context == null) return GetLogs();

            List<Log> retrieved = _logs.Where(l => l.Context == context).ToList();
            return retrieved;
        }

        /// <summary>
        /// Get the List of Log objects having a specified LogLevel and context
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be retrieved. Can be null</param>
        /// <param name="context">The context for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having the specified LogLevel and context</returns>
        internal List<Log> GetLogs(LogLevel? logLevel, string context)
        {
            if (logLevel == null && context == null) return GetLogs();
            if (logLevel == null) return GetLogs(context);
            if (context == null) return GetLogs(logLevel);

            List<Log> retrieved = _logs.Where(l => l.Context == context && l.LogLevel == logLevel).ToList();

            return retrieved;
        }

        /// <summary>
        /// Add a Log object to the list of logs
        /// </summary>
        /// <param name="log">The Log object that should be added to the list</param>
        internal void AddLog(Log log)
        {
            _logs.Add(log);
        }

        /// <summary>
        /// Remove a Log object from the list of logs
        /// </summary>
        /// <param name="log">The Log object that should be removed</param>
        internal void RemoveLog(Log log)
        {
            _logs.Remove(log);
        }

        /// <summary>
        /// Clear all Log objects
        /// </summary>
        internal void ClearLogs()
        {
            _logs.Clear();
        }

        /// <summary>
        /// Clear all Log objects having a specified LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be removed. Can be null</param>
        internal void ClearLogs(LogLevel? logLevel)
        {
            if (logLevel == null)
            {
                ClearLogs();
                return;
            }

            _logs.RemoveAll(l => l.LogLevel == logLevel);
        }

        /// <summary>
        /// Clear all Log objects having a specified LogLevel and context
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be removed. Can be null</param>
        /// <param name="context">The context for which Log objects should be removed. Can be null</param>
        internal void ClearLogs(LogLevel? logLevel, string context)
        {
            if (logLevel == null && context == null)
            {
                ClearLogs();
                return;
            }

            if (logLevel == null)
            {
                ClearLogs(context);
                return;
            }

            if (context == null)
            {
                ClearLogs(logLevel);
                return;
            }

            _logs.RemoveAll(l => l.LogLevel == logLevel && l.Context == context);
        }

        /// <summary>
        /// Clear all Log objects having a specified context
        /// </summary>
        /// <param name="context">The context for which Log objects should be removed. Can be null</param>
        internal void ClearLogs(string context)
        {
            if (context == null) ClearLogs();

            _logs.RemoveAll(l => l.Context == context);
        }
    }
}
