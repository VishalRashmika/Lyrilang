using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lyrilang
{
    internal class UnicodeResolution
    {
        public static IList detectedUnicodeFormats;

        public static supportedLanguages resolveUnicode(String sourceString)
        {
            UnicodeDetectorGenerator detectorGenerator = new UnicodeDetectorGenerator("./GeneratedUnicodeDetector.cs");
            object detector = detectorGenerator.CreateUnicodeDetectorInstance();
            Type detectorType = detector.GetType();
            MethodInfo checkForUnicodeMethod = detectorType.GetMethod("checkForUnicode");
            Type enumType = detectorType.GetNestedType("supportedUnicodeLanguages");

            // Store the returned list
            detectedUnicodeFormats = (IList)checkForUnicodeMethod.Invoke(detector, new object[] { sourceString });

            if (ContainsEnumValue(enumType, "scriptSinhala"))
            {
                Console.WriteLine("Found: Sinhalese Unicode");
                return supportedLanguages.langSinhala;
            }

            // Japanese
            if ((ContainsEnumValue(enumType, "scriptHiragana") || ContainsEnumValue(enumType, "scriptKatakana")) && ContainsEnumValue(enumType, "scriptChinese"))
            {
                Console.WriteLine("Found: Japanese Unicode");
                return supportedLanguages.langJapanese;
            }
            if (ContainsEnumValue(enumType, "scriptHiragana") || ContainsEnumValue(enumType, "scriptKatakana"))
            {
                Console.WriteLine("Found: Pure Japanese Unicode");
                return supportedLanguages.langJapanese;
            }

            //chinese
            if (ContainsEnumValue(enumType, "scriptChinese"))
            {
                Console.WriteLine("Found: Chinese Unicode");
                return supportedLanguages.langChinese;
            }

            return supportedLanguages.LANGUAGE_NOT_SUPPORTED;
        }


        private static bool ContainsEnumValue(Type enumType, string enumValueName)
        {
            object enumValue = Enum.Parse(enumType, enumValueName);
            return detectedUnicodeFormats.Contains(enumValue);
        }

        public static IList DetectedUnicodeFormats
        {
            get { return detectedUnicodeFormats; }
        }

        public static List<T> GetDetectedFormatsAs<T>()
        {
            return detectedUnicodeFormats.Cast<T>().ToList();
        }

    }


}
