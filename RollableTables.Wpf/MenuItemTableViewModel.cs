using System;
using System.Text.Json.Serialization;

namespace RollableTables;

public class MenuItemTableViewModel : MenuItemViewModel
{
    public MenuItemTableViewModel(string name, string tableName)
        : base(name)
    {
        TableName = tableName;
    }
    
    public override MenuItemTypeDiscriminator MenuItemTypeDiscriminator { get; set; } = MenuItemTypeDiscriminator.MenuItemTableViewModel;

    public string TableName { get; set; }

    protected override void CommandAction(object o)
    {
        try
        {
            var rollResult = StaticHolder.TablesService.RollTable(TableName);
            StaticHolder.MainWindowViewModel.AddToLog(rollResult.Value);
        }
        catch (Exception e)
        {
            StaticHolder.MainWindowViewModel.AddToLog(e.Message);
        }
    }
}