using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class OnedimArrayRealWindow : UserControl
    {
        Random random = new Random();
        public OnedimArrayRealWindow()
        {
            InitializeComponent();

            standardRadioButton.IsChecked = true;
            noSkewRadioButton.IsChecked = true;
        }

        private void SortOption_Checked(object sender, RoutedEventArgs e)
        {
        }
        private void SkewOption_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void GenerateArrayReal_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                double[] numbers;
                if (fewUniqueCheckBox.IsChecked == true)
                {
                    numbers = GenerateFewUniqueNumbersArray(size, minValue, maxValue);
                    SortOptionsReal(numbers, minValue, maxValue);
                }
                else
                {
                    numbers = GenerateRealNumbersArray(size, minValue, maxValue);
                    SortOptionsReal(numbers, minValue, maxValue);
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(size.ToString());
                sb.AppendLine(string.Join(" ", numbers.Select(n => n.ToString("0.###", CultureInfo.InvariantCulture))));
                resultOnedim.Text = sb.ToString();
                //resultOnedim.Text = string.Join(" ", numbers.Select(n => n.ToString("0.###", CultureInfo.InvariantCulture)));
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }

        private void SortOptionsReal(double[] numbers, double minValue, double maxValue)
        {
            if (standardRadioButton.IsChecked == true)
            {
                SkewNumbersFuncReal(numbers, minValue, maxValue);
            }
            else if (sortAscRadioButton.IsChecked == true)
            {
                SkewNumbersFuncReal(numbers, minValue, maxValue);
                Array.Sort(numbers);
            }
            else if (sortDescRadioButton.IsChecked == true)
            {
                SkewNumbersFuncReal(numbers, minValue, maxValue);
                Array.Sort(numbers, (a, b) => b.CompareTo(a));
            }
            else if (sortAscAlmostRadioButton.IsChecked == true)
            {
                SkewNumbersFuncReal(numbers, minValue, maxValue);
                Array.Sort(numbers);
                SwapTwoRandomElementsReal(numbers);
            }
            else if (sortDescAlmostRadioButton.IsChecked == true)
            {
                SkewNumbersFuncReal(numbers, minValue, maxValue);
                Array.Sort(numbers, (a, b) => b.CompareTo(a));
                SwapTwoRandomElementsReal(numbers);
            }
        }

        private void SwapTwoRandomElementsReal(double[] array)
        {
            // Генерация случайных индексов для элементов, которые будут меняться местами
            int index1 = random.Next(0, array.Length);
            int index2 = random.Next(0, array.Length);

            // Поиск значений элементов, которые будут меняться местами
            double value1 = array[index1];
            double value2 = array[index2];

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
        private void SkewNumbersFuncReal(double[] numbers, double minValue, double maxValue)
        {
            if (noSkewRadioButton.IsChecked == true)
                SkewNumbersNoneReal(numbers, minValue, maxValue);
            else if (skewMinRadioButton.IsChecked == true)
                SkewNumbersTowardsMinReal(numbers, minValue, maxValue);
            else if (skewMaxRadioButton.IsChecked == true)
                SkewNumbersTowardsMaxReal(numbers, minValue, maxValue);
            else if (skewBothRadioButton.IsChecked == true)
                SkewNumbersToBothReal(numbers, minValue, maxValue);
        }
        private void SkewNumbersNoneReal(double[] numbers, double minValue, double maxValue)
        {
            // Do nothing
        }
        private void SkewNumbersTowardsMinReal(double[] numbers, double minValue, double maxValue)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                // Generate a random number between minValue and skewedMaxValue
                double skewedMaxValue = Math.Max(minValue, numbers[i]);
                numbers[i] = random.NextDouble() * (skewedMaxValue - minValue) + minValue;
            }
        }
        private void SkewNumbersTowardsMaxReal(double[] numbers, double minValue, double maxValue)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                // Generate a random number between skewedMinValue and maxValue
                double skewedMinValue = Math.Min(maxValue, numbers[i]);
                numbers[i] = random.NextDouble() * (maxValue - skewedMinValue) + skewedMinValue;
            }
        }
        private void SkewNumbersToBothReal(double[] numbers, double minValue, double maxValue)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                double skewedMinValue = Math.Min(maxValue, numbers[i]);
                double skewedMaxValue = Math.Max(minValue, numbers[i]);

                numbers[i] = random.NextDouble() * (skewedMaxValue - skewedMinValue) + skewedMinValue;
            }
        }

        private double[] GenerateRealNumbersArray(int size, double minValue, double maxValue)
        {
            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new double[0];
            }

            double[] numbers = new double[size];

            for (int i = 0; i < size; i++)
            {
                numbers[i] = minValue + (random.NextDouble() * (maxValue - minValue));
            }

            return numbers;
        }

        private double[] GenerateFewUniqueNumbersArray(int size, double minValue, double maxValue)
        {
            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new double[0];
            }

            double[] numbers = new double[size];
            int quarterSize = size / 5;

            for (int i = 0; i < size; i++)
            {
                if (i % quarterSize == 0)
                {
                    numbers[i] = minValue + (random.NextDouble() * (maxValue - minValue));
                }
                else
                {
                    numbers[i] = numbers[i - 1];
                }
            }

            return numbers;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(resultOnedim.Text);
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

                    using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        Random random = new Random();

                        for (int i = 0; i < fileCount; i++)
                        {
                            string fileName = $"Test{i + 1}.txt";

                            //double[] numbers = GenerateRealNumbersArray(size, (double)minValue, (double)maxValue);
                            double[] numbers;
                            if (fewUniqueCheckBox.IsChecked == true)
                            {
                                numbers = GenerateFewUniqueNumbersArray(size, minValue, maxValue);
                                SortOptionsReal(numbers, minValue, maxValue);
                            }
                            else
                            {
                                numbers = GenerateRealNumbersArray(size, minValue, maxValue);
                                SortOptionsReal(numbers, minValue, maxValue);
                            }

                            ZipArchiveEntry entry = zipArchive.CreateEntry(fileName);
                            CultureInfo culture = new CultureInfo("en-US");
                            using (StreamWriter writer = new StreamWriter(entry.Open(), System.Text.Encoding.Default))
                            {
                                foreach (double number in numbers)
                                {
                                    string formattedNumber = number.ToString("F3", culture);
                                    writer.Write(formattedNumber + " ");
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