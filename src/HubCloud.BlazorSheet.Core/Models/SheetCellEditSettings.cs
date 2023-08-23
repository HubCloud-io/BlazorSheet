using System;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellEditSettings
    {
        private CellControlKinds _controlKind;

        public Guid Uid { get; set; } = Guid.NewGuid();

        public CellControlKinds ControlKind
        {
            get { return _controlKind; }
            set
            {
                _controlKind = value;
                CellDataType = GetDataType(_controlKind);
            }
        }

        public int CellDataType { get; set; }
        public string ItemsSource { get; set; } = string.Empty;
        public int NumberDigits { get; set; }

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
                   (ItemsSource?.Equals(other.ItemsSource, StringComparison.OrdinalIgnoreCase) ?? true);
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
            return hash;
        }

        public int GetDataType(CellControlKinds controlKind)
        {
            var dataType = 0;

            switch (_controlKind)
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