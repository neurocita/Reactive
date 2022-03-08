using System;

namespace Neurocita.Reactive.Logging
{
    public sealed class NullLogger : ILogger
    {
        public static ILogger Instance => new NullLogger();

        private NullLogger()
        { }

        public bool IsFatalEnabled => false;
        public bool IsErrorEnabled => false;
        public bool IsWarnEnabled => false;
        public bool IsInfoEnabled => false;
        public bool IsDebugEnabled => false;
        public bool IsTraceEnabled => false;

        public void Debug(string message)
        { }

        public void Debug(Func<string> messageFactory)
        { }

        public void Debug(string message, Exception exception)
        { }

        public void Debug(string format, params object[] args)
        { }

        public void Debug(Exception exception, string format, params object[] args)
        { }

        public void Debug(IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Debug(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Error(string message)
        { }

        public void Error(Func<string> messageFactory)
        { }

        public void Error(string message, Exception exception)
        { }

        public void Error(string format, params object[] args)
        { }

        public void Error(Exception exception, string format, params object[] args)
        { }

        public void Error(IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Error(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Fatal(string message)
        { }

        public void Fatal(Func<string> messageFactory)
        { }

        public void Fatal(string message, Exception exception)
        { }

        public void Fatal(string format, params object[] args)
        { }

        public void Fatal(Exception exception, string format, params object[] args)
        { }

        public void Fatal(IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Fatal(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Info(string message)
        { }

        public void Info(Func<string> messageFactory)
        { }

        public void Info(string message, Exception exception)
        { }

        public void Info(string format, params object[] args)
        { }

        public void Info(Exception exception, string format, params object[] args)
        { }

        public void Info(IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Info(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Trace(string message)
        { }

        public void Trace(Func<string> messageFactory)
        { }

        public void Trace(string message, Exception exception)
        { }

        public void Trace(string format, params object[] args)
        { }

        public void Trace(Exception exception, string format, params object[] args)
        { }

        public void Trace(IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Trace(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Warn(string message)
        { }

        public void Warn(Func<string> messageFactory)
        { }

        public void Warn(string message, Exception exception)
        { }

        public void Warn(string format, params object[] args)
        { }

        public void Warn(Exception exception, string format, params object[] args)
        { }

        public void Warn(IFormatProvider formatProvider, string format, params object[] args)
        { }

        public void Warn(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        { }
    }
}
