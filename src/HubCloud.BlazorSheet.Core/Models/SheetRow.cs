using System;
using Newtonsoft.Json;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetRow : ICloneable
    {
        private bool _isHidden;
        private bool _isOpen;
        private bool _isCollapsed;

        public Guid Uid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid ParentUid { get; set; }
        public bool IsGroup { get; set; }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                ShouldRender = true;
            }
        }

        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
                ShouldRender = true;
            }
        }

        public bool IsCollapsed
        {
            get => _isCollapsed;
            set
            {
                _isCollapsed = value;
                ShouldRender = true;
            }
        }

        public bool IsAddRemoveAllowed { get; set; }
        public double HeightValue { get; set; } = 20;

        [JsonIgnore] public bool ShouldRender { get; set; }

        public string Height =>
            (HeightValue.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)) + "px";

        public string Key => string.IsNullOrWhiteSpace(Name) ? Uid.ToString() : Name;

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
                HeightValue = this.HeightValue,
                IsHidden = this.IsHidden,
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