using System;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger
{
    /// <summary>
    /// Sealed class containing the logger logic
    /// </summary>
    public sealed class Logger
    {
        #region Variables
        private readonly LogManager _logManager;
        private bool _enabled;

        private int _maxInMemory;
        private bool _clearMemory;
        #endregion

        #region Properties
        /// <summary>
        /// Property that sets whether exceptions will be thrown or not when calling methods.
        /// Invalid property exceptions will still be thrown
        /// </summary>
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// Property that sets whether the logging capabilities are enabled or not
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                _logManager.Enabled = value;
            }
        }

        /// <summary>
        /// Property that displays the maximum amount of Log objects that should be kept in memory.
        /// If the maximum number is reached, the oldest Log object will be cleared from memory
        /// </summary>
        public int MaxInMemory
        {
            get => _maxInMemory;
            set
            {
                if (value < 1) throw new ArgumentException(nameof(value));
                _maxInMemory = value;
                _logManager.MaxInMemory = value;
            }
        }

        /// <summary>
        /// Property that sets whether logs should be removed from memory or not after a certain threshold is reached. See <see cref="MaxInMemory"/>
        /// </summary>
        public bool ClearMemory
        {
            get => _clearMemory;
            set
            {
                _clearMemory = value;
                _logManager.ClearMemory = value;
            }
        }

        /// <summary>
        /// Property that sets whether warning logging is enabled or not
        /// </summary>
        public bool WarningEnabled { get; set; }
        
        /// <summary>
        /// Property that sets whether trace logging is enabled or not
        /// </summary>
        public bool TraceEnabled { get; set; }
        
        /// <summary>
        /// Property that sets whether info logging is enabled or not
        /// </summary>
        public bool InfoEnabled { get; set; }
        
        /// <summary>
        /// Property that sets whether debug logging is enabled or not
        /// </summary>
        public bool DebugEnabled { get; set; }

        /// <summary>
        /// Property that sets whether error logging is enabled or not
        /// </summary>
        public bool ErrorEnabled { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new Logger object
        /// </summary>
        /// <param name="logManager">The LogManager object that can be used to pass logs</param>
        public Logger(LogManager logManager)
        {
            _logManager = logManager;

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
                _logManager.AddLog(context != null ? new Log(content, context, logLevel) : new Log(content, logLevel));
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
        /// Get the LogManager object
        /// </summary>
        /// <returns>The LogManager object</returns>
        public LogManager GetLogManager()
        {
            return _logManager;
        }
    }
}
