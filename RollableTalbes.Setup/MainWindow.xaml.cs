using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using RollableTables;
using Services;
using TableRow = Services.TableRow;

namespace RollableTalbes.MenuMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public RollableTable[] Tables { get; set; }

        public MenuTreeItems SelectedTreeItem { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Menu = MenuTreeItems.ToTree(StaticHolder.Menu);
            MenuTree.ItemsSource = new ObservableCollection<MenuTreeItems>(new[] { Menu });
            Tables = StaticHolder.TablesService.GetTables();
            UpdateTables();
        }

        private void UpdateTables()
        {
            var usedTables = Menu.GetUsedTables();

            TablesListView.ItemsSource = IsHideUsed.IsChecked ?? false
                                             ? Tables.Where(x => !usedTables.Contains(x.Name)).ToArray()
                                             : Tables;
        }

        public MenuTreeItems Menu { get; set; }

        private void ButtonImportFromJson_OnClick(object sender, RoutedEventArgs e)
        {
            var files = Directory.GetFiles(ImportFromJsonFolder.Text);

            foreach (var file in files)
            {
                try
                {
                    var text = File.ReadAllText(file);

                    Root table = JsonSerializer.Deserialize<Root>(text, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var service = new TablesService();

                    var newTable = new RollableTable
                                   {
                                       Name = table.Name,
                                       Rows = table.Results.Select(x => new TableRow { Value = x.Text, Weight = x.Weight }).ToList(),
                                   };

                    service.SaveTable(newTable);

                    Console.WriteLine($@"{newTable.Name} was added");
                    UpdateTables();
                }
                catch (Exception exception)
                {
                    // ignore
                }
            }
        }

        private void ButtonImportFromTxt_OnClick(object sender, RoutedEventArgs e)
        {
            var files = Directory.GetFiles(ImportFromTxtFolder.Text);

            foreach (var file in files)
            {
                try
                {
                    var text = File.ReadAllLines(file);

                    var newTable = new RollableTable
                                   {
                                       Name = file,
                                       Rows = new List<TableRow>(),
                                   };

                    foreach (var line in text)
                    {
                        var splited = line.Split(':');

                        if (splited.Length == 1)
                        {
                            newTable.Rows.Add(new TableRow { Value = splited.FirstOrDefault() });
                        }
                        else
                        {
                            newTable.Rows.Add(new TableRow { Value = splited[1], Weight = Int32.Parse(splited[0]) });
                        }
                    }

                    var service = new TablesService();

                    service.SaveTable(newTable);
                    UpdateTables();
                }
                catch (Exception exception)
                {
                    // ignore
                }
            }
        }

        private void ButtonRemoveSelected_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (MenuTreeItems)MenuTree.SelectedItem;
            var selectedItemContainer = FindTreeViewSelectedItemContainer(Menu, selectedItem);

            if (selectedItemContainer != null)
            {
                selectedItemContainer.Childs.Remove(selectedItem);
                UpdateTables();
            }
        }

        private static MenuTreeItems FindTreeViewSelectedItemContainer(MenuTreeItems root, MenuTreeItems selected)
        {
            MenuTreeItems result = null;

            foreach (var child in root.Childs)
            {
                if (child != selected)
                {
                    if (child.Childs != null)
                    {
                        result = FindTreeViewSelectedItemContainer(child, selected);

                        if (result != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    result = root;

                    break;
                }
            }

            return result;
        }

        private void ButtonAddFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (MenuTreeItems)MenuTree.SelectedItem;

            if (selectedItem.MenuItemTypeDiscriminator == MenuItemTypeDiscriminator.MenuItemTableViewModel)
            {
                selectedItem = FindTreeViewSelectedItemContainer(Menu, selectedItem);
            }

            selectedItem.Childs.Add(new MenuTreeItems
                                    {
                                        MenuItemTypeDiscriminator = MenuItemTypeDiscriminator.MenuItemLevelViewModel,
                                        Name = FolderName.Text,
                                        Childs = new ObservableCollection<MenuTreeItems>(),
                                    });

            FolderName.Text = string.Empty;
        }

        private void ButtonAddTable_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (MenuTreeItems)MenuTree.SelectedItem;
            var selectedTable = (RollableTable)TablesListView.SelectedItem;

            if (selectedItem.MenuItemTypeDiscriminator == MenuItemTypeDiscriminator.MenuItemTableViewModel)
            {
                selectedItem = FindTreeViewSelectedItemContainer(Menu, selectedItem);
            }

            selectedItem.Childs.Add(new MenuTreeItems
                                    {
                                        MenuItemTypeDiscriminator = MenuItemTypeDiscriminator.MenuItemTableViewModel,
                                        Name = selectedTable.Name,
                                        TableName = selectedTable.Name,
                                    });

            UpdateTables();
        }

        private void ButtonSaveMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            var serialize = MenuTreeItems.ToSerialize(Menu);
            File.WriteAllText("menu.json", System.Text.Json.JsonSerializer.Serialize(serialize, options));
        }

        private void MenuTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedTreeItem = (MenuTreeItems)MenuTree.SelectedItem;
            NodeName.Text = SelectedTreeItem.Name;
            TableName.Content = SelectedTreeItem.TableName;

            SelectedTableItems.ItemsSource = Tables.FirstOrDefault(x => SelectedTreeItem.TableName == x.Name)?.Rows;
        }

        private void ButtonBase_SaveSelectedItem(object sender, RoutedEventArgs e)
        {
            SelectedTreeItem.Name = NodeName.Text;
            MenuTree.ItemsSource = new ObservableCollection<MenuTreeItems>(new[] { Menu });
            ;
        }

        private void TablesListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTableItems.ItemsSource = ( TablesListView.SelectedItem as RollableTable )?.Rows;
        }

        private void IsRemoveUsed_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateTables();
        }

        private void ButtonUp_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (MenuTreeItems)MenuTree.SelectedItem;
            var selectedItemContainer = FindTreeViewSelectedItemContainer(Menu, selectedItem);

            if (selectedItemContainer != null)
            {
                var selectedItemIndex = selectedItemContainer.Childs.IndexOf(selectedItem);

                if (selectedItemIndex > 0)
                {
                    selectedItemContainer.Childs.Move(selectedItemIndex, selectedItemIndex - 1);
                }
            }
        }

        private void ButtonDown_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (MenuTreeItems)MenuTree.SelectedItem;
            var selectedItemContainer = FindTreeViewSelectedItemContainer(Menu, selectedItem);

            if (selectedItemContainer != null)
            {
                var selectedItemIndex = selectedItemContainer.Childs.IndexOf(selectedItem);

                if (selectedItemIndex < selectedItemContainer.Childs.Count - 1)
                {
                    selectedItemContainer.Childs.Move(selectedItemIndex, selectedItemIndex + 1);
                }
            }
        }

        private void RemoveTable_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedTable = (RollableTable)TablesListView.SelectedItem;
            StaticHolder.TablesService.DeleteTable(selectedTable.Name);
            Menu.DeleteTableByName(selectedTable.Name);
            Tables = StaticHolder.TablesService.GetTables();
            UpdateTables();
        }
    }
}