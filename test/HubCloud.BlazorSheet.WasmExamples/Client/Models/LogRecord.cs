namespace Company.WebApplication1.Models;

public class LogRecord
{
    public DateTime Period { get; set; } = DateTime.Now;
    public string Message { get; set; }

    public LogRecord()
    {
    }

    public LogRecord(string message)
    {
        Message = message;
    }
}