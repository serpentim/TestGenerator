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
            noSkewRadioButton.IsChecked = true;
        }

        private void SortOption_Checked(object sender, RoutedEventArgs e)
        {
            // Clear the result text box when a sorting option is selected
            //resultOnedim.Text = string.Empty;
        }
        private void SkewOption_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void GenerateArray_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                /*List<int> list_numbers = new List<int>(size); // Create a list with the specified size

                while (list_numbers.Count < size) // Continue adding numbers until the desired size is reached
                {
                    int number = random.Next(minValue, maxValue + 1);

                    // Check if the "Primes" checkbox is checked and the number is prime
                    if (primeCheckBox.IsChecked == true && IsPrime(number))
                    {
                        list_numbers.Add(number);
                    }
                    // Check if the "Composites" checkbox is checked and the number is composite
                    else if (compositeCheckBox.IsChecked == true && !IsPrime(number))
                    {
                        list_numbers.Add(number);
                    }
                    // Check if neither checkbox is checked, then add the number regardless
                    else if (primeCheckBox.IsChecked == false && compositeCheckBox.IsChecked == false)
                    {
                        list_numbers.Add(number);
                    }
                }

                // Convert the list to an array
                int[] arrayNumbers = list_numbers.ToArray();*/


                int[] numbers = GenerateRandomArray(size, minValue, maxValue);

                

                /*// Check if the "Простые числа" checkbox is checked
                if (primeCheckBox.IsChecked == true)
                {
                    List<int> primeNumbers = GetPrimeNumbers(numbers);
                    numbers = primeNumbers.ToArray();
                }

                // Check if the "Составные числа" checkbox is checked
                if (compositeCheckBox.IsChecked == true)
                {
                    List<int> compositeNumbers = GetCompositeNumbers(numbers);
                    numbers = compositeNumbers.ToArray();
                }*/

                // Check which sorting options are selected and perform the sorting accordingly
                if (standardRadioButton.IsChecked == true)
                {
                    SkewNumbersFunc(numbers, minValue, maxValue);
                }
                else if (sortAscRadioButton.IsChecked == true)
                {
                    SkewNumbersFunc(numbers, minValue, maxValue);
                    Array.Sort(numbers);
                }
                else if (sortDescRadioButton.IsChecked == true)
                {
                    SkewNumbersFunc(numbers, minValue, maxValue);
                    Array.Sort(numbers, (a, b) => b.CompareTo(a));
                }
                else if (sortAscAlmostRadioButton.IsChecked == true)
                {
                    SkewNumbersFunc(numbers, minValue, maxValue);
                    Array.Sort(numbers);
                    SwapTwoRandomElements(numbers);
                }
                else if (sortDescAlmostRadioButton.IsChecked == true)
                {
                    SkewNumbersFunc(numbers, minValue, maxValue);
                    Array.Sort(numbers, (a, b) => b.CompareTo(a));
                    SwapTwoRandomElements(numbers);
                }

                // Check if the "Many Same Numbers" checkbox is checked
                if (manySameNumbersCheckBox.IsChecked == true)
                {
                    int count = (int)(0.8 * numbers.Length); // Пример: 80% элементов будут иметь совпадения
                    int[] sameNumbers = GenerateRandomSameNumbers(minValue, maxValue, count);
                    AddManySameNumbers(numbers, sameNumbers);
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

        private void SkewNumbersFunc(int[] numbers, int minValue, int maxValue)
        {
            if (noSkewRadioButton.IsChecked == true)
                SkewNumbersNone(numbers);
            else if (skewMinRadioButton.IsChecked == true)
                SkewNumbersTowardsMin(numbers, minValue, maxValue);
            else if (skewMaxRadioButton.IsChecked == true)
                SkewNumbersTowardsMax(numbers, minValue, maxValue);
            else if (skewBothRadioButton.IsChecked == true)
                SkewNumbersToBoth(numbers, minValue, maxValue);
        }
        private void SkewNumbersNone(int[] numbers)
        {
            // Do nothing
        }
        private void SkewNumbersTowardsMin(int[] numbers, int minValue, int maxValue)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                // Generate a random number between minValue and skewedMaxValue
                int skewedMaxValue = Math.Max(minValue, numbers[i]);
                numbers[i] = random.Next(minValue, skewedMaxValue + 1);
            }
        }
        private void SkewNumbersTowardsMax(int[] numbers, int minValue, int maxValue)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                // Generate a random number between skewedMinValue and maxValue
                int skewedMinValue = Math.Min(maxValue, numbers[i]);
                numbers[i] = random.Next(skewedMinValue, maxValue + 1);
            }
        }
        private void SkewNumbersToBoth(int[] numbers, int minValue, int maxValue)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                int skewedMinValue = Math.Min(maxValue, numbers[i]);
                int skewedMaxValue = Math.Max(minValue, numbers[i]);

                numbers[i] = random.Next(skewedMinValue, skewedMaxValue + 1);
            }
        }

        private void AddManySameNumbers(int[] numbers, int[] sameNumbers)
        {
            // Check if the "Many Same Numbers" checkbox is checked
            if (manySameNumbersCheckBox.IsChecked == true)
            {
                foreach (int number in sameNumbers)
                {
                    Array.Resize(ref numbers, numbers.Length + 1);
                    numbers[numbers.Length - 1] = number;
                }
            }
        }
        private int[] GenerateRandomSameNumbers(int minValue, int maxValue, int count)
        {
            int[] sameNumbers = new int[count];

            for (int i = 0; i < count; i++)
            {
                sameNumbers[i] = random.Next(minValue, maxValue + 1);
            }

            return sameNumbers;
        }

        /*private List<int> GetPrimeNumbers(int[] numbers)
        {
            List<int> primeNumbers = new List<int>();

            foreach (int number in numbers)
            {
                if (IsPrime(number))
                {
                    primeNumbers.Add(number);
                }
            }

            return primeNumbers;
        }

        private List<int> GetCompositeNumbers(int[] numbers)
        {
            List<int> compositeNumbers = new List<int>();

            foreach (int number in numbers)
            {
                if (!IsPrime(number))
                {
                    compositeNumbers.Add(number);
                }
            }

            return compositeNumbers;
        }

        private bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }*/

        private int[] GenerateRandomArray(int size, int minValue, int maxValue)
        {
            int[] numbers = new int[size];

            for (int i = 0; i < size; i++)
            {
                numbers[i] = random.Next(minValue, maxValue + 1);
            }

            return numbers;
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
                                // Do nothing
                                SkewNumbersFunc(numbers, minValue, maxValue);
                            }
                            else if (sortAscRadioButton.IsChecked == true)
                            {
                                SkewNumbersFunc(numbers, minValue, maxValue);
                                Array.Sort(numbers);
                            }
                            else if (sortDescRadioButton.IsChecked == true)
                            {
                                SkewNumbersFunc(numbers, minValue, maxValue);
                                Array.Sort(numbers, (a, b) => b.CompareTo(a));
                            }
                            else if (sortAscAlmostRadioButton.IsChecked == true)
                            {
                                SkewNumbersFunc(numbers, minValue, maxValue);
                                Array.Sort(numbers);
                                SwapTwoRandomElements(numbers);
                            }
                            else if (sortDescAlmostRadioButton.IsChecked == true)
                            {
                                SkewNumbersFunc(numbers, minValue, maxValue);
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
    }
}
