using System;

namespace Emuses.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string _componentName;

        public LoggerService(string componentName)
        {
            _componentName = componentName;
        }

        public Log PrintLog(string text)
        {
            var log = new Log(_componentName, text);

            Console.WriteLine(log.Head);
            Console.WriteLine(log.Value);

            return log;
        }

        public Log WriteLog(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
