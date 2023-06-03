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

namespace TestGenerator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenOnedimArrayContent_Click(object sender, RoutedEventArgs e)
        {
            OnedimArrayWindow onedimArrayWindow = new OnedimArrayWindow();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(onedimArrayWindow);
        }
        private void OpenOnedimArrayRealContent_Click(object sender, RoutedEventArgs e)
        {
            OnedimArrayRealWindow onedimArrayRealWindow = new OnedimArrayRealWindow();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(onedimArrayRealWindow);
        }
        private void OpenMultidimArrayContent_Click(object sender, RoutedEventArgs e)
        {
            MultidimArrayWindow multidimArrayWindow = new MultidimArrayWindow();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(multidimArrayWindow);
        }
        private void OpenStringContent_Click(object sender, RoutedEventArgs e)
        {
            StringWindow stringWindow = new StringWindow();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(stringWindow);
        }
    }
}
