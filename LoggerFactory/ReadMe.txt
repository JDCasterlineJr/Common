#Settings to go in the app.config
<configSections>
<section name="LoggerFactoryConfiguration" type="Common.LoggerFactory.LoggerFactoryConfigurationSection, DatabaseFactory, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
</configSections>

//Namespace qualified name  
<LoggerFactoryConfiguration Name="Common.LoggerFactory.Loggers.NLogLogger" />
//Assembly qualified name
<LoggerFactoryConfiguration Name="Common.LoggerFactory.Loggers.NLogLogger, Common.LoggerFactory" />
#Settings to go in the app.config

LoggerFactoryConfiguration.Name = qualified name of the concrete implementation LoggerFactory.Logger to use

Example
var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
var logger = new LoggerFactory().CreateInstance(fullName);
try
{
	throw new Exception("Test exception");
}
catch (Exception ex)
{
	logger.Log(new LogItem() { LogLevel = LogLevel.Error, Message = "Test message", Exception = ex });
}