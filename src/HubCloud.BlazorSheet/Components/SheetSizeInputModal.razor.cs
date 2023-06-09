using HubCloud.BlazorSheet.Core.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubCloud.BlazorSheet.Components
{
    public partial class SheetSizeInputModal : ComponentBase
    {
        [Parameter]
        public EventCallback Closed { get; set; }

        [Parameter]
        public int Rows { get; set; }

        [Parameter]
        public int Columns { get; set; }

        [Parameter] 
        public int Width { get; set; } = 200;

        /// <summary>
        /// X position of mouse click.
        /// </summary>
        [Parameter]
        public double ClientX { get; set; }

        /// <summary>
        /// Y position of mouse click.
        /// </summary>
        [Parameter]
        public double ClientY { get; set; }

        private async Task OnOkClick()
        {
            var arg = new SheetSize
            {
                Columns = Columns,
                Rows = Rows
            };

            await Closed.InvokeAsync(arg);
        }

        private async Task OnCancelClick()
        {
            await Closed.InvokeAsync(null);
        }

        private string ComponentStyle()
        {
            var sb = new StringBuilder();

            sb.Append("position: fixed; ");
            sb.Append("background-color: white; ");
            sb.Append("z-index: 1000; ");
            sb.Append("padding: 3px; ");
            sb.Append("border: 1px solid #b8b8b8; ");
            sb.Append($"width: {Width}px; ");

            var topPx = $"{(int)ClientY}px";
            var leftPx = $"{(int)ClientX}px";

            sb.Append($"top: {topPx}; ");
            sb.Append($"left: {leftPx}; ");

            return sb.ToString();
        }
    }
}
