using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CodeDead.Logger.Configuration;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.File
{
    /// <inheritdoc />
    /// <summary>
    /// Sealed class that contains the logic to store Log objects as XML
    /// </summary>
    public sealed class XmlFileAppender : FileAppender
    {
        #region Variables
        private readonly XmlSerializer _serializer;
        #endregion

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        public XmlFileAppender()
        {
            LogLevels = DefaultLogLevels;
            _serializer = new XmlSerializer(typeof(LogRoot));
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        public XmlFileAppender(string path)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public XmlFileAppender(string path, bool enabled)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        public XmlFileAppender(string path, List<LogLevel> logLevels)
        {
            FilePath = path;
            LogLevels = logLevels;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        public XmlFileAppender(string path, List<LogLevel> logLevels, bool enabled)
        {
            FilePath = path;
            LogLevels = logLevels;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = enabled;
        }

        /// <summary>
        /// Validate whether a Log can be exported or not
        /// </summary>
        /// <param name="log">The Log object that needs to be validated</param>
        /// <returns>True if a Log can be exported, otherwise false</returns>
        private bool ValidLog(Log log)
        {
            if (!Enabled) return false;
            if (log == null) throw new ArgumentNullException(nameof(log));
            return LogLevels.Contains(log.LogLevel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object as an XML string
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        public override void ExportLog(Log log)
        {
            if (!ValidLog(log)) return;
            string readContents = null;
            try
            {
                using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Read the contents of the file
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        readContents = sr.ReadToEnd();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // Ignored
            }


            LogRoot root = null;
            if (!string.IsNullOrEmpty(readContents))
            {
                // Convert the XML into a LogRoot object
                using (TextReader reader = new StringReader(readContents))
                {
                    root = (LogRoot)_serializer.Deserialize(reader);
                }
            }

            if (root == null) root = new LogRoot();

            // Add a Log to the LogRoot object
            root.Logs.Add(log);


            using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                string xml;
                using (StringWriter sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        // Convert the object back to XML
                        _serializer.Serialize(writer, root);
                        xml = sww.ToString();
                    }
                }

                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(xml);
                    sw.Flush();
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Export a Log object as an XML string asynchronously
        /// </summary>
        /// <param name="log">The Log object that should be exported</param>
        /// <returns>The Task object that is associated with this method</returns>
        public override async Task ExportLogAsync(Log log)
        {
            await Task.Run(async () =>
            {
                if (!ValidLog(log)) return;
                string readContents = null;
                try
                {
                    using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read the contents of the file
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            readContents = await sr.ReadToEndAsync();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    // Ignored
                }


                LogRoot root = null;
                if (!string.IsNullOrEmpty(readContents))
                {
                    // Convert the XML into a LogRoot object
                    using (TextReader reader = new StringReader(readContents))
                    {
                        root = (LogRoot)_serializer.Deserialize(reader);
                    }
                }

                if (root == null) root = new LogRoot();

                // Add a Log to the LogRoot object
                root.Logs.Add(log);


                using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {

                    string xml;
                    using (StringWriter sww = new StringWriter())
                    {
                        using (XmlWriter writer = XmlWriter.Create(sww))
                        {
                            // Convert the object back to XML
                            _serializer.Serialize(writer, root);
                            xml = sww.ToString();
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        await sw.WriteAsync(xml);
                        await sw.FlushAsync();
                    }
                }
            });
        }

        /// <inheritdoc />
        /// <summary>
        /// Dispose of all resources
        /// </summary>
        public override void Dispose()
        {
            // Nothing needs to be disposed
        }
    }
}
