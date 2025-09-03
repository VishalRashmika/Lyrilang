using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Lyrilang
{
    public class UnitTest
    {
        private readonly string lyrics = "\"The House Of The Rising Sun\" ...";
     
        [Theory]
        [InlineData("වන වගන්තිය සියලු මනුෂ්‍යයෝ නිදහස්ව උපත ලබා ඇත", supportedLanguages.langSinhala)]
        [InlineData("你好吗", supportedLanguages.langChinese)]
        [InlineData("トヨタ", supportedLanguages.langJapanese)]
        [InlineData("あいうえお", supportedLanguages.langJapanese)]
        [InlineData("今日は、日本語のユニコードテキス", supportedLanguages.langJapanese)]

        public void UnicodeTestCases(string input, supportedLanguages expected)
        {
            Assert.Equal(expected, LyrilangEngine.detectLang(input, "World", lyrics));
        }

    }
}
