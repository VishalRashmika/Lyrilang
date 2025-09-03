using System.Xml.Resolvers;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System;
using System.Linq;
using System.Reflection;

namespace Lyrilang
{
    public class LyrilangEngine
    {
        public LyrilangEngine(String titleName, String albumName, String songLyrics)
        {
            String sourceString = Utility.prepSourceString(titleName, albumName, songLyrics);
            
            // Unicode Detector meta-programming approach
            UnicodeDetectorGenerator detectorGenerator = new UnicodeDetectorGenerator("./GeneratedUnicodeDetector.cs");
            object detector = detectorGenerator.CreateUnicodeDetectorInstance();
            Type detectorType = detector.GetType();
            MethodInfo checkForUnicodeMethod = detectorType.GetMethod("checkForUnicode");
            Type enumType = detectorType.GetNestedType("supportedUnicodeLanguages");

            object result = checkForUnicodeMethod.Invoke(detector, new object[] { sourceString });

            // Resolve Unicode Types
            if (Utility.IsEnumValue(result, enumType, "scriptSinhala"))
            {
                Console.WriteLine("Found: Sinhalese Unicode");
            }
            if (Utility.IsEnumValue(result, enumType, "scriptChinese"))
            {
                Console.WriteLine("Found: Chinese Unicode");
            }
            if (Utility.IsEnumValue(result, enumType, "scriptHiragana") || Utility.IsEnumValue(result, enumType, "scriptKatakana"))
            {
                Console.WriteLine("Found: Japanese Unicode");
            }

            // todo: words
            String[] tokenizedSource = Utility.sourceStringTokenizer(sourceString);
            if (containSinhalaWords(tokenizedSource))
            {
                Console.WriteLine("Found: Sinhala Words");
            }

            Console.WriteLine("END OF EXECUTION");
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