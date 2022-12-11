using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace RollableTables;

public class MenuItemSerialize
{
    public MenuItemTypeDiscriminator MenuItemTypeDiscriminator { get; set; }
    public string TableName { get; set; }
    public string Name { get; set; }
    public List<MenuItemSerialize> Childs { get; set; }


    public static MenuItemSerialize ToSerialize(MenuItemViewModel item)
    {
        if (item is MenuItemLevelViewModel level)
        {
            return new MenuItemSerialize
                   {
                       Name = level.Name,
                       Childs = level.Childs.Select(MenuItemSerialize.ToSerialize).ToList(),
                       MenuItemTypeDiscriminator = level.MenuItemTypeDiscriminator,
                   };
        }

        if (item is MenuItemTableViewModel table)
        {
            return new MenuItemSerialize
                   {
                       Name = table.Name,
                       TableName = table.TableName,
                       MenuItemTypeDiscriminator = table.MenuItemTypeDiscriminator,
                   };
        }

        return null;
    }

    public static MenuItemViewModel FromSerialize(MenuItemSerialize item)
    {
        if (item.MenuItemTypeDiscriminator == RollableTables.MenuItemTypeDiscriminator.MenuItemLevelViewModel)
        {
            return new MenuItemLevelViewModel(item.Name, item.Childs.Select(MenuItemSerialize.FromSerialize).ToArray());
        }

        if (item.MenuItemTypeDiscriminator == RollableTables.MenuItemTypeDiscriminator.MenuItemTableViewModel)
        {
            return new MenuItemTableViewModel(item.Name, item.TableName);
        }

        return null;
    }
}