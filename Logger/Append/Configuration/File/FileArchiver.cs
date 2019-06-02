using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CodeDead.Logger.Utility;

namespace CodeDead.Logger.Append.Configuration.File
{
    /// <summary>
    /// Sealed class containing the logic for archiving files
    /// </summary>
    public sealed class FileArchiver : FileConfiguration
    {
        #region Variables
        private string _zipPath;
        #endregion

        #region Properties
        /// <summary>
        /// The path where the ZIP archive should be stored
        /// </summary>
        [XmlElement("ZipPath")]
        public string ZipPath
        {
            get => _zipPath;
            set => _zipPath = value ?? throw new ArgumentNullException(nameof(value));
        }
        #endregion

        /// <summary>
        /// Initialize a new FileArchiver
        /// </summary>
        public FileArchiver()
        {
            // Empty constructor
        }

        /// <summary>
        /// Initialize a new FileArchiver
        /// </summary>
        /// <param name="path">The path where the archive file should be stored</param>
        public FileArchiver(string path)
        {
            ZipPath = path;
        }

        /// <summary>
        /// Method that is called when the FileArchiver should act
        /// </summary>
        public override void Invoke(List<string> files)
        {
            if (!Enabled || string.IsNullOrEmpty(ZipPath)
                || InvokePolicies == null || InvokePolicies.Count == 0) return;

            List<string> invokedFiles = new List<string>();
            foreach (InvokePolicy.InvokePolicy policy in InvokePolicies)
            {
                invokedFiles.AddRange(files.Where(file => policy.ShouldInvoke(file)));
            }

            if (invokedFiles.Count == 0) return;
            ZipUtility.AddFilesToZip(ZipPath, invokedFiles, true);
        }
    }
}