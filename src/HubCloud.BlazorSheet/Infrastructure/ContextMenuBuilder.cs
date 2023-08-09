using BBComponents.Abstract;
using BBComponents.Enums;
using BBComponents.Models;
using HubCloud.BlazorSheet.Core.Enums;

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

    public static IEnumerable<IMenuItem> BuildColumnContextMenu(string callFrom, SheetRegimes regime, bool allowAddRemove)
    {
        var isAddRemoveAllowed = regime == SheetRegimes.Design || allowAddRemove;
        var isDesignRegime = regime == SheetRegimes.Design;

        var items = new List<IMenuItem>();

        AddAddRemoveItems(items, isAddRemoveAllowed);
        AddDesignItems(items, isDesignRegime, callFrom);
        AddShowHideItems(items, isDesignRegime);
        AddCloseItems(items, isAddRemoveAllowed);

        // if (isAddRemoveAllowed)
        // {
        //     items.Add(new MenuItem()
        //     {
        //         Kind = MenuItemKinds.Item,
        //         Name = AddBeforeItemName,
        //         Title = "Add before",
        //         IconClass = "fa fa-plus text-primary"
        //     });
        //
        //     items.Add(new MenuItem()
        //     {
        //         Kind = MenuItemKinds.Item,
        //         Name = AddAfterItemName,
        //         Title = "Add after",
        //         IconClass = "fa fa-plus text-primary"
        //     });
        // }
        //
        //
        // if (isDesignRegime)
        // {
        //     if (callFrom == "column")
        //     {
        //         items.Add(new MenuItem()
        //         {
        //             Kind = MenuItemKinds.Item,
        //             Name = WidthItemName,
        //             Title = "Change width",
        //             IconClass = "fa fa-edit text-primary"
        //         });
        //     }
        //
        //     if (callFrom == "row")
        //     {
        //         items.Add(new MenuItem()
        //         {
        //             Kind = MenuItemKinds.Item,
        //             Name = HeightItemName,
        //             Title = "Change height",
        //             IconClass = "fa fa-edit text-primary"
        //         });
        //     }
        //
        //     items.Add(new MenuItem()
        //     {
        //         Kind = MenuItemKinds.Item,
        //         Name = SheetSizeItemName,
        //         Title = "Change sheet size",
        //         IconClass = "fa fa-edit text-primary"
        //     });
        // }
        //
        // items.Add(new MenuItem()
        // {
        //     Kind = MenuItemKinds.Delimiter
        // });
        //
        // if (isDesignRegime)
        // {
        //     items.Add(new MenuItem()
        //     {
        //         Kind = MenuItemKinds.Item,
        //         Name = ShowHideItemName,
        //         Title = ShowHideItemName,
        //         IconClass = "fa fa-sharp fa-regular fa-eye"
        //     });
        // }
        //
        // items.Add(new MenuItem()
        // {
        //     Kind = MenuItemKinds.Item,
        //     Name = ShowHiddenHideHiddenItemName,
        //     Title = "Show hidden/Hide hidden",
        //     IconClass = "fa fa-sharp fa-regular fa-eye"
        // });
        //
        // if (isAddRemoveAllowed)
        // {
        //     items.Add(new MenuItem()
        //     {
        //         Kind = MenuItemKinds.Delimiter
        //     });
        //
        //     items.Add(new MenuItem()
        //     {
        //         Kind = MenuItemKinds.Item,
        //         Name = RemoveItemName,
        //         Title = "Remove",
        //         IconClass = "fa fa-times text-danger"
        //     });
        // }
        //
        // items.Add(new MenuItem()
        // {
        //     Kind = MenuItemKinds.Delimiter
        // });
        //
        // items.Add(new MenuItem()
        // {
        //     Kind = MenuItemKinds.Item,
        //     Name = CloseItemName,
        //     Title = "Close",
        //     IconClass = "fa fa-times text-secondary"
        // });

        return items;
    }
    
    private static void AddAddRemoveItems(List<IMenuItem> items, bool isAddRemoveAllowed)
    {
        if (!isAddRemoveAllowed) return;

        items.AddRange(new[]
        {
            CreateMenuItem(MenuItemKinds.Item, AddBeforeItemName, "Add before", "fa fa-plus text-primary"),
            CreateMenuItem(MenuItemKinds.Item, AddAfterItemName, "Add after", "fa fa-plus text-primary")
        });
    }
    
    private static void AddDesignItems(List<IMenuItem> items, bool isDesignRegime, string callFrom)
    {
        if (!isDesignRegime) return;

        var sizeItemName = callFrom == "column" ? WidthItemName : (callFrom == "row" ? HeightItemName : null);
        var sizeTitle = callFrom == "column" ? "Change width" : (callFrom == "row" ? "Change height" : null);

        if (sizeItemName != null)
        {
            items.Add(CreateMenuItem(MenuItemKinds.Item, sizeItemName, sizeTitle, "fa fa-edit text-primary"));
        }

        items.Add(CreateMenuItem(MenuItemKinds.Item, SheetSizeItemName, "Change sheet size", "fa fa-edit text-primary"));
    }
    
    private static void AddShowHideItems(List<IMenuItem> items, bool isDesignRegime)
    {
        items.Add(CreateDelimiter());

        if (isDesignRegime)
        {
            items.Add(CreateMenuItem(MenuItemKinds.Item, ShowHideItemName, ShowHideItemName, "fa fa-sharp fa-regular fa-eye"));
        }

        items.Add(CreateMenuItem(MenuItemKinds.Item, ShowHiddenHideHiddenItemName, "Show hidden/Hide hidden", "fa fa-sharp fa-regular fa-eye"));
    }
    
    private static void AddCloseItems(List<IMenuItem> items, bool isAddRemoveAllowed)
    {
        if (isAddRemoveAllowed)
        {
            items.Add(CreateDelimiter());
            items.Add(CreateMenuItem(MenuItemKinds.Item, RemoveItemName, "Remove", "fa fa-times text-danger"));
        }

        items.Add(CreateDelimiter());
        items.Add(CreateMenuItem(MenuItemKinds.Item, CloseItemName, "Close", "fa fa-times text-secondary"));
    }
    
    private static IMenuItem CreateMenuItem(MenuItemKinds kind, string name, string title, string iconClass)
    {
        return new MenuItem()
        {
            Kind = kind,
            Name = name,
            Title = title,
            IconClass = iconClass
        };
    }
    
    private static IMenuItem CreateDelimiter()
    {
        return new MenuItem() { Kind = MenuItemKinds.Delimiter };
    }
}