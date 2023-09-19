using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Validator;

public interface IWorkbookValidator
{
    IEnumerable<CellValidationResult> Validate(Sheet sheet);
    IEnumerable<CellValidationResult> Validate(Workbook workbook);
}