using System;
using System.Collections.Generic;
using System.Text;

namespace Neurocita.Reactive.Logging
{
    internal class Log : LogBase
    {
        internal Log(object instance)
            : base(instance)
        {
        }

        internal Log(object instance, LogLevel logLevel)
            : base(instance, logLevel)
        {
        }

        protected override void Write(LogLevel loglevel, string message)
        {
            throw new NotImplementedException();
        }

        protected override void Write(LogLevel loglevel, Func<string> messageFactory)
        {
            Write(loglevel, messageFactory.Invoke);
        }

        protected override void Write(LogLevel loglevel, string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        protected override void Write(LogLevel loglevel, string format, params object[] args)
        {
            Write(loglevel, string.Format(format, args));
        }

        protected override void Write(LogLevel loglevel, Exception exception, string format, params object[] args)
        {
            Write(loglevel, string.Format(format, args), exception);
        }

        protected override void Write(LogLevel loglevel, IFormatProvider formatProvider, string format, params object[] args)
        {
            Write(loglevel, string.Format(formatProvider, format, args));
        }

        protected override void Write(LogLevel loglevel, Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            Write(loglevel, string.Format(formatProvider, format, args), exception);
        }
    }
}
