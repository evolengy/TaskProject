using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Services.StringBuilder
{
    public static class StringBuilder
    {
        /// <summary>
        /// Обрезать строку
        /// </summary>
        /// <param name="curString">Строка</param>
        /// <param name="checkCount">Проверка на количество символов</param>
        /// <param name="cutCount">Количество вырезанных символов</param>
        /// <returns></returns>
        public static string CutString(string curString, int checkCount, int cutCount)
        {
            string cutString = "";

            if (curString.Length <= checkCount)
            {
                cutString = curString;
            }
            else
            {
                cutString = curString.Substring(0, cutCount) + "..";
            }
            return cutString;
        }
    }
}
