using System;
using System.IO;
using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Configuration.File.InvokePolicy
{
    /// <summary>
    /// Sealed class that contains the file size policy logic
    /// </summary>
    public sealed class FileSizePolicy : InvokePolicy
    {
        #region Variables
        private long _maxFileSize;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the maximum size of a file before a FileConfiguration should be invoked
        /// </summary>
        /// <exception cref="ArgumentException">When the value is equal to or smaller than zero</exception>
        [XmlElement("MaxFileSize")]
        public long MaxFileSize
        {
            get => _maxFileSize;
            set
            {
                if (value <= 0) throw new ArgumentException(nameof(value));
                _maxFileSize = value;
            }
        }
        #endregion

        /// <summary>
        /// Initialize a new FileSizePolicy
        /// </summary>
        public FileSizePolicy()
        {
            // Empty constructor
        }

        /// <summary>
        /// Initialize a new FileSizePolicy
        /// </summary>
        /// <param name="maxFileSize">The maximum size of a file in bytes</param>
        public FileSizePolicy(long maxFileSize)
        {
            MaxFileSize = maxFileSize;
        }

        /// <summary>
        /// Method that is called when the file size policy should be checked
        /// </summary>
        /// <param name="filePath">The path of the file that should be checked</param>
        /// <returns>True if the FileConfiguration should be invoked, otherwise false</returns>
        public override bool ShouldInvoke(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return false;
            FileInfo fi = new FileInfo(filePath);
            return fi.Length >= MaxFileSize;
        }
    }
}