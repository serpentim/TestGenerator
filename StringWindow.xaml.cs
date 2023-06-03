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
    /// Interaction logic for StringWindow.xaml
    /// </summary>
    public partial class StringWindow : UserControl
    {
        //private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string UppercaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string DigitCharacters = "1234567890";

        public StringWindow()
        {
            InitializeComponent();
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
            if (MultipleWordsRadioButton.IsChecked == true && !int.TryParse(NumberOfWordsTextBox.Text, out numberOfWords))
            {
                MessageBox.Show("Недопустимое количество слов. Пожалуйста, введите допустимое целое число.");
                return;
            }
            string generatedString = "";

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
            Random random = new Random();
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
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < numberOfWords; i++)
            {
                int wordLength = random.Next(1, length / numberOfWords + 1);
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
    }
}
