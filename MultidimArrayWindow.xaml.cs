using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для MultidimArrayWindow.xaml
    /// </summary>
    public partial class MultidimArrayWindow : UserControl
    {
        private int[,] MultidimArray;
        public MultidimArrayWindow()
        {
            InitializeComponent();
        }
        private void GenerateMultidimArrayContent_Click(object sender, RoutedEventArgs e)
        {
            // Get the number of rows and columns from the text boxes
            if (!int.TryParse(rowsTextBox.Text, out int rows) || !int.TryParse(columnsTextBox.Text, out int columns))
            {
                MessageBox.Show("Недопустимый ввод. Введите допустимые целые числа для количества строк и столбцов.");
                return;
            }
            // Get the min and max values from the text boxes
            if (!int.TryParse(minValueTextBox.Text, out int minValue) || !int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                MessageBox.Show("Недопустимый ввод. Введите допустимые целые числа для минимального и максимального значений.");
                return;
            }

            // Create a random number generator
            Random random = new Random();

            // Generate a random array
            MultidimArray = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    MultidimArray[i, j] = random.Next(minValue, maxValue + 1); // Generate a random number within the specified range
                }
            }
            DisplayMultidimArray();
        }
        private void DisplayMultidimArray()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int rows = MultidimArray.GetLength(0);
            int columns = MultidimArray.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    stringBuilder.Append(MultidimArray[i, j].ToString() + " ");
                }
                stringBuilder.AppendLine(); // Add a new line after each row
            }
            //resultMultidim.Text = "Сгенерированный массив:\n" + stringBuilder.ToString();
            resultMultidim.Text = stringBuilder.ToString();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Show the main window
            Window.GetWindow(this)?.Close(); // Close the subsidiary content window
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(resultMultidim.Text);
            //MessageBox.Show("Выходные данные массива были скопированы в буфер обмена.");
        }
        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            // Открытие диалогового окна сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Получение пути выбранного файла
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Создание и использование объекта StreamWriter для записи данных в файл
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(resultMultidim.Text);
                    }

                    MessageBox.Show("Файл успешно сохранен.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении файла: " + ex.Message);
                }
            }
        }
    }
}
