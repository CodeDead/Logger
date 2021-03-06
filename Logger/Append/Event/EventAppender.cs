﻿using System.Xml.Serialization;

namespace CodeDead.Logger.Append.Event
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract class that can handle exporting Log objects to the Windows Event Viewer.
    /// Inherit this class to make your own event appending logic
    /// </summary>
    [XmlInclude(typeof(WindowsEventAppender))]
    public abstract class EventAppender : LogAppender
    {
    }
}
