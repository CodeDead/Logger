using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append
{
    /// <summary>
    /// Interface that contains methods that should be implemented by inherited members
    /// </summary>
    public interface ILogAppender
    {
        void ExportLog(Log log);
    }
}
