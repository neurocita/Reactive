using System;

namespace Neurocita.Reactive
{
    public class ConsoleLog : Log
    {
        public override void Write(LogEntry logEntry)
        {
            Console.WriteLine(logEntry);
        }
    }
}
