using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            TextEncoding = Encoding.Default;
            _serializer = new XmlSerializer(typeof(LogRoot));
        }
        
        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="encoding">The encoding that should be used to read and write data</param>
        public XmlFileAppender(Encoding encoding)
        {
            LogLevels = DefaultLogLevels;
            TextEncoding = encoding;
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
            TextEncoding = Encoding.Default;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="encoding">The encoding that should be used to read and write data</param>
        public XmlFileAppender(string path, Encoding encoding)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            TextEncoding = encoding;
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
            TextEncoding = Encoding.Default;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        /// <param name="encoding">The encoding that should be used to read and write data</param>
        public XmlFileAppender(string path, bool enabled, Encoding encoding)
        {
            FilePath = path;
            LogLevels = DefaultLogLevels;
            TextEncoding = encoding;
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
            TextEncoding = Encoding.Default;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = true;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="encoding">The encoding that should be used to read and write data</param>
        public XmlFileAppender(string path, List<LogLevel> logLevels, Encoding encoding)
        {
            FilePath = path;
            LogLevels = logLevels;
            TextEncoding = encoding;
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
            TextEncoding = Encoding.Default;
            _serializer = new XmlSerializer(typeof(LogRoot));
            Enabled = enabled;
        }

        /// <summary>
        /// Initialize a new XmlFileAppender
        /// </summary>
        /// <param name="path">The path of the file that should be used to write Log objects to</param>
        /// <param name="logLevels">The List of log levels that should be exported</param>
        /// <param name="enabled">True if exporting Log objects to a file should be enabled, otherwise false</param>
        /// <param name="encoding">The encoding that should be used to read and write data</param>
        public XmlFileAppender(string path, List<LogLevel> logLevels, bool enabled, Encoding encoding)
        {
            FilePath = path;
            LogLevels = logLevels;
            TextEncoding = encoding;
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
            return log != null && LogLevels.Contains(log.LogLevel);
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
                    using (StreamReader sr = new StreamReader(fs, TextEncoding))
                    {
                        readContents = sr.ReadToEnd();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // Ignored
            }
            
            LogRoot root;
            if (!string.IsNullOrEmpty(readContents))
            {
                try
                {
                    // Convert the XML into a LogRoot object
                    using (TextReader reader = new StringReader(readContents))
                    {
                        root = (LogRoot) _serializer.Deserialize(reader);
                    }
                }
                catch (Exception)
                {
                    if (ThrowErrors) throw;
                    return;
                }
            }
            else
            {
                root = new LogRoot();
            }

            // Add a Log to the LogRoot object
            root.Logs.Add(log);

            try
            {
                using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    string xml = ToXml(root);
                    using (StreamWriter sw = new StreamWriter(fs, TextEncoding))
                    {
                        sw.Write(xml);
                        sw.Flush();
                    }
                }
            }
            catch (Exception)
            {
                if (ThrowErrors) throw;
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
            if (!ValidLog(log)) return;
            await Task.Run(async () =>
            {
                string readContents = null;
                try
                {
                    using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Read the contents of the file
                        using (StreamReader sr = new StreamReader(fs, TextEncoding))
                        {
                            readContents = await sr.ReadToEndAsync();
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    // Ignored
                }
                
                LogRoot root;
                if (!string.IsNullOrEmpty(readContents))
                {
                    try
                    {
                        // Convert the XML into a LogRoot object
                        using (TextReader reader = new StringReader(readContents))
                        {
                            root = (LogRoot)_serializer.Deserialize(reader);
                        }
                    }
                    catch (Exception)
                    {
                        if (ThrowErrors) throw;
                        return;
                    }
                }
                else
                {
                    root = new LogRoot();
                }

                // Add a Log to the LogRoot object
                root.Logs.Add(log);

                try
                {
                    using (FileStream fs = System.IO.File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                    {
                        string xml = ToXml(root);
                        using (StreamWriter sw = new StreamWriter(fs, TextEncoding))
                        {
                            await sw.WriteAsync(xml);
                            await sw.FlushAsync();
                        }
                    }
                }
                catch (Exception)
                {
                    if (ThrowErrors) throw;
                }
            });
        }

        /// <summary>
        /// Convert a LogRoot object to an XML string
        /// </summary>
        /// <param name="root">The LogRoot object that should be converted into an XML string</param>
        /// <returns>The XML string value of the serialized LogRoot object</returns>
        private string ToXml(LogRoot root)
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
            return xml;
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
