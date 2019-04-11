# CodeDead.Logger
This repository contains a simple, yet exstensible logging library for .NET Framework applications. Some of the capabilities included in this library are the ability to write logs to any number of different
files at the same time, writing your own appending logic and even writing logs asynchonously. It is highly recommended to look at the code samples to get a general idea of the workflow.

## NuGet
This library is available as a NuGet package:
#TODO

## Dependencies
* [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)

## Example usage

```
// Logger objects can be generated using the LogFactory using either a key:
// Logger logger = LogFactory.GenerateLogger("MAIN");
// Or a default Logger object can be retrieved
Logger logger = LogFactory.GenerateLogger();

// The list of log levels that would have to be appended
List<LogLevel> logLevels = new List<LogLevel>
{
	LogLevel.Trace,
	LogLevel.Debug,
	LogLevel.Info,
	LogLevel.Warning,
	LogLevel.Error
};

// This is the default console appender, but you can implement the LogAppender or ConsoleAppender to write your own logic
DefaultConsoleAppender consoleWriter = new DefaultConsoleAppender(logLevels, true);
// You can have as many appenders as your system allows
logger.GetLogManager().AddLogAppender(consoleWriter);

// Defaults
logger.Trace("Hello trace!");
logger.Debug("Hello debug!");
logger.Info("Hello info!");
logger.Warn("Hello warn!");
logger.Error("Hello error!");
```

# TODO
- [ ] Write the code for the DefaultFileAppender
- [ ] Allow Settings to be read/exported to/from a file (JSON/XML)
- [ ] Publish on NuGet

# About
This library is maintained by @CodeDead. You can find more about us using the following links
* [Website](https://codedead.com)
* [Twitter](https://twitter.com/C0DEDEAD)
* [Facebook](https://facebook.com/deadlinecodedead)
* [Reddit](https://reddit.com/r/CodeDead/)