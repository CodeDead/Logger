using System.Collections.Generic;
using System.Xml.Serialization;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Configuration
{
    /// <summary>
    /// Sealed class that contains a List of Log object, used for converting to/from JSON/XML
    /// </summary>
    [XmlRoot("LogRoot")]
    public sealed class LogRoot
    {
        #region Properties
        /// <summary>
        /// Property that contains a List of Log objects
        /// </summary>
        [XmlArray("Logs"), XmlArrayItem(typeof(Log), ElementName = "Log")]
        public List<Log> Logs;
        #endregion

        public LogRoot()
        {
            Logs = new List<Log>();
        }
    }
}
