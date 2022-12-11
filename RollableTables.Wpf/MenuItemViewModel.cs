using System.Text.Json.Serialization;

namespace RollableTables;

public abstract class MenuItemViewModel : ViewModelBase 
{
    protected MenuItemViewModel(string name)
    {
        Name = name;
        Command =  new RelayCommand(x => CommandAction(x));
    }

    public abstract MenuItemTypeDiscriminator MenuItemTypeDiscriminator { get; set; }
    
    public string Name { get; set; }
    
    [JsonIgnore]
    public RelayCommand Command { get; set; }

    [JsonIgnore]
    public MenuItemLevelViewModel? Parent { get; set; }
    
    protected abstract void CommandAction(object o);
}