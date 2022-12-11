namespace RollableTables;

public class MenuItemLevelViewModel : MenuItemViewModel 
{
    public MenuItemLevelViewModel(string name, MenuItemViewModel[] childs)
    :base(name)
    {
        Childs = childs;
    }
    
    public override MenuItemTypeDiscriminator MenuItemTypeDiscriminator { get; set; } = MenuItemTypeDiscriminator.MenuItemLevelViewModel;
    
    public MenuItemViewModel[] Childs { get; set; }

    
    public void FillParents()
    {
        foreach (var child in Childs)
        {
            child.Parent = this;

            if (child is MenuItemLevelViewModel level)
            {
                level.FillParents();
            }
        }
    }

    protected override void CommandAction(object o)
    {
        StaticHolder.MainWindowViewModel.ChangeButtons(this);
    }
}