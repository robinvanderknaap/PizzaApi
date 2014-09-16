using System.Diagnostics;

namespace Api.Infrastructure.Services
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}