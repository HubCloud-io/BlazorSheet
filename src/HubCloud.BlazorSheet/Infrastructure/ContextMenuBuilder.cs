using BBComponents.Abstract;
using BBComponents.Enums;
using BBComponents.Models;

namespace HubCloud.BlazorSheet.Infrastructure;

public class ContextMenuBuilder
{
    public const string AddBeforeItemName = "AddBefore";
    public const string AddAfterItemName = "AddAfter";
    
    public const string WidthItemName = "Width";
    public const string HeightItemName = "Height";
    
    public const string RemoveItemName = "Remove";
    public const string CloseItemName = "Close";

    public const string SheetSizeItemName = "SheetSize";

    public const string ShowHideItemName = "Show/Hide";
    public const string ShowHiddenHideHiddenItemName = "ShowHidden/HideHidden";

    public static IEnumerable<IMenuItem> BuildColumnContextMenu(string callFrom)
    {
        var items = new List<IMenuItem>();

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = AddBeforeItemName,
            Title = "Add before",
            IconClass = "fa fa-plus text-primary"
        });
        
        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = AddAfterItemName,
            Title = "Add after",
            IconClass = "fa fa-plus text-primary"
        });

        if (callFrom == "column")
        {
            items.Add(new MenuItem()
            {
                Kind = MenuItemKinds.Item,
                Name = WidthItemName,
                Title = "Change width",
                IconClass = "fa fa-edit text-primary"
            });
        }
        
        if (callFrom == "row")
        {
            items.Add(new MenuItem()
            {
                Kind = MenuItemKinds.Item,
                Name = HeightItemName,
                Title = "Change height",
                IconClass = "fa fa-edit text-primary"
            });
        }

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = SheetSizeItemName,
            Title = "Change sheet size",
            IconClass = "fa fa-edit text-primary"
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Delimiter
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = ShowHideItemName,
            Title = ShowHideItemName,
            IconClass = "fa fa-sharp fa-regular fa-eye"
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = ShowHiddenHideHiddenItemName,
            Title = "Show hidden/Hide hidden",
            IconClass = "fa fa-sharp fa-regular fa-eye"
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Delimiter
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = RemoveItemName,
            Title = "Remove",
            IconClass = "fa fa-times text-danger"
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Delimiter
        });

        items.Add(new MenuItem()
        {
            Kind = MenuItemKinds.Item,
            Name = CloseItemName,
            Title = "Close",
            IconClass = "fa fa-times text-secondary"
        });

        return items;
    }
}