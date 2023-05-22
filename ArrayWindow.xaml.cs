using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для ArrayWindow.xaml
    /// </summary>
    public partial class ArrayWindow : UserControl
    {
        private int[] OnedimArray;
        private int[,] TwodimArray;
        public ArrayWindow()
        {
            InitializeComponent();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Show the main window
            Window.GetWindow(this)?.Close(); // Close the subsidiary content window
        }
        private void GenerateOnedimArrayContent_Click(object sender, RoutedEventArgs e)
        {
            //int size = 10;
            // Get the size of the array from the text box
            if (!int.TryParse(sizeTextBox.Text, out int size))
            {
                MessageBox.Show("Недопустимый ввод. Введите допустимое целое число для длины массива.");
                return;
            }

            // Create a random number generator
            Random random = new Random();

            // Generate a random array
            OnedimArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                OnedimArray[i] = random.Next(100); // Generate a random number between 0 and 99
            }
            DisplayOnedimArray();
        }
        private void DisplayOnedimArray()
        {
            StringBuilder stringBuilder = new StringBuilder();
            //resultOnedim.Items.Clear();
            //resultOnedim.Items.Add("Сгенерированный массив:");

            foreach (int element in OnedimArray)
            {
                //resultOnedim.Items.Add(element.ToString());
                stringBuilder.Append(element.ToString() + " ");
            }
            resultOnedim.Text = stringBuilder.ToString();
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(resultOnedim.Text);
            //MessageBox.Show("Выходные данные массива были скопированы в буфер обмена.");
        }
        private void GenerateMultidimArrayContent_Click(object sender, RoutedEventArgs e)
        {
            //int rows = 5;
            //int columns = 5;
            // Get the number of rows and columns from the text boxes
            if (!int.TryParse(rowsTextBox.Text, out int rows) || !int.TryParse(columnsTextBox.Text, out int columns))
            {
                MessageBox.Show("Недопустимый ввод. Введите допустимые целые числа для количества строк и столбцов.");
                return;
            }

            // Create a random number generator
            Random random = new Random();

            // Generate a random array
            TwodimArray = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    TwodimArray[i, j] = random.Next(100); // Generate a random number between 0 and 99
                }
            }
            DisplayTwodimArray();
        }
        private void DisplayTwodimArray()
        {
            resultMultidim.Items.Clear();
            //resultMultidim.Items.Add("Сгенерированный массив:");

            int rows = TwodimArray.GetLength(0);
            int columns = TwodimArray.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                string rowText = "";
                for (int j = 0; j < columns; j++)
                {
                    rowText += TwodimArray[i, j] + " ";
                }
                resultMultidim.Items.Add(rowText);
            }
        }
    }
}
