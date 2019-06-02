using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CodeDead.Logger.Utility
{
    /// <summary>
    /// Static class containing ZIP logic
    /// </summary>
    public static class ZipUtility
    {
        /// <summary>
        /// Add files to an existing ZIP archive
        /// </summary>
        /// <param name="zipPath">The path of the ZIP archive</param>
        /// <param name="files">The list of files that should be added to the ZIP archive</param>
        /// <param name="deleteFiles">Sets whether the files that are zipped should be deleted or not</param>
        public static void AddFilesToZip(string zipPath, List<string> files, bool deleteFiles)
        {
            if (files == null || files.Count == 0)
            {
                return;
            }

            ZipArchiveMode mode = ZipArchiveMode.Create;
            if (File.Exists(zipPath))
            {
                mode = ZipArchiveMode.Update;
            }

            using (ZipArchive zipArchive = ZipFile.Open(zipPath, mode))
            {
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    zipArchive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                    if (deleteFiles)
                    {
                        File.Delete(file);
                    }
                }
            }
        }
    }
}