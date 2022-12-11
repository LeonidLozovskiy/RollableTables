using System;
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
        
        // MenuItemViewModel test = new MenuItemLevelViewModel("Root",
        //                                                     new MenuItemViewModel[]
        //                                                     {
        //                                                         new MenuItemTableViewModel("Безделушки", "Безделушки"),
        //                                                         new MenuItemLevelViewModel("Level", new MenuItemViewModel[]
        //                                                                                             {
        //                                                                                                 new MenuItemTableViewModel("Безделушки", "Безделушки"),
        //                                                                                                 new MenuItemLevelViewModel("Level2", new MenuItemViewModel[]
        //                                                                                                                                     {
        //                                                                                                                                         new MenuItemTableViewModel("Безделушки", "Безделушки"),
        //                                                                                                                                         new MenuItemTableViewModel("Безделушки", "Безделушки"),
        //                                                                                                                                     })
        //                                                                                             })
        //                                                     }
        //                                                    );
        //
        //
        //
        // var tt = MenuItemSerialize.ToSerialize(test);
        // File.WriteAllText("menu.json",System.Text.Json.JsonSerializer.Serialize(tt, options));
        // Task.Delay(1000);
        var json = File.ReadAllText("menu.json");
        
        var menuItemSerialize = JsonSerializer.Deserialize<MenuItemSerialize>(json, options);

        var menu = MenuItemSerialize.FromSerialize(menuItemSerialize);
        
        ((MenuItemLevelViewModel)menu).FillParents();                        

        MainWindowViewModel = new MainWindowViewModel(menu);
    }

    public static TablesService TablesService { get; set; } = new TablesService();

    public static MainWindowViewModel MainWindowViewModel { get; set; }
}