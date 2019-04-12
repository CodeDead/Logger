using System.Collections.Generic;
using System.Xml.Serialization;

namespace CodeDead.Logger.Configuration
{
    /// <summary>
    /// Sealed class that is used for saving and loading Logger objects to the filesystem
    /// </summary>
    [XmlRoot("LoggerRoot")]
    public sealed class LoggerRoot
    {
        #region Properties
        /// <summary>
        /// Property that contains the List of Logger objects that can be saved to the filesystem or were loaded from the filesystem
        /// </summary>
        [XmlElement("Logger")]
        public List<Logger> Loggers;
        #endregion

        /// <summary>
        /// Initialize a new LoggerRoot object
        /// </summary>
        public LoggerRoot()
        {
            Loggers = new List<Logger>();
        }
    }
}
