using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TheMostGamesTask3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            /*StringProcessor.AddRussianStringToDictionary("Не выходи из комнаты, не совершай ошибку."); //TODO убрать тестовые строки
            StringProcessor.AddRussianStringToDictionary("строка2");
            StringProcessor.AddRussianStringToDictionary("строка3456");
            StringProcessor.AddRussianStringToDictionary("строка62");
            StringProcessor.AddRussianStringToDictionary("строкастрока");
            StringProcessor.AddRussianStringToDictionary("строкаиздалека");
            StringProcessor.AddRussianStringToDictionary("строкаиздалекаа");
            StringProcessor.AddEglishStringToDictionary("12 | ab vab");
            StringProcessor.AddEglishStringToDictionary("12 | ab");
            StringProcessor.AddEglishStringToDictionary("sfedsf | ab ab");
            StringProcessor.AddEglishStringToDictionary("bewr | ab asdfasvab");/*
            StringProcessor.FindEqualEngPetrenkoIndexStringsForAllRussianStrings();*/
        }

        /*
         * Кнопка активации поиска совпадений по индексу Петренко и выводу результата.
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<StringPair> _listOutput = new List<StringPair>();
            _listOutput.Clear();
            DataGridResultOutput.ItemsSource = _listOutput;
            StringProcessor.FindEqualEngPetrenkoIndexStringsForAllRussianStrings();
            Dictionary<int, List<int>> dictionaryProcessed = StringProcessor.EngStringsWithSamePetrenkoIndexes;

            foreach (KeyValuePair<int, List<int>> pair in dictionaryProcessed) 
            {
                foreach (int engIndex in pair.Value)
                {
                    _listOutput.Add(new StringPair(StringProcessor.GetRusStringByIndex(pair.Key), StringProcessor.GetEngStringByIndex(engIndex)));
                }
            }

            DataGridResultOutput.Items.Refresh();
        }

        /*
         * Кнопка добавления русской строки.
         */
        private void ButtonAddRusString_Click(object sender, RoutedEventArgs e)
        {
            StringProcessor.AddRussianStringToDictionary(TextBoxRusInputString.Text);
        }

        /*
         * Кнопка добавления английской строки.
         */
        private void ButtonAddEngString_Click(object sender, RoutedEventArgs e)
        {
            StringProcessor.AddEglishStringToDictionary(TextBoxEngInputString.Text);
        }
    }
}
