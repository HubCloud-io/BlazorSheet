using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class Workbook
    {
        private List<Sheet> _sheets = new List<Sheet>();    

        public Guid Uid { get; set; } = Guid.NewGuid();

        public Sheet FirstSheet => _sheets[0];

        public Workbook(WorkbookSettings settings)
        {
            foreach(var sheetSettings in settings.Sheets)
            {
                var newSheet = new Sheet(sheetSettings);
                AddSheet(newSheet);
            }
        }

        public void AddSheet(Sheet sheet)
        {
            if(_sheets.Any(x=>x.Uid == sheet.Uid))
            {
                return;
            }

            if(_sheets.Any(x=>x.Name.Equals(sheet.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            _sheets.Add(sheet);
        }

        public WorkbookSettings ToSettings()
        {
            var settings = new WorkbookSettings();

            foreach(var sheet in _sheets)
            {
                var sheetSettings = sheet.ToSettings();
                settings.Sheets.Add(sheetSettings);
            }

            return settings;
        }
    }
}
