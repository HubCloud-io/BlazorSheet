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

    // public static SheetCell NextEditingCellDown(Sheet sheet, SheetCell currentCell)
    // {
    //     if (sheet == null)
    //         return null;
    //
    //     if (currentCell == null)
    //         return null;
    //
    //     var address = sheet.CellAddress(currentCell);
    //
    //     var currentRow = address.Row;
    //     var currentColumn = address.Column;
    //
    //     var flagContinue = true;
    //
    //     SheetCell nextCell = null;
    //     
    //     while (flagContinue)
    //     {
    //         currentColumn += 1;
    //         if (currentColumn > sheet.ColumnsCount)
    //         {
    //             currentRow += 1;
    //             currentColumn = 1;
    //         }
    //
    //         if (currentRow > sheet.RowsCount)
    //         {
    //             flagContinue = false;
    //             break;
    //         }
    //
    //         nextCell = sheet.GetCell(currentRow, currentColumn);
    //         if (nextCell.EditSettingsUid.HasValue)
    //         {
    //             flagContinue = false;
    //             break;
    //         }
    //         else
    //         {
    //             flagContinue = true;
    //             nextCell = null;
    //         }
    //     }
    //
    //     return nextCell;
    //
    // }
    
    public static SheetCell NextEditingCellRight(Sheet sheet, SheetCell currentCell)
    {
        // Return null if either sheet or currentCell is null
        if (sheet == null || currentCell == null)
            return null;

        var address = sheet.CellAddress(currentCell);
        var currentRow = address.Row;
        var currentColumn = address.Column;

        // Keep searching for the next editing cell until the end of the sheet is reached
        while (true)
        {
            currentColumn += 1;
            if (currentColumn > sheet.ColumnsCount)
            {
                currentRow += 1;
                currentColumn = 1;
            }

            // If we've passed the last row, exit loop
            if (currentRow > sheet.RowsCount)
                return null;

            var nextCell = sheet.GetCell(currentRow, currentColumn);
        
            // If this cell has EditSettingsUid, it's the next editing cell we're looking for
            if (nextCell.EditSettingsUid.HasValue)
                return nextCell;
        }
    }
    
    public static SheetCell NextEditingCellDown(Sheet sheet, SheetCell currentCell)
    {
        // Return null if either sheet or currentCell is null
        if (sheet == null || currentCell == null)
            return null;

        var address = sheet.CellAddress(currentCell);
        var currentRow = address.Row;
        var currentColumn = address.Column;

        // Keep searching for the next editing cell until the end of the sheet is reached
        while (true)
        {
            currentRow += 1;
            if (currentRow > sheet.RowsCount)
            {
                currentRow = 1;
                currentColumn += 1;
            }

            // If we've passed the last row, exit loop
            if (currentColumn > sheet.ColumnsCount)
                return null;

            var nextCell = sheet.GetCell(currentRow, currentColumn);
        
            // If this cell has EditSettingsUid, it's the next editing cell we're looking for
            if (nextCell.EditSettingsUid.HasValue)
                return nextCell;
        }
    }

}