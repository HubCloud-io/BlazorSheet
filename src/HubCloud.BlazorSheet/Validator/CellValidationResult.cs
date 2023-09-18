namespace HubCloud.BlazorSheet.Validator;

public class CellValidationResult
{
    public Guid SheetUid { get; set; }
    public string SheetName { get; set; }
    public string CellAddress { get; set; }
    public string Message { get; set; }
}