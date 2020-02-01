using CarWorkshop.WPF;
using System.Windows;

namespace CarWorkshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainVM mainVM)
        {
            InitializeComponent();
            this.DataContext = mainVM;
        }
    }
}
