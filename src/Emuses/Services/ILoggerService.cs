namespace Emuses.Services
{
    public interface ILoggerService
    {
        Log PrintLog(string text);

        Log WriteLog(string text);
    }
}
