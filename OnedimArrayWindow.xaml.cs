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
    /// Логика взаимодействия для OnedimArrayWindow.xaml
    /// </summary>
    public partial class OnedimArrayWindow : UserControl
    {
        Random random = new Random();
        public OnedimArrayWindow()
        {
            InitializeComponent();

            standardRadioButton.IsChecked = true;
        }

        private void SortOption_Checked(object sender, RoutedEventArgs e)
        {
            // Clear the result text box when a sorting option is selected
            resultOnedim.Text = string.Empty;
        }

        private void GenerateArray_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);

                // Check which sorting options are selected and perform the sorting accordingly
                if (standardRadioButton.IsChecked == true)
                {
                    // Do nothing
                }
                else if (sortAscRadioButton.IsChecked == true)
                {
                    Array.Sort(numbers);
                }
                else if (sortDescRadioButton.IsChecked == true)
                {
                    Array.Sort(numbers, (a, b) => b.CompareTo(a));
                }
                else if (sortAscAlmostRadioButton.IsChecked == true)
                {
                    Array.Sort(numbers);
                    SwapTwoRandomElements(numbers);
                }
                else if (sortDescAlmostRadioButton.IsChecked == true)
                {
                    Array.Sort(numbers, (a, b) => b.CompareTo(a));
                    SwapTwoRandomElements(numbers);
                }

                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }

        private void SwapTwoRandomElements(int[] array)
        {
            // Генерация случайных индексов для элементов, которые будут меняться местами
            int index1 = random.Next(0, array.Length);
            int index2 = random.Next(0, array.Length);

            // Поиск значений элементов, которые будут меняться местами
            int value1 = array[index1];
            int value2 = array[index2];

            // Проверка, чтобы значения элементов отличались
            while (value1 == value2)
            {
                index2 = random.Next(0, array.Length);
                value2 = array[index2];
            }

            // Меняем местами значения элементов
            array[index1] = value2;
            array[index2] = value1;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Show the main window
            Window.GetWindow(this)?.Close(); // Close the subsidiary content window
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(resultOnedim.Text);
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
                        writer.Write(resultOnedim.Text);
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
                    int size = int.Parse(sizeTextBox.Text);
                    int minValue = int.Parse(minValueTextBox.Text);
                    int maxValue = int.Parse(maxValueTextBox.Text);

                    // Create a new zip archive
                    using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        Random random = new Random();

                        // Create the specified number of text files
                        for (int i = 0; i < fileCount; i++)
                        {
                            string fileName = $"Test{i + 1}.txt";

                            // Generate a new random array for each file
                            int[] numbers = GenerateRandomArray(size, minValue, maxValue);

                            if (standardRadioButton.IsChecked == true)
                            {
                                // Do nothing, numbers array is already generated randomly
                            }
                            else if (sortAscRadioButton.IsChecked == true)
                            {
                                Array.Sort(numbers);
                            }
                            else if (sortDescRadioButton.IsChecked == true)
                            {
                                Array.Sort(numbers, (a, b) => b.CompareTo(a));
                            }
                            else if (sortAscAlmostRadioButton.IsChecked == true)
                            {
                                Array.Sort(numbers);
                                SwapTwoRandomElements(numbers);
                            }
                            else if (sortDescAlmostRadioButton.IsChecked == true)
                            {
                                Array.Sort(numbers, (a, b) => b.CompareTo(a));
                                SwapTwoRandomElements(numbers);
                            }

                            // Create a new entry in the zip archive with the file name
                            ZipArchiveEntry entry = zipArchive.CreateEntry(fileName);

                            // Write the file content to the entry
                            using (StreamWriter writer = new StreamWriter(entry.Open()))
                            {
                                foreach (int number in numbers)
                                {
                                    writer.Write(number + " ");
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
        private int[] GenerateRandomArray(int size, int minValue, int maxValue)
        {
            int[] numbers = new int[size];

            for (int i = 0; i < size; i++)
            {
                numbers[i] = random.Next(minValue, maxValue + 1);
            }

            return numbers;
        }
    }
}
