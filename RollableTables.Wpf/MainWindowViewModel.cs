using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RollableTables;

public class MainWindowViewModel : ViewModelBase
{
    private string _log = string.Empty;

    private MenuItemLevelViewModel _buttonsTree;

    private ObservableCollection<MenuItemViewModel> _buttons;

    public MainWindowViewModel(MenuItemViewModel buttonsTree)
    {
        _buttonsTree = (MenuItemLevelViewModel)buttonsTree;
        
        Buttons = new ObservableCollection<MenuItemViewModel>(_buttonsTree.Childs);
    }

    public string Log
    {
        get => _log;

        set => Set(ref _log, value);
    }

    public ObservableCollection<MenuItemViewModel> Buttons
    {
        get => _buttons;

        set => Set(ref _buttons, value);
    }

    public void AddToLog(string log)
    {
        Log += $"{DateTime.Now}: {log}{Environment.NewLine}";
    }

    public void ChangeButtons(MenuItemLevelViewModel level)
    {
        if (level != null)
        {
            Buttons = new ObservableCollection<MenuItemViewModel>(level.Childs);
            
            if (level.Parent != null)
            {
                var backButton = new MenuItemLevelViewModel("Back", level.Parent.Childs);
                backButton.Parent = level.Parent.Parent;
                Buttons.Add(backButton);
                Buttons.Add(new MenuItemLevelViewModel("On Top", ((MenuItemLevelViewModel)_buttonsTree).Childs));
            }
        }
    }
}