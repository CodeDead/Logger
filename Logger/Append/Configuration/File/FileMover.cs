using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Configuration.File
{
    public sealed class FileMover : FileConfiguration
    {
        #region Variables
        private string _directory;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets where files should be moved to
        /// </summary>
        [XmlElement("Directory")]
        public string Directory
        {
            get => _directory;
            set
            {
                string directory = value;
                if (directory != null)
                {
                    string output = directory.Substring(directory.Length-1, 1);
                    if (output != "\\")
                    {
                        directory += "\\";
                    }
                }
                _directory = directory;
            }
        }
        #endregion

        /// <summary>
        /// Initialize a new FileMover
        /// </summary>
        public FileMover()
        {
            // Empty constructor
        }

        /// <summary>
        /// Initialize a new FileMover
        /// </summary>
        /// <param name="directory">The directory where files should be moved to</param>
        public FileMover(string directory)
        {
            Directory = directory;
        }
        
        /// <summary>
        /// Method that is called when the FileMover should act
        /// </summary>
        /// <param name="files">The list of files that should be validated against the given policies</param>
        public override void Invoke(List<string> files)
        {
            if (!Enabled || string.IsNullOrEmpty(Directory)
                || InvokePolicies == null || InvokePolicies.Count == 0) return;

            List<string> invokedFiles = new List<string>();
            foreach (InvokePolicy.InvokePolicy policy in InvokePolicies)
            {
                invokedFiles.AddRange(files.Where(file => policy.ShouldInvoke(file)));
            }
            
            if (invokedFiles.Count == 0) return;
            foreach (string s in invokedFiles)
            {
                string fileName = System.IO.Path.GetFileName(s);
                System.IO.File.Move(s, Directory + fileName);
            }
        }
    }
}