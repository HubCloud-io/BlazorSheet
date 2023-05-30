using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCell
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public Guid RowUid { get; set; }
        public Guid ColumnUid { get; set; }
        public Guid? StyleUid { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public object Value { get; set; }

        public SheetCell ConcreteClone()
        {
            var clone = new SheetCell()
            {
                Uid = this.Uid,
                RowUid = this.RowUid,
                ColumnUid = this.ColumnUid,
                Name = this.Name,
                
                Text = this.Text,
                Value = this.Value
                
            };

            return clone;
        }

        public object Clone()
        {
            return ConcreteClone();
        }

        public void ApplyFormat(string format)
        {
            if (Value == null)
                return;

            var text = Value as string;

            if (string.IsNullOrEmpty(text) ||
                string.IsNullOrEmpty(format))
                return;

            if (DateTime.TryParse(text, out var date))
                Text = date.ToString(format);
            else if (decimal.TryParse(text, out var decimalValue))
                Text = decimalValue.ToString(format);
        }
    }
}