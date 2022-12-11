using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        var json = File.ReadAllText("menu.json");
        
        Menu = JsonSerializer.Deserialize<MenuItemSerialize>(json, options);
    }

    public static TablesService TablesService { get; set; } = new TablesService();

    public static MenuItemSerialize Menu { get; set; }
}