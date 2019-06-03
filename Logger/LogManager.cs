using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Append;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger
{
    /// <summary>
    /// Sealed class that contains the Log logic
    /// </summary>
    public sealed class LogManager
    {
        #region Properties
        /// <summary>
        /// Property that contains the List of LogAppender objects that are associated with this LogManager instance
        /// </summary>
        [XmlArray("LogAppenders"), XmlArrayItem(typeof(LogAppender), ElementName = "LogAppender")]
        public List<LogAppender> LogAppenders;
        #endregion

        #region Delegates
        public delegate void LogAdded(Log log);
        #endregion

        #region Events
        /// <summary>
        /// Event that is called when a Log object is added to the list of Log objects
        /// </summary>
        public event LogAdded LogAddedEvent;
        #endregion

        /// <summary>
        /// Initialize a new LogManager object
        /// </summary>
        public LogManager()
        {
            LogAppenders = new List<LogAppender>();
        }

        /// <summary>
        /// Add a LogAppender object to the List of LogAppender objects
        /// </summary>
        /// <param name="logAppender">The LogAppender object that should be added to the List of LogAppender objects</param>
        public void AddLogAppender(LogAppender logAppender)
        {
            if (logAppender == null) throw new ArgumentNullException(nameof(logAppender));
            LogAppenders.Add(logAppender);
        }

        /// <summary>
        /// Remove a LogAppender object from the List of LogAppender objects
        /// </summary>
        /// <param name="logAppender">The LogAppender object that should be removed</param>
        public void RemoveLogAppender(LogAppender logAppender)
        {
            LogAppenders.Remove(logAppender);
        }

        /// <summary>
        /// Add a Log object to the log repository
        /// </summary>
        /// <param name="log">The Log that should be added to the log repository</param>
        public void AddLog(Log log)
        {
            foreach (LogAppender exporter in LogAppenders)
            {
                exporter.ExportLog(log);
            }

            LogAddedEvent?.Invoke(log);
        }

        /// <summary>
        /// Add a Log object to the LogRepository instance asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be added to the LogRepository instance</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public async Task AddLogAsync(Log log)
        {
            await Task.Run(async () =>
            {
                foreach (LogAppender exporter in LogAppenders)
                {
                    await exporter.ExportLogAsync(log);
                }

                LogAddedEvent?.Invoke(log);
            });
        }
    }
}
