using System;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetRow: ICloneable
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public double HeightValue { get; set; } = 20;

        public string Height => (HeightValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)) + "px";

        public SheetRow()
        {

        }

        public SheetRow(double heightValue)
        {
            HeightValue = heightValue;
        }

        public SheetRow ConcreteClone()
        {
            var clone = new SheetRow()
            {
                Uid = this.Uid,
                HeightValue = this.HeightValue

            };

            return clone;
        }

        public object Clone()
        {
            return ConcreteClone();
        }
    }
}