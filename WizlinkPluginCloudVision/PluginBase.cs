using System;

namespace WizlinkPluginCloudVision
{
    public interface IPluginBase
    {
        event EventHandler<LoggerEventArgs> LoggerHandler;
        void Log(string message, LogLevel logLevel, Exception exception = null);
    }

    /// <summary>
    /// The PluginBase class, among other things, enables direct transfer of logs to Wizlink.
    /// </summary>
    public abstract class PluginBase : IPluginBase
    {
        public event EventHandler<LoggerEventArgs> LoggerHandler;

        public virtual void Log(string message, LogLevel logLevel = LogLevel.Information, Exception exception = null)
        {
            LoggerEventArgs args = new LoggerEventArgs
            {
                Message = message,
                LogLevel = logLevel,
                Exception = exception
            };

            EventHandler<LoggerEventArgs> handler = LoggerHandler;
            handler?.Invoke(this, args);
        }
    }

    [Hide]
    public class LoggerEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Enum LogLevel { get; set; }
        public Exception Exception { get; set; }
    }

    /// <summary>
    /// The Hide attribute provides the ability to hide a class or method in the Plugin activity.
    /// </summary>
    [Hide]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Hide : Attribute { }

    /// <summary>
    /// The WizlinkVisible attribute restricts the visibility of classes in the Plugin activity to only those explicitly marked with the attribute.
    /// </summary>
    [Hide]
    [AttributeUsage(AttributeTargets.Class)]
    public class WizlinkVisible : Attribute { }

    /// <summary>
    /// The TupleDescription attribute allows to describe each element of Tuple returned by Plugin method.
    /// </summary>
    [Hide]
    [AttributeUsage(AttributeTargets.ReturnValue)]
    public class TupleDescription : Attribute
    {
        public string[] ElementsDescriptions { get; }

        public TupleDescription(string[] elementsDescriptions)
        {
            ElementsDescriptions = elementsDescriptions;
        }
    }

    public enum LogLevel
    {
        Debug = 1, Information = 2, Warning = 3, Error = 4
    }
}
