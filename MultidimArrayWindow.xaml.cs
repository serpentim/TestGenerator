using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        private Random random = new Random();
        private int[,] multidimArray;
        public MultidimArrayWindow()
        {
            InitializeComponent();
        }
        private void GenerateMultidimArray_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(rowsTextBox.Text, out int rows) &&
                int.TryParse(columnsTextBox.Text, out int columns) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                if (maxValue < minValue)
                {
                    MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                    return;
                }

                if (fewUniqueCheckBox.IsChecked == true)
                {
                    multidimArray = GenerateFewUniqueNumbersArray(rows, columns, minValue, maxValue);
                }
                else
                {
                    multidimArray = GenerateMultidimArray(rows, columns, minValue, maxValue);
                }

                DisplayMultidimArray();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для количества строк и столбцов и диапазона значений массива.");
            }
        }

        private int[,] GenerateMultidimArray(int rows, int columns, int minValue, int maxValue)
        {
            int[,] multidimArray = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    multidimArray[i, j] = random.Next(minValue, maxValue + 1);
                }
            }
            return multidimArray;
        }

        private void DisplayMultidimArray()
        {
            StringBuilder stringBuilder = new StringBuilder();

            int rows = multidimArray.GetLength(0);
            int columns = multidimArray.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    stringBuilder.Append(multidimArray[i, j].ToString() + " ");
                }
                stringBuilder.AppendLine();
            }
            resultMultidim.Text = stringBuilder.ToString();
        }

        /*private int[,] GenerateFewUniqueNumbersArray(int rows, int columns, int minValue, int maxValue)
        {
            int[,] multidimArray = new int[rows, columns];
            int quarterSize = ((rows + columns) / 2) / 5;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if ((i / quarterSize) == (j / quarterSize))
                    {
                        multidimArray[i, j] = random.Next(minValue, maxValue + 1);
                    }
                    else
                    {
                         multidimArray[i, j] = multidimArray[i - 1, j - 1];
                    }
                }
            }
            return multidimArray;
        }*/
        private int[,] GenerateFewUniqueNumbersArray(int rows, int columns, int minValue, int maxValue)
        {
            int[,] multidimArray = new int[rows, columns];
            int quarterSize = ((rows + columns) / 2) / 5;
            int previousValue = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if ((i / quarterSize) == (j / quarterSize))
                    {
                        multidimArray[i, j] = random.Next(minValue, maxValue + 1);
                        previousValue = multidimArray[i, j];
                    }
                    else
                    {
                        multidimArray[i, j] = previousValue;
                    }
                }
            }
            return multidimArray;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(resultMultidim.Text);
            //MessageBox.Show("Выходные данные массива были скопированы в буфер обмена.");
        }
        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
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

        private void CreateZip_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(fileCountTextBox.Text, out int fileCount))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "ZIP Archive (*.zip)|*.zip";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string zipFilePath = saveFileDialog.FileName;
                    int rows = int.Parse(rowsTextBox.Text);
                    int columns = int.Parse(columnsTextBox.Text);
                    int minValue = int.Parse(minValueTextBox.Text);
                    int maxValue = int.Parse(maxValueTextBox.Text);

                    using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        Random random = new Random();

                        for (int i = 0; i < fileCount; i++)
                        {
                            string fileName = $"Test{i + 1}.txt";
                            int[,] multidimArray;

                            if (fewUniqueCheckBox.IsChecked == true)
                            {
                                multidimArray = GenerateFewUniqueNumbersArray(rows, columns, minValue, maxValue);
                            }
                            else
                            {
                                multidimArray = GenerateMultidimArray(rows, columns, minValue, maxValue);
                            }

                            ZipArchiveEntry entry = zipArchive.CreateEntry(fileName);
                            using (StreamWriter writer = new StreamWriter(entry.Open(), System.Text.Encoding.UTF8))
                            {
                                writer.WriteLine(rows);
                                writer.WriteLine(columns);

                                for (int j = 0; j < rows; j++)
                                {
                                    for (int k = 0; k < columns; k++)
                                    {
                                        writer.Write(multidimArray[j, k] + " ");
                                    }
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                    //MessageBox.Show("Файлы успешно созданы внутри архива.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное количество файлов.");
            }
        }
    }
}
