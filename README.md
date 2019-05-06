# CodeDead.Logger
This repository contains a simple, yet extensible logging library for .NET Framework applications. Some of the capabilities included in this library are the ability to write logs to any number of different
files at the same time, writing your own appending logic and even writing logs asynchonously. It is highly recommended to look at the code samples to get a general idea of the workflow.

## NuGet
This library is available as a NuGet package:
#TODO

## Dependencies
* [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

## Example usage
```
// Logger objects can be generated using the LogFactory using either a key:
// Logger logger = LogFactory.GenerateLogger("MAIN");
// Or a default Logger object can be retrieved
Logger logger = LogFactory.GenerateLogger();
string defaultName = logger.Name;

// This is the default console appender but you can implement the LogAppender or ConsoleAppender to write your own logic
DefaultConsoleAppender consoleWriter = new DefaultConsoleAppender();
// This is the default file appender but you can implement the LogAppender or FileAppender to write your own logic
DefaultFileAppender fileAppender = new DefaultFileAppender(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\exampleLog.log");
// You can have as many appenders as your system allows
logger.LogManager.AddLogAppender(consoleWriter);
logger.LogManager.AddLogAppender(fileAppender);

// Defaults
logger.Trace("Hello trace!");
logger.Debug("Hello debug!");
logger.Info("Hello info!");
logger.Warn("Hello warn!");
logger.Error("Hello error!");
```

## Appenders
Appenders are used for 'appending' logs to a specific output. There are some implementations of those available to you already. They are:

| Class | Description |
| --- | --- |
| `DefaultConsoleAppender` | Default implementation of the `ConsoleAppender` that can be used for writing logs to the console using a `Format` that can be customized |
| `DefaultFileAppender` | Default implementation of the `FileAppender` that can be used for writing logs to a file using a `Format` that can be customized |
| `JsonFileAppender` | A spin of the `FileAppender` that can write logs as a JSON file |
| `XmlFileAppender` | A spin of the `FileAppender` that can write logs as an XML file |
| `CsvFileAppender` | A spin of the `FileAppender` that can write logs in CSV format. You can adjust the delimiter using the `Delimiter` property |
| `WindowsEventAppender` | A spin of the `EventAppender` that can write logs to the Windows Event Log |

# TODO
- [X] Allow loggers to be read/exported to/from a file (JSON/XML)
- [X] Write the code for the DefaultFileAppender
- [X] Write the code for the XmlFileAppender
- [X] Write the code for the JsonFileAppender
- [X] Write the code for the CsvFileAppender
- [ ] Write the code for the RollingFileAppender
- [X] Write the code for the WindowsEventAppender
- [ ] Write documentation
- [X] Write contribution guidelines
- [ ] Publish on NuGet

# About
This library is maintained by CodeDead. You can find more about us using the following links:
* [Website](https://codedead.com)
* [Twitter](https://twitter.com/C0DEDEAD)
* [Facebook](https://facebook.com/deadlinecodedead)
* [Reddit](https://reddit.com/r/CodeDead/)

We would also like to thank JetBrains for the open source license that they granted us to work with wonderful tools such as [Rider](https://jetbrains.com/rider) and [Resharper](https://jetbrains.com/resharper).
