using System.Xml.Resolvers;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System;
using System.Linq;
using System.Reflection;

namespace Lyrilang
{
    public enum supportedLanguages
    {
        langEnglish,
        langSinhala,
        langTamil,
        langDevanagari,
        langArabic,
        langChinese,
        langKorean,
        langThai,
        langJapanese,
        LANGUAGE_NOT_SUPPORTED
    }
    public class LyrilangEngine
    {
        public LyrilangEngine(String titleName, String albumName, String songLyrics)
        {

            // TODO: Check whether text contain unicode
             // if: run unicode if-not: tokenize


            //String[] tokenizedSource = Utility.sourceStringTokenizer(sourceString);

            //if (containSinhalaWords(tokenizedSource))
            //{
            //    Console.WriteLine("Found: Sinhala Words");
            //}

            Console.WriteLine("END OF EXECUTION");
        }

        public static String testFunc()
        {
            return "hello";
        }

        public static supportedLanguages detectLang(String titleName, String albumName, String songLyrics)
        {
            String sourceString = Utility.prepSourceString(titleName, albumName, songLyrics);
            return UnicodeResolution.resolveUnicode(sourceString);
        }

        private static bool containSinhalaWords(String[] tokenizedSource) {
            // todo:get and load hashset from external source
            HashSet<String> sinhalaWords = new HashSet<String> 
            {
                "ayubowan", "kohomada", "mata", "mama", "oba", "api", "meka", "eka", "dan", "kiyala"
            };
            bool hasSinhalaWords = tokenizedSource.Any(word => sinhalaWords.Contains(word));
            return hasSinhalaWords;
        }

    }
}