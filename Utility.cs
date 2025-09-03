using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lyrilang
{
    internal class Utility
    {
        public static String prepSourceString(String titleName, String albumName, String songLyrics)
        {
            String regexPattern = @"\[[^\]]*\]";
            String regexPattern2 = @"\([^)]*\)";

            songLyrics = Regex.Replace(songLyrics, regexPattern, "");
            songLyrics = Regex.Replace(songLyrics, regexPattern2, "");
            songLyrics = songLyrics
                .Replace(",", "")
                .Replace("?", "")
                .Replace("\"", "");

            return titleName + " " + albumName + " " + songLyrics;
        }

        public static String[] sourceStringTokenizer(String sourceString)
        {
            char[] delimeterList = { ' ', '\t', '\n' };
            String[] tokenizedSource = sourceString.ToLower().Split(delimeterList, StringSplitOptions.RemoveEmptyEntries);

            return tokenizedSource;
        }

        public static bool IsEnumValue(object enumValue, Type enumType, string valueName)
        {
            var targetValue = Enum.Parse(enumType, valueName);
            return enumValue.Equals(targetValue);
        }
    }
}
