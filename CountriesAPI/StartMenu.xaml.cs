using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CountriesAPI
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl
    {
        MainWindow _main;
        public StartMenu(MainWindow main) 
        {
            InitializeComponent();
            _main = main;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _main.start.Children.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            About about = new About(_main);
            _main.start.Children.Clear();
            _main.start.Children.Add(about);
        }
    }
}
