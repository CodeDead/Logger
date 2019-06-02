using System.Collections.Generic;
using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Configuration.File
{
    /// <summary>
    /// Abstract class that contains configuration settings for files
    /// </summary>
    [XmlInclude(typeof(FileArchiver))]
    public abstract class FileConfiguration
    {
        #region Properties
        /// <summary>
        /// Sets whether the FileArchiver is enabled or not
        /// </summary>
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the list that contains the invoke policies
        /// </summary>
        [XmlArray("InvokePolicies"), XmlArrayItem(typeof(InvokePolicy.InvokePolicy), ElementName = "InvokePolicy")]
        public List<InvokePolicy.InvokePolicy> InvokePolicies { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new FileConfiguration
        /// </summary>
        protected FileConfiguration()
        {
            InvokePolicies = new List<InvokePolicy.InvokePolicy>();
        }

        public abstract void Invoke(List<string> files);
    }
}