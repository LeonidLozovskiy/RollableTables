using System.Windows;

namespace RollableTables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = StaticHolder.MainWindowViewModel;
        }
    }
}