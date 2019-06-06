using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.Memory
{
    public sealed class DefaultMemoryAppender : MemoryAppender
    {
        #region Variables
        private int _maxInMemory;
        #endregion
        
        #region Delegates
        public delegate void LogRemoved(Log log);
        public delegate void LogsCleared();
        public delegate void LogsClearedByLogLevel(LogLevel? logLevel);
        public delegate void LogsClearedByContext(string context);
        public delegate void LogsClearedByLogLevelAndContext(LogLevel? logLevel, string context);
        public delegate void LogsRetrieved(List<Log> logs);
        #endregion

        #region Events
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
        
        #region Properties
        /// <summary>
        /// Gets or sets the maximum amount of logs that can be stored in memory. Set to zero if there is no limit
        /// </summary>
        /// <exception cref="ArgumentException">When the value is smaller than zero</exception>
        [XmlElement("MaxInMemory")]
        public int MaxInMemory
        {
            get => _maxInMemory;
            set
            {
                if (value < 0) throw new ArgumentException(nameof(value));
                _maxInMemory = value;
            }
        }
        #endregion
        
        /// <summary>
        /// Initialize a new DefaultMemoryAppender
        /// </summary>
        public DefaultMemoryAppender()
        {
            _maxInMemory = 0;
            LogList = new List<Log>();
        }

        /// <summary>
        /// Initialize a new DefaultMemoryAppender
        /// </summary>
        /// <param name="maxInMemory">The maximum amount of logs that can be stored in memory. Set to zero if there is no limit</param>
        public DefaultMemoryAppender(int maxInMemory)
        {
            MaxInMemory = maxInMemory;
            LogList = new List<Log>();
        }
        
        /// <summary>
        /// Method that is called when a Log object should be stored in memory
        /// </summary>
        /// <param name="log">The Log object that should be stored in memory</param>
        public override void ExportLog(Log log)
        {
            CleanLogs();
            LogList.Add(log);
        }

        /// <summary>
        /// Method that is called when a Log object should be stored in memory asynchronously
        /// </summary>
        /// <param name="log">The object that should be stured in memory</param>
        /// <returns>The Task that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            await Task.Run(() =>
            {
                CleanLogs();
                LogList.Add(log);
            });
        }

        /// <summary>
        /// Clean the logs if applicable
        /// </summary>
        private void CleanLogs()
        {
            if (MaxInMemory <= 0) return;
            while (LogList.Count + 1 > MaxInMemory)
            {
                RemoveLog(LogList[0]);
            }
        }

        /// <summary>
        /// Get a List of all Log objects
        /// </summary>
        /// <returns>The List of all Log objects</returns>
        public List<Log> GetLogs()
        {
            LogsRetrievedEvent?.Invoke(LogList);
            return LogList;
        }

        /// <summary>
        /// Get a List of Log objects having a specified LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be retrieved. Can be null</param>
        /// <returns>The List of Log objects having a specified LogLevel</returns>
        public List<Log> GetLogs(LogLevel? logLevel)
        {
            if (logLevel == null) return GetLogs();

            List<Log> retrieved = LogList.Where(l => l.LogLevel == logLevel).ToList();
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
            if (context == null) return GetLogs();

            List<Log> retrieved = LogList.Where(l => l.Context == context).ToList();
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
            if (logLevel == null && context == null) return GetLogs();
            if (logLevel == null) return GetLogs(context);
            if (context == null) return GetLogs(logLevel);

            List<Log> retrieved = LogList.Where(l => l.Context == context && l.LogLevel == logLevel).ToList();
            
            LogsRetrievedEvent?.Invoke(retrieved);
            return retrieved;
        }

        /// <summary>
        /// Remove a Log object from memory
        /// </summary>
        /// <param name="log">The Log object that should be removed from memory</param>
        public void RemoveLog(Log log)
        {
            if (LogList.Remove(log))
            {
                LogRemovedEvent?.Invoke(log);                
            }
        }

        /// <summary>
        /// Clear all Log objects
        /// </summary>
        public void ClearLogs()
        {
            LogList.Clear();
            LogsClearedEvent?.Invoke();
        }

        /// <summary>
        /// Clear all Log objects having a specified LogLevel
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be removed. Can be null</param>
        public void ClearLogs(LogLevel? logLevel)
        {
            if (logLevel == null)
            {
                ClearLogs();
                return;
            }
            LogList.RemoveAll(l => l.LogLevel == logLevel);
            LogsClearedByLogLevelEvent?.Invoke(logLevel);
        }

        /// <summary>
        /// Clear the Log objects having a specified context
        /// </summary>
        /// <param name="context">The context for which Log objects should be removed. Can be null</param>
        public void ClearLogs(string context)
        {
            if (context == null)
            {
                ClearLogs();
                return;
            }
            LogList.RemoveAll(l => l.Context == context);
            LogsClearedByContextEvent?.Invoke(context);
        }

        /// <summary>
        /// Clear the Log objects having a specified LogLevel and context
        /// </summary>
        /// <param name="logLevel">The LogLevel for which Log objects should be removed. Can be null</param>
        /// <param name="context">The context for which Log objects should be removed. Can be null</param>
        public void ClearLogs(LogLevel? logLevel, string context)
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

            LogList.RemoveAll(l => l.LogLevel == logLevel && l.Context == context);
            LogsClearedByLogLevelAndContextEvent?.Invoke(logLevel, context);
        }
        
        /// <summary>
        /// Dispose of any used resources
        /// </summary>
        public override void Dispose()
        {
            LogList.Clear();
        }
    }
}
