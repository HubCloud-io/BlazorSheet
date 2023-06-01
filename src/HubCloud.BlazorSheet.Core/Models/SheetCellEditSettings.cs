﻿using System;
using HubCloud.BlazorSheet.Core.Enums;

namespace HubCloud.BlazorSheet.Core.Models
{
    public class SheetCellEditSettings
    {
        public Guid Uid { get; set; } = Guid.NewGuid();
        public CellControlKinds ControlKind { get; set; }
        public int NumberDigits { get; set; }

        public SheetCellEditSettings()
        {
                
        }

        public SheetCellEditSettings(SheetCommandPanelModel commandPanelModel)
        {
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

            SheetCellEditSettings other = (SheetCellEditSettings)obj;
            return ControlKind == other.ControlKind &&
                   NumberDigits == other.NumberDigits;
        }

        // override GetHashCode as well when Equals is overridden
        public override int GetHashCode()
        {
            // use prime numbers to calculate hash code
            int hash = 17;
            hash = hash * 23 + ControlKind.GetHashCode();
            hash = hash * 23 + NumberDigits.GetHashCode();
            return hash;
        }
    }
}