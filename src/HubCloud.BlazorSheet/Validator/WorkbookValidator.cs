using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Validator;

public class WorkbookValidator : IWorkbookValidator
{
    public IEnumerable<CellValidationResult> Validate(Sheet sheet)
    {
        if (sheet is null)
            return Enumerable.Empty<CellValidationResult>();

        var resultList = new List<CellValidationResult>();

        var requiredEditCells = sheet.Cells.Where(x => x.EditSettingsUid != null &&
                                                       sheet.EditSettings.FirstOrDefault(s => s.Uid == x.EditSettingsUid)?.Required == true);
        
        foreach (var editCell in requiredEditCells)
        {
            var isEmpty = ExpressoFunctions.FunctionLibrary.IsEmptyFunction.Eval(editCell.Value);
            if (isEmpty)
                resultList.Add(new CellValidationResult
                {
                    SheetUid = sheet.Uid,
                    SheetName = sheet.Name,
                    CellAddress = sheet.CellAddress(editCell).ToString()
                });
        }

        return resultList;
    }

    public IEnumerable<CellValidationResult> Validate(Workbook workbook)
    {
        if (workbook?.Sheets is null)
            return Enumerable.Empty<CellValidationResult>();
        
        var resultList = new List<CellValidationResult>();
        foreach (var sheet in workbook.Sheets)
        {
            var sheetValidationResults = Validate(sheet)?.ToArray();
            if (sheetValidationResults?.Any() == true)
                resultList.AddRange(sheetValidationResults);
        }

        return resultList;
    }
}