using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CodeDead.Logger.Configuration
{
    /// <summary>
    /// Sealed class for handling loading and saving configuration data
    /// </summary>
    internal static class ConfigurationManager
    {
        /// <summary>
        /// Load a LoggerRoot object using the data contained in a configuration file
        /// </summary>
        /// <param name="filePath">The path of the file that contains the LoggerRoot configuration data</param>
        /// <returns>The LoggerRoot object that was built using the configuration file</returns>
        internal static LoggerRoot LoadLoggerRoot(string filePath)
        {
            string data = ReadFile(filePath);
            return LoadConfiguration(data);
        }

        /// <summary>
        /// Load a LoggerRoot object using the data contained in a configuration file
        /// </summary>
        /// <param name="filePath">The path of the file that contains the configuration data</param>
        /// <returns>The LoggerRoot object that was built using the configuration file</returns>
        internal static async Task<LoggerRoot> LoadLoggerRootAsync(string filePath)
        {
            string data = await ReadFileAsync(filePath);
            return await Task.Run(() => LoadConfiguration(data));
        }

        /// <summary>
        /// Load a LoggerRoot object using a given byte array
        /// </summary>
        /// <param name="bytes">The byte array that contains the configuration data</param>
        /// <returns>The LoggerRoot object that was built using the configuration data</returns>
        internal static LoggerRoot LoadLoggerRoot(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentException(nameof(bytes));

            string data = Convert.ToString(bytes);
            return LoadConfiguration(data);
        }

        /// <summary>
        /// Load a LoggerRoot object using a given byte array asynchronously
        /// </summary>
        /// <param name="bytes">The byte array that contains the configuration data</param>
        /// <returns>The LoggerRoot object that was built using the configuration data</returns>
        internal static async Task<LoggerRoot> LoadLoggerRootAsync(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentException(nameof(bytes));

            return await Task.Run(() =>
            {
                string data = Convert.ToString(bytes);
                return LoadConfiguration(data);
            });
        }

        /// <summary>
        /// Save a LoggerRoot object to the filesystem
        /// </summary>
        /// <param name="filePath">The path where the LoggerRoot object should be stored</param>
        /// <param name="root">The LoggerRoot object that should be saved</param>
        /// <param name="saveFormat">The format in which the LoggerRoot object should be stored</param>
        internal static void SaveLoggerRoot(string filePath, LoggerRoot root, SaveFormats saveFormat)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException(nameof(filePath));
            if (root == null) throw new ArgumentNullException(nameof(root));

            switch (saveFormat)
            {
                case SaveFormats.Xml:
                    SaveLoggerRootXml(filePath, root);
                    break;
                case SaveFormats.Json:
                    SaveLoggerRootJson(filePath, root);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveFormat), saveFormat, null);
            }
        }

        /// <summary>
        /// Save a LoggerRoot object to the filesystem asynchronously
        /// </summary>
        /// <param name="filePath">The path where the LoggerRoot object should be stored</param>
        /// <param name="root">The LoggerRoot object that should be saved</param>
        /// <param name="saveFormat">The format in which the LoggerRoot object should be stored</param>
        /// <returns>The Task object that is associated with this asynchronous method</returns>
        internal static async Task SaveLoggerRootAsync(string filePath, LoggerRoot root, SaveFormats saveFormat)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException(nameof(filePath));
            if (root == null) throw new ArgumentNullException(nameof(root));

            switch (saveFormat)
            {
                case SaveFormats.Xml:
                    await Task.Run(() => SaveLoggerRootXml(filePath, root));
                    break;
                case SaveFormats.Json:
                    await Task.Run(() => SaveLoggerRootJson(filePath, root));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveFormat), saveFormat, null);
            }
        }

        /// <summary>
        /// Save a LoggerRoot object to the filesystem in XML format
        /// </summary>
        /// <param name="filePath">The path where the LoggerRoot object should be stored</param>
        /// <param name="root">The LoggerRoot object that should be saved</param>
        private static void SaveLoggerRootXml(string filePath, LoggerRoot root)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(LoggerRoot));
            string xml;

            using (StringWriter sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, root);
                    xml = sww.ToString();
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(xml);
            }
        }

        /// <summary>
        /// Save a LoggerRoot object to the filesystem in JSON format
        /// </summary>
        /// <param name="filePath">The path where the LoggerRoot object should be stored</param>
        /// <param name="root">The LoggerRoot object that should be saved</param>
        private static void SaveLoggerRootJson(string filePath, LoggerRoot root)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            string json = serializer.Serialize(root);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(json);
            }
        }

        /// <summary>
        /// Load the configuration of a LoggerRoot object
        /// </summary>
        /// <param name="data">The data that contains the configuration of the LoggerRoot object</param>
        /// <returns>The LoggerRoot object that was built using the configuration data</returns>
        private static LoggerRoot LoadConfiguration(string data)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentException(nameof(data));
            bool isJson = IsJson(data);
            bool isXml = IsXml(data);

            if (!isJson && !isXml) throw new ArgumentException(nameof(data));

            if (isJson)
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer(new SimpleTypeResolver());
                return jsSerializer.Deserialize<LoggerRoot>(data);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoggerRoot));
            using (TextReader reader = new StringReader(data))
            {
                return (LoggerRoot)xmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Check if an input might be a JSON
        /// </summary>
        /// <param name="input">The input that should be validated</param>
        /// <returns>True if the input might be JSON, otherwise false</returns>
        private static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        /// <summary>
        /// Check if an input might be XML
        /// </summary>
        /// <param name="data">The input that should be validated</param>
        /// <returns>True if the input might be XML, otherwise false</returns>
        private static bool IsXml(string data)
        {
            return !string.IsNullOrEmpty(data) && data.TrimStart().StartsWith("<");
        }

        /// <summary>
        /// Read the contents of a file into a string
        /// </summary>
        /// <param name="filePath">The path of the file that should be read</param>
        /// <returns>The contents of a file, if applicable</returns>
        private static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            string readContents;
            using (StreamReader streamReader = new StreamReader(filePath, true))
            {
                readContents = streamReader.ReadToEnd();
            }
            return readContents;
        }

        /// <summary>
        /// Asynchronously read the contents of a file into a string
        /// </summary>
        /// <param name="filePath">The path of the file that should be read</param>
        /// <returns>The contents of a file, if applicable</returns>
        private static async Task<string> ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            string readContents;
            using (StreamReader streamReader = new StreamReader(filePath, true))
            {
                readContents = await streamReader.ReadToEndAsync();
            }
            return readContents;
        }
    }
}
