using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Lyrilang
{
    public class UnicodeDetectorGenerator
    {
        public Assembly CompiledAssembly { get; private set; }

        public UnicodeDetectorGenerator(String generatedFileName)
        {
            //todo: if generatedFileName is not a valid filename
            Dictionary<String, String> supportedScriptList = new Dictionary<String, String>();

            supportedScriptList.Add("Sinhala", "0x0D80 | 0x0DFF");
            supportedScriptList.Add("Tamil", "0x0B80 | 0x0BFF");
            supportedScriptList.Add("Devanagari", "0x0900 | 0x097F");
            supportedScriptList.Add("Arabic", "0x0600 | 0x06FF");
            supportedScriptList.Add("Chinese", "0x4E00 | 0x9FFF");
            supportedScriptList.Add("Korean", "0xAC00 | 0xD7AF");
            supportedScriptList.Add("Thai", "0x0E00 | 0x0E7F");
            supportedScriptList.Add("Hiragana", "0x3040 | 0x309F");
            supportedScriptList.Add("Katakana", "0x30A0 | 0x30FF");

            genFile(supportedScriptList, generatedFileName);
        }

        private void genFile(Dictionary<String, String> supportedScriptList, String generatedFileName) {
            string code = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrilang
{
    internal class UnicodeDetector
    {
        public enum supportedUnicodeLanguages
        {";
            foreach (var keyValue in supportedScriptList.Keys)
            {
                code = code + "\n\t\t\t" + "script" + keyValue + ",";
            }
            code = code + "\n\t\t\t" + "UNICODE_FORMAT_NOT_FOUND" + ",";

            code = code + @"
        }

        public supportedUnicodeLanguages checkForUnicode(String sourceLyrics)
        {";
            foreach (var keyValue in supportedScriptList.Keys)
            {
                code = code + "\n\t\t\tif (contain" + keyValue + "Unicode(sourceLyrics)) return supportedUnicodeLanguages.script" + keyValue + ";";
            }
            code = code + "\n\n\t\t\treturn supportedUnicodeLanguages.UNICODE_FORMAT_NOT_FOUND;";

            code = code + @"
        }";

            foreach (var dictItems in supportedScriptList)
            {
                String[] unicodeRangeSplit = dictItems.Value.Split(" | ");
                code = code + "\n\n\t\tpublic bool contain" + dictItems.Key + "Unicode(String sourceString)\n\t\t{";
                code = code + "\n\t\t\treturn sourceString.Any(c => c >= " + unicodeRangeSplit[0] + " && c <= " + unicodeRangeSplit[1] + ");\n\t\t}";
            }

            code = code + "\n\t}\n}";

            File.WriteAllText(generatedFileName, code);
            Console.WriteLine("Genearted file successfully : " + generatedFileName);

            // compile
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            // Add System.Runtime reference
            var systemRuntimePath = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Runtime.dll");
            if (File.Exists(systemRuntimePath))
            {
                references.Add(MetadataReference.CreateFromFile(systemRuntimePath));
            }

            // Add additional core references
            var coreLibPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var additionalRefs = new[]
            {
                "System.Collections.dll",
                "System.Linq.dll",
                "System.Runtime.Extensions.dll"
            };

            foreach (var refName in additionalRefs)
            {
                var refPath = Path.Combine(coreLibPath, refName);
                if (File.Exists(refPath))
                {
                    references.Add(MetadataReference.CreateFromFile(refPath));
                }
            }

            var compilation = CSharpCompilation.Create("UnicodeDetectorAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            // Load the compiled assembly
            ms.Seek(0, SeekOrigin.Begin);
            CompiledAssembly = Assembly.Load(ms.ToArray());
            Console.WriteLine("UnicodeDetector compiled successfully!");
        }

        public object CreateUnicodeDetectorInstance()
        {
            if (CompiledAssembly == null)
            {
                throw new InvalidOperationException("Assembly not compiled yet!");
            }

            Type unicodeDetectorType = CompiledAssembly.GetType("Lyrilang.UnicodeDetector");
            return Activator.CreateInstance(unicodeDetectorType);
        }
    }
}
