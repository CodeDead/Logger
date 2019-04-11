using System;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract class containing the file appending logic
    /// Inherit this class to implement your own file appending logic
    /// </summary>
    public abstract class FileAppender : LogAppender
    {
        #region Variables
        private string _filePath;
        #endregion

        #region Properties
        /// <summary>
        /// Property that contains the path to which a Log object should be written
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set => _filePath = value ?? throw new ArgumentNullException(nameof(value));
        }
        #endregion
    }
}
