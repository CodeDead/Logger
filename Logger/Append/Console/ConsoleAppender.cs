using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Console
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract class that can be used to handle exporting Log objects to the console
    /// Inherit this class to implement your own console appending logic
    /// </summary>
    [XmlInclude(typeof(DefaultConsoleAppender))]
    public abstract class ConsoleAppender : LogAppender
    {

    }
}
