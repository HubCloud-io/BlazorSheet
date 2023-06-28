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
    public partial class CellLinkInputModal : ComponentBase
    {
        [Parameter]
        public EventCallback<CellLink> Closed { get; set; }

        [Parameter]
        public string Link { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public double ClientX { get; set; }

        [Parameter]
        public double ClientY { get; set; }

        private async Task OnOkClick()
        {
            var cellLink = new CellLink 
            { 
                Text = Text,
                Link = Link
            };
            await Closed.InvokeAsync(cellLink);
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
            sb.Append($"width: 150px; ");

            var topPx = $"{(int)ClientY}px";
            var leftPx = $"{(int)ClientX}px";

            sb.Append($"top: {topPx}; ");
            sb.Append($"left: {leftPx}; ");

            return sb.ToString();
        }
    }
}
