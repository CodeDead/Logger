using System;
using System.Text;
using System.Xml.Serialization;
using CodeDead.Logger.Append.Configuration.File;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract class containing the file appending logic
    /// Inherit this class to implement your own file appending logic
    /// </summary>
    [XmlInclude(typeof(CsvFileAppender))]
    [XmlInclude(typeof(DefaultFileAppender))]
    [XmlInclude(typeof(JsonFileAppender))]
    [XmlInclude(typeof(XmlFileAppender))]
    public abstract class FileAppender : LogAppender
    {
        #region Variables
        private string _filePath;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the encoding that should be used to write and retrieve data
        /// </summary>
        [XmlElement("TextEncoding")]
        public Encoding TextEncoding { get; set; }
        
        /// <summary>
        /// Gets or sets the path to which a Log object should be written
        /// </summary>
        [XmlElement("FilePath")]
        public string FilePath
        {
            get => _filePath;
            set => _filePath = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Property that contains the FileConfiguration
        /// </summary>
        [XmlElement("FileConfiguration")]
        public FileConfiguration FileConfiguration { get; set; }
        #endregion
    }
}
