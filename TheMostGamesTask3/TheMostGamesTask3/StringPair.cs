using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMostGamesTask3
{
    /*
     * Класс для вывода совпадений строк.
     */
    class StringPair
    {
        string _rus; //Русская строка
        string _eng; //Одна из соответствующих ей английских строк
        public StringPair(string rus, string eng)
        {
            _rus = rus;
            _eng = eng;
        }

        public string Rus
        {
            get
            {
                return _rus;
            }
        }

        public string Eng
        {
            get
            {
                return _eng;
            }
        }
    }
}
