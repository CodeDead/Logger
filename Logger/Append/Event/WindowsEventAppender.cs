using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.Event
{
    /// <inheritdoc />
    /// <summary>
    /// Sealed class that can be used to handle writing Log objects to the Windows Event Log
    /// </summary>
    public sealed class WindowsEventAppender : EventAppender
    {
        #region Variables
        private string _name;
        private const string DefaultFormat = "[%l](%C)\t-\t%c";
        #endregion

        #region Properties
        /// <summary>
        /// The name of the EventLog that should be used to add logs in the Windows Event Log
        /// </summary>
        [XmlElement("Name")]
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly
        /// </summary>
        [XmlElement("EventSource")]
        public string EventSource { get; set; }

        /// <summary>
        /// Property that sets the format in which Log objects should be displayed in the console
        /// </summary>
        [XmlElement("Format")]
        public string Format { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        public WindowsEventAppender()
        {
            // This should only be used for serialization
            Name = "";
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        public WindowsEventAppender(string name)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            Enabled = true;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="logLevels">The List of log levels that can be exported</param>
        public WindowsEventAppender(string name, List<LogLevel> logLevels)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = logLevels;
            Enabled = true;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="enabled">True if exporting Log objects is enabled, otherwise false</param>
        public WindowsEventAppender(string name, bool enabled)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            Enabled = enabled;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="logLevels">The List of log levels that can be exported</param>
        /// <param name="enabled">True if exporting Log objects is enabled, otherwise false</param>
        public WindowsEventAppender(string name, List<LogLevel> logLevels, bool enabled)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = logLevels;
            Enabled = enabled;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        public WindowsEventAppender(string name, string eventSource)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            EventSource = eventSource;
            Enabled = true;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="format">The format that can be used to display messages in the Windows Event Log</param>
        public WindowsEventAppender(string name, string eventSource, string format)
        {
            Name = name;
            Format = format;
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            EventSource = eventSource;
            Enabled = true;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="logLevels">The List of log levels that can be exported</param>
        public WindowsEventAppender(string name, string eventSource, List<LogLevel> logLevels)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = logLevels;
            EventSource = eventSource;
            Enabled = true;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="format">The format that can be used to display messages in the Windows Event Log</param>
        /// <param name="logLevels">The List of log levels that can be exported</param>
        public WindowsEventAppender(string name, string eventSource, string format, List<LogLevel> logLevels)
        {
            Name = name;
            Format = format;
            LogLevels = logLevels;
            EventSource = eventSource;
            Enabled = true;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="enabled">True if exporting Log objects is enabled, otherwise false</param>
        public WindowsEventAppender(string name, string eventSource, bool enabled)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            EventSource = eventSource;
            Enabled = enabled;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="format">The format that can be used to display messages in the Windows Event Log</param>
        /// <param name="enabled">True if exporting Log objects is enabled, otherwise false</param>
        public WindowsEventAppender(string name, string eventSource, string format, bool enabled)
        {
            Name = name;
            Format = format;
            LogLevels = new List<LogLevel> { LogLevel.Trace, LogLevel.Debug, LogLevel.Info, LogLevel.Warning, LogLevel.Error };
            EventSource = eventSource;
            Enabled = enabled;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="logLevels">The List of log levels that can be exported</param>
        /// <param name="enabled">True if exporting Log objects is enabled, otherwise false</param>
        public WindowsEventAppender(string name, string eventSource, List<LogLevel> logLevels, bool enabled)
        {
            Name = name;
            Format = DefaultFormat;
            LogLevels = logLevels;
            EventSource = eventSource;
            Enabled = enabled;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Initialize a new WindowsEventAppender
        /// </summary>
        /// <param name="name">The name of the EventLog that should be used to add logs in the Windows Event Log</param>
        /// <param name="eventSource">The EventSource that can be used to link an event log to a source. This could be the name of your application but it might require administrative privileges to configure this correctly</param>
        /// <param name="format">The format that can be used to display messages in the Windows Event Log</param>
        /// <param name="logLevels">The List of log levels that can be exported</param>
        /// <param name="enabled">True if exporting Log objects is enabled, otherwise false</param>
        public WindowsEventAppender(string name, string eventSource, string format, List<LogLevel> logLevels, bool enabled)
        {
            Name = name;
            Format = format;
            LogLevels = logLevels;
            EventSource = eventSource;
            Enabled = enabled;

            EventSource = CreateEventSource();
        }

        /// <summary>
        /// Create an EventSource in the Windows Event Log. This might require administrative privileges and will default to 'Application' if a SecurityException is caught
        /// </summary>
        /// <returns>The name of the EventSource that this instance is allowed to use</returns>
        private string CreateEventSource()
        {
            string eventSource = EventSource;
            if (string.IsNullOrEmpty(eventSource)) eventSource = "Application";

            try
            {
                // Check if an EventSource exists
                bool sourceExists = EventLog.SourceExists(eventSource);
                if (!sourceExists)
                {   // Create the EventSource
                    if (string.IsNullOrEmpty(Name)) Name = "Application";
                    EventLog.CreateEventSource(eventSource, Name);
                }
            }
            catch (SecurityException)
            {
                eventSource = "Application";
            }

            return eventSource;
        }

        /// <summary>
        /// Format a Log object into a printable string
        /// </summary>
        /// <param name="log">The Log object that should be converted into a printable string</param>
        /// <returns>The string representation of a Log object using the given format</returns>
        private string FormatLog(Log log)
        {
            string output = Format
                .Replace("%d", log.LogDate.ToString(CultureInfo.InvariantCulture))
                .Replace("%l", Enum.GetName(typeof(LogLevel), log.LogLevel))
                .Replace("%c", log.Content)
                .Replace("%C", log.Context);
            return output;
        }

        /// <summary>
        /// Validate whether a Log object can be exported or not
        /// </summary>
        /// <param name="log">The Log object that should be validated</param>
        /// <returns>True if the Log object can be exported, otherwise false</returns>
        private bool ValidLog(Log log)
        {
            if (!Enabled) return false;
            if (log == null) throw new ArgumentNullException(nameof(log));
            return LogLevels.Contains(log.LogLevel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Write a Log object to the Windows Event Log
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
            if (!ValidLog(log)) return;
            using (EventLog eventLog = new EventLog(Name))
            {
                eventLog.Source = EventSource;

                EventLogEntryType entryType;
                switch (log.LogLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                    case LogLevel.Info:
                        entryType = EventLogEntryType.Information;
                        break;
                    case LogLevel.Warning:
                        entryType = EventLogEntryType.Warning;
                        break;
                    case LogLevel.Error:
                        entryType = EventLogEntryType.Error;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                eventLog.WriteEntry(FormatLog(log), entryType);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Write a Log object to the Windows Event Log asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            await Task.Run(() => { ExportLog(log); });
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public override void Dispose()
        {
            // No resources need to be disposed
        }
    }
}
