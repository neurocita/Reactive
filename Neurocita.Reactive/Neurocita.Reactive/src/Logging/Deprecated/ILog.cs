using System;

namespace Neurocita.Reactive.Logging
{
    public interface ILog
    {
		bool IsFatalEnabled { get; }
		bool IsErrorEnabled { get; }
		bool IsWarnEnabled { get; }
		bool IsInfoEnabled { get; }
		bool IsDebugEnabled { get; }
		bool IsTraceEnabled { get; }
		void Fatal(string message);
		void Fatal(Func<string> messageFactory);
		void Fatal(string message, Exception exception);
		void Fatal(string format, params object[] args);
		void Fatal(Exception exception, string format, params object[] args);
		void Fatal(IFormatProvider formatProvider, string format, params object[] args);
		void Fatal(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void Error(string message);
		void Error(Func<string> messageFactory);
		void Error(string message, Exception exception);
		void Error(string format, params object[] args);
		void Error(Exception exception, string format, params object[] args);
		void Error(IFormatProvider formatProvider, string format, params object[] args);
		void Error(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void Warn(string message);
		void Warn(Func<string> messageFactory);
		void Warn(string message, Exception exception);
		void Warn(string format, params object[] args);
		void Warn(Exception exception, string format, params object[] args);
		void Warn(IFormatProvider formatProvider, string format, params object[] args);
		void Warn(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void Info(string message);
		void Info(Func<string> messageFactory);
		void Info(string message, Exception exception);
		void Info(string format, params object[] args);
		void Info(Exception exception, string format, params object[] args);
		void Info(IFormatProvider formatProvider, string format, params object[] args);
		void Info(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void Debug(string message);
		void Debug(Func<string> messageFactory);
		void Debug(string message, Exception exception);
		void Debug(string format, params object[] args);
		void Debug(Exception exception, string format, params object[] args);
		void Debug(IFormatProvider formatProvider, string format, params object[] args);
		void Debug(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void Trace(string message);
		void Trace(Func<string> messageFactory);
		void Trace(string message, Exception exception);
		void Trace(string format, params object[] args);
		void Trace(Exception exception, string format, params object[] args);
		void Trace(IFormatProvider formatProvider, string format, params object[] args);
		void Trace(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
	}
}
