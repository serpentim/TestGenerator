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
        private void OpenArrayContent_Click(object sender, RoutedEventArgs e)
        {
            ArrayWindow arrayWindow = new ArrayWindow();
            MainGrid.Children.Clear();
            MainGrid.Children.Add(arrayWindow);
        }
        private void OpenGraphContent_Click(object sender, RoutedEventArgs e)
        {
        }
        private void OpenMatrixContent_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
