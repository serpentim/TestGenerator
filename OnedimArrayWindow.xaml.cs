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
using static TestGenerator.OnedimArrayWindow;

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
                int[] numbers;

                if (primeCheckBox.IsChecked == true || compositeCheckBox.IsChecked == true)
                {
                    numbers = GeneratePrimeOrCompositeNumbers(size, minValue, maxValue, primeCheckBox.IsChecked == true, compositeCheckBox.IsChecked == true);
                    SortOptions(numbers, minValue, maxValue);
                }
                else if (palindromeCheckBox.IsChecked == true)
                {
                    numbers = GeneratePalindromeArray(size, minValue, maxValue);
                    SortOptions(numbers, minValue, maxValue);
                }
                else if (fewUniqueCheckBox.IsChecked == true)
                {
                    numbers = GenerateFewUniqueNumbersArray(size, minValue, maxValue);
                    SortOptions(numbers, minValue, maxValue);
                }
                else if (oddCheckBox.IsChecked == true || evenCheckBox.IsChecked == true)
                {
                    numbers = GenerateOddOrEvenNumbersArray(size, minValue, maxValue);
                    SortOptions(numbers, minValue, maxValue);
                }
                else
                {
                    numbers = GenerateRandomArray(size, minValue, maxValue);
                    // Check which sorting options are selected and perform the sorting accordingly
                    SortOptions(numbers, minValue, maxValue);
                }
                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }

        private void SortOptions(int[] numbers, int minValue, int maxValue)
        {
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
                SkewNumbersNone(numbers, minValue, maxValue);
            else if (skewMinRadioButton.IsChecked == true)
                SkewNumbersTowardsMin(numbers, minValue, maxValue);
            else if (skewMaxRadioButton.IsChecked == true)
                SkewNumbersTowardsMax(numbers, minValue, maxValue);
            else if (skewBothRadioButton.IsChecked == true)
                SkewNumbersToBoth(numbers, minValue, maxValue);
        }
        private void SkewNumbersNone(int[] numbers, int minValue, int maxValue)
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

        private int[] GenerateRandomArray(int size, int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new int[0];
            }

            int[] numbers = new int[size];

            for (int i = 0; i < size; i++)
            {
                numbers[i] = random.Next(minValue, maxValue + 1);
            }

            return numbers;
        }

        private int[] GenerateOddOrEvenNumbersArray(int size, int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new int[0];
            }

            int[] numbers = new int[size];

            for (int i = 0; i < size; i++)
            {
                int randomNumber = random.Next(minValue, maxValue + 1);

                if (evenCheckBox.IsChecked == true && randomNumber % 2 == 0)
                {
                    numbers[i] = randomNumber;
                }
                else if (oddCheckBox.IsChecked == true && randomNumber % 2 != 0)
                {
                    numbers[i] = randomNumber;
                }
                else
                {
                    i--;
                }
            }

            return numbers;
        }

        private int[] GenerateFewUniqueNumbersArray(int size, int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new int[0];
            }

            int[] numbers = new int[size];
            int quarterSize = size / 5; // Размер каждых 20% массива

            for (int i = 0; i < size; i++)
            {
                // Генерируем повторяющееся число только для каждой четверти массива
                if (i % quarterSize == 0)
                {
                    numbers[i] = random.Next(minValue, maxValue + 1);
                }
                else
                {
                    // Используем предыдущее сгенерированное число для остальных элементов
                    numbers[i] = numbers[i - 1];
                }
            }

            return numbers;
        }

        private int[] GeneratePalindromeArray(int size, int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new int[0];
            }

            int[] numbers = new int[size];
            List<int> palindromeNumbers = new List<int>();

            for (int i = minValue; i <= maxValue; i++)
            {
                if (IsPalindrome(i))
                {
                    palindromeNumbers.Add(i);
                }
            }

            /*if (palindromeNumbers.Count < size)
            {
                throw new ArgumentException("Недостаточное количество палиндромов в заданном диапазоне.");
            }*/

            for (int i = 0; i < size; i++)
            {
                int randomIndex = random.Next(0, palindromeNumbers.Count);
                numbers[i] = palindromeNumbers[randomIndex];
                //palindromeNumbers.RemoveAt(randomIndex);
            }

            return numbers;
        }
        private bool IsPalindrome(int number)
        {
            string numberString = number.ToString();
            int left = 0;
            int right = numberString.Length - 1;

            while (left < right)
            {
                if (numberString[left] != numberString[right])
                {
                    return false;
                }

                left++;
                right--;
            }

            return true;
        }

        private int[] GeneratePrimeOrCompositeNumbers(int size, int minValue, int maxValue, bool generatePrimes, bool generateComposites)
        {
            if (primeCheckBox.IsChecked == true && minValue < 2)
            {
                MessageBox.Show("Мин. и макс. значения диапазона должны быть не меньше 2 для генерации простых чисел.");
                return new int[0];
            }

            if (compositeCheckBox.IsChecked == true && minValue < 4)
            {
                MessageBox.Show("Мин. и макс. значения диапазона должны быть не меньше 4 для генерации составных чисел.");
                return new int[0];
            }

            if (maxValue < minValue)
            {
                MessageBox.Show("Максимальное значение диапазона должно быть не меньше минимального значения.");
                return new int[0];
            }

            List<int> numbers = new List<int>();

            while (numbers.Count < size)
            {
                int number = random.Next(minValue, maxValue + 1);

                if (generatePrimes && IsPrime(number))
                {
                    numbers.Add(number);
                }
                else if (generateComposites && !IsPrime(number))
                {
                    numbers.Add(number);
                }
            }

            return numbers.ToArray();
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
                    using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create)) //будет ошибка если попытаться сохранить файл с уже сущ именем
                    {
                        Random random = new Random();

                        // Create the specified number of text files
                        for (int i = 0; i < fileCount; i++)
                        {
                            string fileName = $"Test{i + 1}.txt";
                            int[] numbers;

                            // Generate a new random array for each file
                            if (primeCheckBox.IsChecked == true || compositeCheckBox.IsChecked == true)
                            {
                                numbers = GeneratePrimeOrCompositeNumbers(size, minValue, maxValue, primeCheckBox.IsChecked == true, compositeCheckBox.IsChecked == true);
                                SortOptions(numbers, minValue, maxValue);
                            }
                            else if (palindromeCheckBox.IsChecked == true)
                            {
                                numbers = GeneratePalindromeArray(size, minValue, maxValue);
                                SortOptions(numbers, minValue, maxValue);
                            }
                            else if (fewUniqueCheckBox.IsChecked == true)
                            {
                                numbers = GenerateFewUniqueNumbersArray(size, minValue, maxValue);
                                SortOptions(numbers, minValue, maxValue);
                            }
                            else if (oddCheckBox.IsChecked == true || evenCheckBox.IsChecked == true)
                            {
                                numbers = GenerateOddOrEvenNumbersArray(size, minValue, maxValue);
                                SortOptions(numbers, minValue, maxValue);
                            }
                            else
                            {
                                numbers = GenerateRandomArray(size, minValue, maxValue);
                                // Check which sorting options are selected and perform the sorting accordingly
                                SortOptions(numbers, minValue, maxValue);
                            }

                            // Create a new entry in the zip archive with the file name
                            ZipArchiveEntry entry = zipArchive.CreateEntry(fileName);

                            // Write the file content to the entry
                            using (StreamWriter writer = new StreamWriter(entry.Open(), System.Text.Encoding.UTF8))
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
