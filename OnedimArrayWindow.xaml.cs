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
    /// Логика взаимодействия для OnedimArrayWindow.xaml
    /// </summary>
    public partial class OnedimArrayWindow : UserControl
    {
        public OnedimArrayWindow()
        {
            InitializeComponent();
        }
        private void GenerateArray_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);
                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }
        private void GenerateArraySortAsc_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);
                Array.Sort(numbers);
                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }
        private void GenerateArraySortDesc_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);
                Array.Sort(numbers, (a, b) => b.CompareTo(a)); // Обратный компаратор для сортировки по убыванию
                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }
        private void GenerateArraySortAscAlmost_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);
                Array.Sort(numbers);

                // Поиск первого элемента, отличного от последнего элемента в массиве
                int lastElement = numbers[numbers.Length - 1];
                int indexToSwap = -1;
                //int indexToSwap = Array.FindIndex(numbers, x => x != numbers[lastElement]);
                for (int i = numbers.Length - 2; i >= 0; i--)
                {
                    if (numbers[i] != lastElement)
                    {
                        indexToSwap = i;
                        break;
                    }
                }

                // Поменять последний элемент с найденным элементом
                if (indexToSwap != -1)
                {
                    int temp = numbers[indexToSwap];
                    numbers[indexToSwap] = numbers[numbers.Length - 1];
                    numbers[numbers.Length - 1] = temp;
                }

                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }
        private void GenerateArraySortDescAlmost_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);
                Array.Sort(numbers, (a, b) => b.CompareTo(a));

                // Поиск первого элемента, отличного от первого элемента в массиве
                int indexToSwap = Array.FindIndex(numbers, x => x != numbers[0]);

                // Поменять первый элемент с найденным элементом
                if (indexToSwap != -1)
                {
                    int temp = numbers[0];
                    numbers[0] = numbers[indexToSwap];
                    numbers[indexToSwap] = temp;
                }

                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }
        /*private void GenerateArraySortDescAlmost_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeTextBox.Text, out int size) &&
                int.TryParse(minValueTextBox.Text, out int minValue) &&
                int.TryParse(maxValueTextBox.Text, out int maxValue))
            {
                int[] numbers = GenerateRandomArray(size, minValue, maxValue);
                Array.Sort(numbers, (a, b) => b.CompareTo(a));

                // "Рушение" порядка путем перемещения одного элемента в начало массива
                if (numbers.Length > 1)
                {
                    int temp = numbers[0];
                    numbers[0] = numbers[1];
                    numbers[1] = temp;
                }

                resultOnedim.Text = string.Join(" ", numbers);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные значения для длины и диапазона массива.");
            }
        }*/
        private int[] GenerateRandomArray(int size, int minValue, int maxValue)
        {
            Random random = new Random();
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
    }
}
