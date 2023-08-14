using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetColumn: ICloneable
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid ParentUid { get; set; }
        public bool IsGroup { get; set; }
        public bool IsOpen { get; set; }
        public bool IsHidden { get; set; }
        public bool IsCollapsed { get; set; }
        public bool IsAddRemoveAllowed { get; set; }
        public double WidthValue { get; set; } = 100;
        public string Width => (WidthValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)) + "px";
        public string Key => string.IsNullOrWhiteSpace(Name) ? Uid.ToString() : Name;

        public SheetColumn()
        {

        }
        
        public SheetColumn(double widthValue)
        {
            WidthValue = widthValue;
        }
        
        public SheetColumn ConcreteClone()
        {
            var clone = new SheetColumn()
            {
                Uid = this.Uid,
                IsHidden = this.IsHidden,
                WidthValue = this.WidthValue,
                IsCollapsed = this.IsCollapsed

            };

            return clone;
        }

        public object Clone()
        {
            return ConcreteClone();
        }
    }
}