using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RollableTables;

namespace RollableTalbes.MenuMaker;

public class MenuTreeItems
{
    public MenuItemTypeDiscriminator MenuItemTypeDiscriminator { get; set; }
    public string TableName { get; set; }
    public string Name { get; set; }
    public ObservableCollection<MenuTreeItems> Childs { get; set; }

    public string[] GetUsedTables()
    {
        List<string> result = new List<string>();
        if (!string.IsNullOrWhiteSpace(TableName))
        {
            result.Add(TableName);
        }

        if (Childs != null)
        {
            foreach (var child in Childs)
            {
                result.AddRange(child.GetUsedTables());
            }
        }

        return result.ToArray();
    }

    public void DeleteTableByName(string tableName)
    {
        var valueToDelete = Childs.FirstOrDefault(x => x.TableName == tableName);
        if (valueToDelete != null)
        {
            Childs.Remove(valueToDelete);
        }

        foreach (var child in Childs.Where(x => x.MenuItemTypeDiscriminator == MenuItemTypeDiscriminator.MenuItemLevelViewModel))
        {
            child.DeleteTableByName(tableName);
        }
    }

    public static MenuTreeItems ToTree(MenuItemSerialize item)
    {
        if (item == null)
        {
            return null;
        }
        
        var result = new MenuTreeItems
               {
                   MenuItemTypeDiscriminator = item.MenuItemTypeDiscriminator,
                   TableName = item.TableName,
                   Name = item.Name,
                   
               };

        if (item.Childs != null)
        {
            result.Childs = new ObservableCollection<MenuTreeItems>(item.Childs?.Select(MenuTreeItems.ToTree)?.ToArray());
        }
        
        return result;
    }
    
    public static MenuItemSerialize ToSerialize(MenuTreeItems item)
    {
        if (item == null)
        {
            return null;
        }
        
        var result = new MenuItemSerialize
               {
                   MenuItemTypeDiscriminator = item.MenuItemTypeDiscriminator,
                   TableName = item.TableName,
                   Name = item.Name,
                   Childs = new List<MenuItemSerialize>(item.Childs?.Select(MenuTreeItems.ToSerialize)?.ToArray()?? new MenuItemSerialize []{}),
               };
        
        if (item.Childs != null)
        {
            result.Childs = new List<MenuItemSerialize>(item.Childs?.Select(MenuTreeItems.ToSerialize)?.ToArray());
        }
        return result;
    }
}