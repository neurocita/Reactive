using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive.Logging
{
    public abstract class LogBase : ILog
    {
        private readonly object instance;
        private readonly LogLevel logLevel = LogLevel.Off;

        protected LogBase(object instance)
        {
            this.instance = instance;
        }

        protected LogBase(object instance, LogLevel logLevel)
            : this(instance)
        {
            this.logLevel = logLevel;
        }

        public bool IsFatalEnabled => logLevel >= LogLevel.Fatal;

        public bool IsErrorEnabled => logLevel >= LogLevel.Error;

        public bool IsWarnEnabled => logLevel >= LogLevel.Warn;

        public bool IsInfoEnabled => logLevel >= LogLevel.Info;

        public bool IsDebugEnabled => logLevel >= LogLevel.Debug;

        public bool IsTraceEnabled => logLevel >= LogLevel.Trace;

        public void Fatal(string message)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, message);
        }

        public void Fatal(Func<string> messageFactory)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, messageFactory);
        }

        public void Fatal(string message, Exception exception)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, message, exception);
        }

        public void Fatal(string format, params object[] args)
        {
            if (!IsFatalEnabled)
                Write(LogLevel.Fatal, format, args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            if (!IsFatalEnabled)
                Write(LogLevel.Fatal, exception, format, args);
        }

        public void Fatal(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (!IsFatalEnabled)
                Write(LogLevel.Fatal, formatProvider, format, args);
        }

        public void Fatal(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
                Write(LogLevel.Fatal, exception, formatProvider, format, args);
        }

        public void Error(string message)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Fatal, message);
        }

        public void Error(Func<string> messageFactory)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, messageFactory);
        }

        public void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, message, exception);
        }

        public void Error(string format, params object[] args)
        {
            if (!IsErrorEnabled)
                Write(LogLevel.Error, format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            if (!IsErrorEnabled)
                Write(LogLevel.Error, exception, format, args);
        }

        public void Error(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (!IsErrorEnabled)
                Write(LogLevel.Error, formatProvider, format, args);
        }

        public void Error(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
                Write(LogLevel.Error, exception, formatProvider, format, args);
        }

        public void Warn(string message)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, message);
        }

        public void Warn(Func<string> messageFactory)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, messageFactory);
        }

        public void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, message, exception);
        }

        public void Warn(string format, params object[] args)
        {
            if (!IsWarnEnabled)
                Write(LogLevel.Warn, format, args);
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            if (!IsWarnEnabled)
                Write(LogLevel.Warn, exception, format, args);
        }

        public void Warn(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (!IsWarnEnabled)
                Write(LogLevel.Warn, formatProvider, format, args);
        }

        public void Warn(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                Write(LogLevel.Warn, exception, formatProvider, format, args);
        }

        public void Info(string message)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, message);
        }

        public void Info(Func<string> messageFactory)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, messageFactory);
        }

        public void Info(string message, Exception exception)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, message, exception);
        }

        public void Info(string format, params object[] args)
        {
            if (!IsInfoEnabled)
                Write(LogLevel.Info, format, args);
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            if (!IsInfoEnabled)
                Write(LogLevel.Info, exception, format, args);
        }

        public void Info(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (!IsInfoEnabled)
                Write(LogLevel.Info, formatProvider, format, args);
        }

        public void Info(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
                Write(LogLevel.Info, exception, formatProvider, format, args);
        }

        public void Debug(string message)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, message);
        }

        public void Debug(Func<string> messageFactory)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, messageFactory);
        }

        public void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, message, exception);
        }

        public void Debug(string format, params object[] args)
        {
            if (!IsDebugEnabled)
                Write(LogLevel.Debug, format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            if (!IsDebugEnabled)
                Write(LogLevel.Debug, exception, format, args);
        }

        public void Debug(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (!IsDebugEnabled)
                Write(LogLevel.Debug, formatProvider, format, args);
        }

        public void Debug(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
                Write(LogLevel.Debug, exception, formatProvider, format, args);
        }

        public void Trace(string message)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, message);
        }

        public void Trace(Func<string> messageFactory)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, messageFactory);
        }

        public void Trace(string message, Exception exception)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, message, exception);
        }

        public void Trace(string format, params object[] args)
        {
            if (!IsTraceEnabled)
                Write(LogLevel.Trace, format, args);
        }

        public void Trace(Exception exception, string format, params object[] args)
        {
            if (!IsTraceEnabled)
                Write(LogLevel.Trace, exception, format, args);
        }

        public void Trace(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (!IsTraceEnabled)
                Write(LogLevel.Trace, formatProvider, format, args);
        }

        public void Trace(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
                Write(LogLevel.Trace, exception, formatProvider, format, args);
        }

        abstract protected void Write(LogLevel loglevel, string message);

        abstract protected void Write(LogLevel loglevel, Func<string> messageFactory);

        abstract protected void Write(LogLevel loglevel, string message, Exception exception);

        abstract protected void Write(LogLevel loglevel, string format, params object[] args);

        abstract protected void Write(LogLevel loglevel, Exception exception, string format, params object[] args);

        abstract protected void Write(LogLevel loglevel, IFormatProvider formatProvider, string format, params object[] args);

        abstract protected void Write(LogLevel loglevel, Exception exception, IFormatProvider formatProvider, string format, params object[] args);
    }
}
