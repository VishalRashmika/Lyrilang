using System.Xml.Resolvers;
using static System.Net.Mime.MediaTypeNames;

namespace Lyrilang
{
    public class LyrilangEngine
    {
        public LyrilangEngine(String titleName, String albumName, String songLyrics)
        {
            String sourceString = prepSourceString(titleName, albumName, songLyrics);

            if (checkForSinhalaUnicode(sourceString))
            {
                Console.WriteLine("Found: Sinhalese Unicode");
            }
            else
            {
                Console.WriteLine("Not found: Sinhalese Unicode");
            }

            // todo: confirm unicode first
            String[] tokenizedSource = sourceStringTokenizer(sourceString);
            if (containSinhalaWords(tokenizedSource))
            {
                Console.WriteLine("Found: Sinhala Words");
            }
            else { 
                Console.WriteLine("Not found: Sinhala words");
            }

        }

        private static String prepSourceString(String titleName, String albumName, String songLyrics) {
            // todo: handle empty values
            return titleName + " " + albumName + " " + songLyrics;
        }

        private static String[] sourceStringTokenizer(String sourceString)
        {
            char[] delimeterList = { ' ', '\t', '\n' };
            String[] tokenizedSource = sourceString.ToLower().Split(delimeterList, StringSplitOptions.RemoveEmptyEntries);

            return tokenizedSource;
        }

        private bool checkForSinhalaUnicode(String sourceString)
        {
            bool sinhalaUnicode = sourceString.Any(c => c >= 0x0D80 && c <= 0x0DFF);
            return sinhalaUnicode;
        }

        private static bool containSinhalaWords(String[] tokenizedSource) {
            // get and load hashset from external source
            HashSet<String> sinhalaWords = new HashSet<String> 
            {
                "ayubowan", "kohomada", "mata", "mama", "oba", "api", "meka", "eka", "dan", "kiyala"
            };
            bool hasSinhalaWords = tokenizedSource.Any(word => sinhalaWords.Contains(word));
            return hasSinhalaWords;
        }

   
    }
}


/*
 class Program
{
    static void Main()
    {
        string text = "Hello, I said ayubowan to my friend";
        var lowerText = text.ToLower();

        // Define Sinhala pattern words in a HashSet
        var sinhalaPatterns = new HashSet<string>
        {
            "ayubowan", "kohomada", "mata", "mama", "oba", "api", "meka", "eka", "dan", "kiyala"
        };

        // Split input text into words
        var words = lowerText.Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        // Check if any word in the text is in the HashSet
        bool hasSinhalaScript = words.Any(word => sinhalaPatterns.Contains(word));

        Console.WriteLine(hasSinhalaScript); // Output: True
    }
}
 */