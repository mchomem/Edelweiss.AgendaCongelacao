using System;
using System.Text.RegularExpressions;

namespace Edelweiss.Utils
{
    public static class Text
    {
        public static String OnlyNumbers(String text)
        {
            String regexPattern = "[^0-9.]";
            return Regex.Replace(text, regexPattern, String.Empty);
        }

        public static Boolean IsOnlyNumbers(String text)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return regex.IsMatch(text);
        }
    }
}
