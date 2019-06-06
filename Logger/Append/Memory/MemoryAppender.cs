using System.Collections.Generic;
using CodeDead.Logger.Logging;

namespace CodeDead.Logger.Append.Memory
{
    /// <summary>
    /// Abstract class that contains the logic behind memory appenders
    /// </summary>
    public abstract class MemoryAppender : LogAppender
    {
        #region Properties
        /// <summary>
        /// Gets or sets the list of Log objects
        /// </summary>
        public List<Log> LogList { get; set; }
        #endregion

        /// <summary>
        /// Initialize a new MemoryAppender
        /// </summary>
        protected MemoryAppender()
        {
            LogList = new List<Log>();
        }
    }
}
