namespace WebAPIServer.Services
{
    public interface ILoggerService
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
    }
    public class LoggerService : ILoggerService
    {
        private readonly LogLevel _minLevel;
        private readonly TextWriter _writer;

        public enum LogLevel
        {
            Trace,
            Debug,
            Information,
            Warning,
            Error,
            Fatal
        }

        public LoggerService(LogLevel minLevel, TextWriter writer = null)
        {
            _minLevel = minLevel;
            _writer = writer ?? Console.Out; // Use console by default
        }

        private void Write(LogLevel level, string message)
        {
            if (level >= _minLevel)
            {
                _writer.WriteLine($"{DateTime.Now} [{level}] - {message}");
            }
        }

        public void Trace(string message) => Write(LogLevel.Trace, message);
        public void Debug(string message) => Write(LogLevel.Debug, message);
        public void Info(string message) => Write(LogLevel.Information, message);
        public void Warning(string message) => Write(LogLevel.Warning, message);
        public void Error(string message) => Write(LogLevel.Error, message);
        public void Fatal(string message) => Write(LogLevel.Fatal, message);
    }
}
