using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetColumn: ICloneable
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public bool Hidden { get; set; }
        public double WidthValue { get; set; } = 100;
        public string Width => (WidthValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)) + "px";

        public SheetColumn()
        {

        }
        
        public SheetColumn(double widthValue)
        {
            widthValue = widthValue;
        }
        
        public SheetColumn ConcreteClone()
        {
            var clone = new SheetColumn()
            {
                Uid = this.Uid,
                Hidden = this.Hidden,
                WidthValue = this.WidthValue

            };

            return clone;
        }

        public object Clone()
        {
            return ConcreteClone();
        }
    }
}