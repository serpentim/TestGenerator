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
    /// Interaction logic for StringWindow.xaml
    /// </summary>
    public partial class StringWindow : UserControl
    {
        private SeedGenerator seedGenerator;
        private Random random;
        //private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string UppercaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string DigitCharacters = "1234567890";

        public StringWindow()
        {
            InitializeComponent();

            seedGenerator = new SeedGenerator();
        }

        private void GenerateSeed_Click(object sender, RoutedEventArgs e)
        {
            int generatedSeed = Guid.NewGuid().GetHashCode();
            seedTextBox.Text = generatedSeed.ToString();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            int length;
            if (!int.TryParse(LengthTextBox.Text, out length))
            {
                MessageBox.Show("Недопустимое значение длины. Пожалуйста, введите допустимое целое число.");
                return;
            }
            int numberOfWords = 0;
            if (MultipleWordsRadioButton.IsChecked == true && !int.TryParse(wordCountTextBox.Text, out numberOfWords))
            {
                MessageBox.Show("Недопустимое количество слов. Пожалуйста, введите допустимое целое число.");
                return;
            }
            string generatedString = "";

            if (!string.IsNullOrWhiteSpace(seedTextBox.Text) && int.TryParse(seedTextBox.Text, out int customSeed))
            {
                random = new Random(customSeed);
            }
            else
            {
                random = new Random(seedGenerator.GetRandomSeed());
            }

            if (SingleWordRadioButton.IsChecked == true)
            {
                generatedString = GenerateRandomString(length, GetSelectedCharacters());
            }
            else if (MultipleWordsRadioButton.IsChecked == true)
            {
                generatedString = GenerateMultipleWords(length, numberOfWords, GetSelectedCharacters());
            }

            ResultTextBox.Text = generatedString;
        }
        private string GenerateRandomString(int length, string selectedCharacters)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(selectedCharacters.Length);
                sb.Append(selectedCharacters[index]);
            }

            return sb.ToString();
        }
        private string GenerateMultipleWords(int length, int numberOfWords, string selectedCharacters)
        {
            StringBuilder sb = new StringBuilder();

            int remainingLength = length;
            int remainingWords = numberOfWords;

            for (int i = 0; i < numberOfWords; i++)
            {
                int wordLength = (remainingLength / remainingWords);

                StringBuilder wordBuilder = new StringBuilder();

                for (int j = 0; j < wordLength; j++)
                {
                    int index = random.Next(selectedCharacters.Length);
                    char character = selectedCharacters[index];
                    wordBuilder.Append(character);
                }

                string word = wordBuilder.ToString();
                sb.Append(word);
                sb.Append(" ");

                remainingLength -= wordLength;
                remainingWords--;
            }

            return sb.ToString().Trim();
        }
        private string GetSelectedCharacters()
        {
            if (UpperCaseRadioButton.IsChecked == true)
            {
                return UppercaseCharacters;
            }
            else if (LowerCaseRadioButton.IsChecked == true)
            {
                return LowercaseCharacters;
            }
            else if (DigitsRadioButton.IsChecked == true)
            {
                return DigitCharacters;
            }
            else
            {
                // По умолчанию использовать все символы
                return UppercaseCharacters + LowercaseCharacters + DigitCharacters;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Show the main window
            Window.GetWindow(this)?.Close(); // Close the subsidiary content window
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ResultTextBox.Text);
            //MessageBox.Show("Выходные данные массива были скопированы в буфер обмена.");
        }
        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(LengthTextBox.Text, out int length))
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
                            writer.Write(ResultTextBox.Text);
                        }

                        MessageBox.Show("Файл успешно сохранен.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при сохранении файла: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для размера массива.");
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
                    int length = int.Parse(LengthTextBox.Text);
                    //int numberOfWords = int.Parse(wordCountTextBox.Text);
                    int numberOfWords = 0;
                    if (MultipleWordsRadioButton.IsChecked == true)
                    {
                        if (!int.TryParse(wordCountTextBox.Text, out numberOfWords))
                        {
                            MessageBox.Show("Недопустимое количество слов. Пожалуйста, введите допустимое целое число.");
                            return;
                        }
                    }
                    // Create a new zip archive
                    using (ZipArchive zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        SeedGenerator seedGenerator = new SeedGenerator(!string.IsNullOrWhiteSpace(seedTextBox.Text) ? int.Parse(seedTextBox.Text) : (int?)null);

                        if (!string.IsNullOrWhiteSpace(seedTextBox.Text))
                        {
                            int seedValue = int.Parse(seedTextBox.Text);
                            seedGenerator = new SeedGenerator(seedValue);
                        }

                        for (int i = 0; i < fileCount; i++)
                        {
                            string fileName = $"Test{i + 1}.txt";
                            string generatedString = "";

                            // Генерация случайных чисел с использованием указанного сида или случайного сида
                            int seed = seedGenerator.GetRandomSeed();
                            Random random = new Random(seed);

                            if (SingleWordRadioButton.IsChecked == true)
                            {
                                generatedString = GenerateRandomString(length, GetSelectedCharacters());
                            }
                            else if (MultipleWordsRadioButton.IsChecked == true)
                            {
                                generatedString = GenerateMultipleWords(length, numberOfWords, GetSelectedCharacters());
                            }

                            // Create a new entry in the zip archive with the file name
                            ZipArchiveEntry entry = zipArchive.CreateEntry(fileName);

                            // Write the file content to the entry
                            using (StreamWriter writer = new StreamWriter(entry.Open(), System.Text.Encoding.UTF8))
                            {
                                writer.WriteLine(length);
                                writer.WriteLine(generatedString);
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
