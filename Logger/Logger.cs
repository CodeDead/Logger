using System;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger
{
    /// <summary>
    /// Sealed class containing the logger logic
    /// </summary>
    public sealed class Logger
    {
        #region Properties
        /// <summary>
        /// Property that contains the LogManager object for this Logger instance
        /// </summary>
        [XmlElement("LogManager")]
        public LogManager LogManager { get; set; }

        /// <summary>
        /// Property that sets whether exceptions will be thrown or not when calling methods.
        /// Invalid property exceptions will still be thrown
        /// </summary>
        [XmlElement("ThrowExceptions")]
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// Property that sets whether the logging capabilities are enabled or not
        /// </summary>
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Property that sets whether warning logging is enabled or not
        /// </summary>
        [XmlElement("WarningEnabled")]
        public bool WarningEnabled { get; set; }
        
        /// <summary>
        /// Property that sets whether trace logging is enabled or not
        /// </summary>
        [XmlElement("TraceEnabled")]
        public bool TraceEnabled { get; set; }
        
        /// <summary>
        /// Property that sets whether info logging is enabled or not
        /// </summary>
        [XmlElement("InfoEnabled")]
        public bool InfoEnabled { get; set; }
        
        /// <summary>
        /// Property that sets whether debug logging is enabled or not
        /// </summary>
        [XmlElement("DebugEnabled")]
        public bool DebugEnabled { get; set; }

        /// <summary>
        /// Property that sets whether error logging is enabled or not
        /// </summary>
        [XmlElement("ErrorEnabled")]
        public bool ErrorEnabled { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new Logger
        /// </summary>
        public Logger()
        {
            // Empty constructor, required for (de)serialization
        }

        /// <summary>
        /// Initialize a new Logger object
        /// </summary>
        /// <param name="logManager">The LogManager object that can be used to pass logs</param>
        public Logger(LogManager logManager)
        {
            LogManager = logManager;

            Enabled = true;
            ThrowExceptions = false;

            TraceEnabled = true;
            DebugEnabled = true;
            InfoEnabled = true;
            WarningEnabled = true;
            ErrorEnabled = true;
        }

        private void AddLog(string content, string context, LogLevel logLevel)
        {
            try
            {
                LogManager.AddLog(context != null ? new Log(content, context, logLevel) : new Log(content, logLevel));
            }
            catch (Exception ex)
            {
                if (ThrowExceptions) throw;
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Write a new trace Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log</param>
        public void Trace(string content)
        {
            if (!Enabled || !TraceEnabled) return;
            AddLog(content, null, LogLevel.Trace);
        }

        /// <summary>
        /// Write a new trace Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context that applies to the Log object</param>
        public void Trace(string content, string context)
        {
            if (!Enabled || !TraceEnabled) return;
            AddLog(content, context, LogLevel.Trace);
        }

        /// <summary>
        /// Write a new debug Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        public void Debug(string content)
        {
            if (!Enabled || !DebugEnabled) return;
            AddLog(content, null, LogLevel.Debug);
        }
        
        /// <summary>
        /// Write a new debug Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context that applies to the Log object</param>
        public void Debug(string content, string context)
        {
            if (!Enabled || !DebugEnabled) return;
            AddLog(content, context, LogLevel.Debug);
        }

        /// <summary>
        /// Write a new info Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        public void Info(string content)
        {
            if (!Enabled || !InfoEnabled) return;
            AddLog(content, null, LogLevel.Info);
        }

        /// <summary>
        /// Write a new info Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context that applies to the Log object</param>
        public void Info(string content, string context)
        {
            if (!Enabled || !InfoEnabled) return;
            AddLog(content, context, LogLevel.Info);
        }

        /// <summary>
        /// Write a new warning Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        public void Warn(string content)
        {
            if (!Enabled || !WarningEnabled) return;
            AddLog(content, null, LogLevel.Warning);
        }

        /// <summary>
        /// Write a new warning Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context that applies to the Log object</param>
        public void Warn(string content, string context)
        {
            if (!Enabled || !WarningEnabled) return;
            AddLog(content, context, LogLevel.Warning);
        }

        /// <summary>
        /// Write a new error Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        public void Error(string content)
        {
            if (!Enabled || !ErrorEnabled) return;
            AddLog(content, null, LogLevel.Error);
        }

        /// <summary>
        /// Write a new error Log object
        /// </summary>
        /// <param name="content">The content that should be added to the Log object</param>
        /// <param name="context">The context that applies to the Log object</param>
        public void Error(string content, string context)
        {
            if (!Enabled || !ErrorEnabled) return;
            AddLog(content, context, LogLevel.Error);
        }

        /// <summary>
        /// Write a new error Log object
        /// </summary>
        /// <param name="ex">The Exception object that should be added to the Log</param>
        public void Error(Exception ex)
        {
            if (!Enabled && !ErrorEnabled) return;
            AddLog(ex.Message + Environment.NewLine + ex.StackTrace, null, LogLevel.Error);
        }

        /// <summary>
        /// Write a new error Log object
        /// </summary>
        /// <param name="ex">The Exception object that should be added to the Log</param>
        /// <param name="context">The context that applies to the Log object</param>
        public void Error(Exception ex, string context)
        {
            if (!Enabled && !ErrorEnabled) return;
            AddLog(ex.Message + Environment.NewLine + ex.StackTrace, context, LogLevel.Error);
        }
    }
}
