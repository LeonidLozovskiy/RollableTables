using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Services;

namespace RollableTables;

public static class StaticHolder
{
    static StaticHolder()
    {
        var options = new JsonSerializerOptions();
        options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        
        var json = File.ReadAllText("menu.json");

        MenuItemSerialize menuItemSerialize;

        try
        {
            menuItemSerialize = LoadMenu(options);
        }
        catch (FileNotFoundException e)
        {
            var newMenu = new MenuItemSerialize
                          {
                              Name = "Root",
                              Childs = new List<MenuItemSerialize>(),
                              MenuItemTypeDiscriminator = MenuItemTypeDiscriminator.MenuItemLevelViewModel,
                          };
            File.WriteAllText("menu.json", JsonSerializer.Serialize(newMenu, options));
            Task.Delay(1000);
            menuItemSerialize = LoadMenu(options);
        }
        
        var menu = MenuItemSerialize.FromSerialize(menuItemSerialize);
        
        ((MenuItemLevelViewModel)menu).FillParents();                        

        MainWindowViewModel = new MainWindowViewModel(menu);
    }

    public static TablesService TablesService { get; set; } = new TablesService();

    public static MainWindowViewModel MainWindowViewModel { get; set; }
    
    private static MenuItemSerialize? LoadMenu(JsonSerializerOptions options)
    {
        var json = File.ReadAllText("menu.json");
        return JsonSerializer.Deserialize<MenuItemSerialize>(json, options);
    }
}