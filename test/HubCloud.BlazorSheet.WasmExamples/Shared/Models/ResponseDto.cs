namespace HubCloud.BlazorSheet.WasmExamples.Shared.Models;

public class ResponseDto<T>
{
    public int HttpCode { get; set; } = 200;
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = "OK";
    public T Data { get; set; } = default(T);

    public bool IsOK => StatusCode == 200;
    
    public void SetStatus(int status, string message)
    {
        StatusCode = status;
        Message = message;
    }

    public override string ToString()
    {
        return $"{StatusCode}:{Message}";
    }
}