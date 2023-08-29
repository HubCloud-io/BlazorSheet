using HubCloud.BlazorSheet.Core.Models;

namespace HubCloud.BlazorSheet.Infrastructure;

public class SheetArrowNavigationHelper
{
    public static SheetCell ArrowUp(Sheet sheet, SheetCell currentCell)
    {
        if (currentCell == null)
            return null;

        var address = sheet.CellAddress(currentCell);

        var nextRow = address.Row - 1;

        if (nextRow <= 0)
            return null;

        var nextCell = sheet.GetCell(nextRow, address.Column);

        return nextCell;
    }
    
    public static SheetCell ArrowDown(Sheet sheet, SheetCell currentCell)
    {
        if (currentCell == null)
            return null;

        var address = sheet.CellAddress(currentCell);

        var nextRow = address.Row + 1;

        if (nextRow > sheet.RowsCount)
            return null;

        var nextCell = sheet.GetCell(nextRow, address.Column);

        return nextCell;
    }
    
    public static SheetCell ArrowLeft(Sheet sheet, SheetCell currentCell)
    {
        if (currentCell == null)
            return null;

        var address = sheet.CellAddress(currentCell);

        var nextColumn = address.Column - 1;

        if (nextColumn <= 0)
            return null;

        var nextCell = sheet.GetCell(address.Row, nextColumn);

        return nextCell;
    }
    
    public static SheetCell ArrowRight(Sheet sheet, SheetCell currentCell)
    {
        if (currentCell == null)
            return null;

        var address = sheet.CellAddress(currentCell);

        var nextColumn = address.Column + 1;

        if (nextColumn > sheet.ColumnsCount)
            return null;

        var nextCell = sheet.GetCell(address.Row, nextColumn);

        return nextCell;
    }
}