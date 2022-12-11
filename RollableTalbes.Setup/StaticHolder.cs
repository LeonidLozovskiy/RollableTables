using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RollableTables;
using Services;

namespace RollableTalbes.MenuMaker;

public static class StaticHolder
{
    static StaticHolder()
    {        
        var options = new JsonSerializerOptions();
        options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        

        try
        {
            LoadMenu(options);
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
            LoadMenu(options);
        }
    }

    private static void LoadMenu(JsonSerializerOptions options)
    {
        var json = File.ReadAllText("menu.json");
        Menu = JsonSerializer.Deserialize<MenuItemSerialize>(json, options);
    }

    public static TablesService TablesService { get; set; } = new TablesService();

    public static MenuItemSerialize Menu { get; set; }
}