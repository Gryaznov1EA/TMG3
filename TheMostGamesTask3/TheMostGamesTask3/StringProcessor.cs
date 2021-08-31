using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TheMostGamesTask3
{
    static class StringProcessor
    {
        /*
         * Структура для русской строки.
         */
        struct RussianString
        {
            string textMain; //Основной текст русской строки.
            double indexPetrenko; //Индекс Петренко основного текста русской строки.

            public string TextMain { get; set; }

            public double IndexPetrenko { get; set; }

            public RussianString(string text, double index) : this()
            {
                TextMain = text;
                IndexPetrenko = index;
            }

            
        }

        /*
         * Структура для английской строки.
         */
        struct EnglishString
        {

            string textMain; //Основной текст английской строки.
            string textComment; //Комментарий английской строки.
            double textMainIndexPetrenko; //Индекс Петренко основного текста английской строки.
            double textCommentIndexPetrenko; //Индекс Петренко комментария английской строки.

            public string TextMain { get; set; }

            public string TextComment { get; set; }

            public double TextMainIndexPetrenko { get; set; }

            public double TextCommentIndexPetrenko { get; set; }
            public EnglishString(string textMainInput, string textCommentInput, double textMainIndexInput, double textCommentIndexInput) : this()
            {
                TextMain = textMainInput;
                TextComment = textCommentInput;
                TextMainIndexPetrenko = textMainIndexInput;
                TextCommentIndexPetrenko = textCommentIndexInput;
            }
        }

        static Dictionary<int, RussianString> _russianStrings = new Dictionary<int, RussianString>(); //Словарь для русских строк.
        static Dictionary<int, EnglishString> _englishStrings = new Dictionary<int, EnglishString>(); //Словарь для английских строк.
        static Dictionary<int, List<int>> _engStringsWithSamePetrenkoIndexes = new Dictionary<int, List<int>>(); //Словарь соответствия индексов английских строк
                                                                                                                 //индексу русской строки.

        /*
         * Свойство get для получения словаря соответствия индексов английских строк индексу русской строки.
         */
        public static Dictionary<int, List<int>> EngStringsWithSamePetrenkoIndexes
        {
            get
            {
                return _engStringsWithSamePetrenkoIndexes;
            }
        }

        /*
         * Получение по индексу русской строкисписка индексов английских строк, 
         * сумма индексов Петренко текста и комментария которых 
         * соответствуют индексу Петренко русской строки. 
         */
        public static List<int> GetListOfEngIndexes(int ruIndex)
        {
            return _engStringsWithSamePetrenkoIndexes.GetValueOrDefault(ruIndex);
        }

        /*
         * Получение текста русской строки по индексу строки в списке русских строк.
         */
        public static string GetRusStringByIndex(int ruIndex)
        {
            try
            {
                RussianString ruTextRS = new RussianString();
                _russianStrings.TryGetValue(ruIndex, out ruTextRS);
                return ruTextRS.TextMain;
            }
            catch (Exception e)
            {
                return "Wrong Rus index";
            }
        }

        /*
         * Получение текста английской строки по индексу строки в списке английских строк.
         */
        public static string GetEngStringByIndex(int enIndex)
        {
            try
            {
                EnglishString enTextES = new EnglishString();
                _englishStrings.TryGetValue(enIndex, out enTextES);
                return new string(enTextES.TextMain + "|" + enTextES.TextComment);
            }
            catch (Exception e)
            {
                return "Wrong Eng index";
            }
        }


        /*
        * Индекс Петренко рассчитывается следующим образом:
        * Поскольку для каждого значимого символа индекс будет отсчитываться от 0.5 и будет увеличиваться на единицу, 
        * нахождение суммы индексов можно упростить, разбив процесс на 
        * а) нахождение суммы целых частей коэффициэнтов (от 0 для первого символа до n-1 для последнего 
        * (где n - количество значимых символов в строке) в виде формулы (A*(A+1))/2 (формула сложения целых чисел от 1 до А).
        * В данном случае A (наибольший член последовательности) будет представлять из себя n-1 (где n - количество значимых символов),
        * поэтому формула нахождения суммы целых частей коэффициэнтов
        * будет выглядеть следующим образом: ((n-1)*((n-1)+1))/2
        * б) добавление к нему суммы дробных частей (каждая из которых для каждого символа всегда будет 0.5),
        * которое может быть выражено как умножение 0.5 на n (количество значимых символов в строке).
        * После сложения суммы целых частей и суммы дробных частей получившийся результат умножается на количество значимых символов.
        */
        static double CountPetrenkoIndex(string processedText)
        {
            int count = 0;
            double indexPetrenko = 0.0;
            try
            {
                count = Regex.Matches(processedText.ToString(), @"[абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz01234567890]", RegexOptions.IgnoreCase).Count;

                indexPetrenko = ((((count - 1) * ((count - 1) + 1)) / 2) + (count * 0.5)) * count;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message.ToString());
                return -1;
            }
            
            return indexPetrenko;
        }

        /*
         * Подсчёт индекса Петренко для русской строки.
         */
        static RussianString CountRussianStringIndex(string processedText)
        {
            double count = CountPetrenkoIndex(processedText);
            return new RussianString(processedText, count);
        }

        /*
         * Подсчёт индекса Петренко для английской строки.
         */
        static EnglishString CountEnglishStringIndex(string processedText)
        {
            try
            {
                string[] textSplitArray = processedText.Split(new char[] { '|' });
                string textMain = textSplitArray[0];
                string textComment = textSplitArray[1];
                double textMainIndex = CountPetrenkoIndex(textMain);
                double textCommentIndex = CountPetrenkoIndex(textComment);

                return new EnglishString(textMain, textComment, textMainIndex, textCommentIndex);
            }
            catch(Exception e)
            {
                return new EnglishString(processedText, "", CountPetrenkoIndex(processedText), 0);
            }

        }

        /*
         * Добавление русской строки в список.
         */
        public static int AddRussianStringToDictionary(string text)
        {
            _russianStrings.Add(_russianStrings.Count+1,CountRussianStringIndex(text));
            return 1;
        }

        /*
         * Добавление английской строки в список.
         */
        public static int AddEglishStringToDictionary(string text)
        {
            _englishStrings.Add(_englishStrings.Count + 1, CountEnglishStringIndex(text));
            return 1;
        }


        /*
         * Нахождение всех английских строк с суммами индексов для текста и комментария, совпадающими с индексом Петренко русской строки.
         */
        static List<int> FindEqualEngIndexes(RussianString russianProcessedText)
        {
            List<int> matchingEnglishStringIndexes = new List<int>();
            foreach (KeyValuePair<int, EnglishString> engstring in _englishStrings)
            {
                if (russianProcessedText.IndexPetrenko == (engstring.Value.TextMainIndexPetrenko + engstring.Value.TextCommentIndexPetrenko))
                {
                    matchingEnglishStringIndexes.Add(engstring.Key);
                }

            }
            return matchingEnglishStringIndexes;
        }


        /*
         * Заполнение списка соответствия номеров английских строк номерам русских строк так, 
         * чтобы сумма индексов Петренко текста и комментария английских строк совпадала
         * с индексом Петренко русской строки.
         */
        public static int FindEqualEngPetrenkoIndexStringsForAllRussianStrings()
        {
            _engStringsWithSamePetrenkoIndexes.Clear();
            List<int> engStrings = new List<int>();
            foreach (KeyValuePair<int, RussianString> russtring in _russianStrings)
            {
                engStrings = FindEqualEngIndexes(russtring.Value);
                _engStringsWithSamePetrenkoIndexes.TryAdd(russtring.Key, engStrings);
                Debug.WriteLine("elems in list " + engStrings.Count.ToString());
            }
            return 1;
        }

    }
}
