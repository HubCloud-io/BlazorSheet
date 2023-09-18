using System;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellEditSettings
    {
        public Guid Uid { get; set; } = Guid.NewGuid();

        public CellControlKinds ControlKind { get; set; }
        public int CellDataType { get; set; }
        public string ItemsSource { get; set; } = string.Empty;
        public int NumberDigits { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool AutoGenerate { get; set; }
        public string AutoGenerateMask { get; set; }
        public CellAutoClearMethods AutoClearMethod { get; set; }
        

        public SheetCellEditSettings()
        {
        }

        public SheetCellEditSettings(SheetCommandPanelModel commandPanelModel)
        {
            CellDataType = commandPanelModel.DataType;
            ItemsSource = commandPanelModel.ItemsSource;
            ControlKind = commandPanelModel.ControlKind;
            NumberDigits = commandPanelModel.NumberDigits;
        }

        public bool IsStandard()
        {
            return ControlKind == CellControlKinds.Undefined;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            SheetCellEditSettings other = (SheetCellEditSettings) obj;
            return ControlKind == other.ControlKind &&
                   NumberDigits == other.NumberDigits &&
                   CellDataType == other.CellDataType &&
                   (ItemsSource?.Equals(other.ItemsSource, StringComparison.OrdinalIgnoreCase) ?? true) &&
                   Required == other.Required &&
                   (DefaultValue?.Equals(other.DefaultValue, StringComparison.OrdinalIgnoreCase) ?? true) &&
                   AutoGenerate == other.AutoGenerate &&
                   (AutoGenerateMask?.Equals(other.AutoGenerateMask, StringComparison.OrdinalIgnoreCase) ?? true) &&
                   AutoClearMethod == other.AutoClearMethod;
        }

        // override GetHashCode as well when Equals is overridden
        public override int GetHashCode()
        {
            // use prime numbers to calculate hash code
            int hash = 17;
            hash = hash * 23 + ControlKind.GetHashCode();
            hash = hash * 23 + NumberDigits.GetHashCode();
            hash = hash * 23 + CellDataType.GetHashCode();
            hash = hash * 23 + ItemsSource?.GetHashCode() ?? 0;
            hash = hash * 23 + Required.GetHashCode();
            hash = hash * 23 + DefaultValue?.GetHashCode() ?? 0;
            hash = hash * 23 + AutoGenerate.GetHashCode();
            hash = hash * 23 + AutoGenerateMask?.GetHashCode() ?? 0;
            hash = hash * 23 + AutoClearMethod.GetHashCode();
            
            return hash;
        }

        public static int GetDataType(CellControlKinds controlKind)
        {
            var dataType = 0;

            switch (controlKind)
            {
                case CellControlKinds.TextInput:
                    dataType = (int) CellDataTypes.String;
                    break;
                case CellControlKinds.NumberInput:
                    dataType = (int) CellDataTypes.Number;
                    break;
                case CellControlKinds.DateInput:
                    dataType = (int) CellDataTypes.DateTime;
                    break;
                case CellControlKinds.DateTimeInput:
                    dataType = (int) CellDataTypes.DateTime;
                    break;
                case CellControlKinds.CheckBox:
                    dataType = (int) CellDataTypes.Bool;
                    break;
                case CellControlKinds.ComboBox:
                    dataType = 0;
                    break;
                case CellControlKinds.Select:
                    dataType = 0;
                    break;
                default:
                    dataType = 0;
                    break;
            }

            return dataType;
        }
    }
}