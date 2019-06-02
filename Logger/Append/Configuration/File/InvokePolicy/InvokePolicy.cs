using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Configuration.File.InvokePolicy
{
    /// <summary>
    /// Abstract class that contains the logic behind an invoke policy
    /// </summary>
    [XmlInclude(typeof(FileAgePolicy))]
    [XmlInclude(typeof(FileSizePolicy))]
    public abstract class InvokePolicy
    {
        public abstract bool ShouldInvoke(string filePath);
    }
}