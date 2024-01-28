#pragma warning disable CS8602 // Dereference of a possibly null reference.
using Patter.Operations;

namespace Patter.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void SeekTest_NoCase()
        {
            var pattern = new PatternBuilder<string>()
                .Seek("foo")
                .CaptureUntil("bar")
                .Build();

            Assert.IsNull(pattern.Matches("bar").FirstOrDefault());
            Assert.AreEqual("foo not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual("FOO not ", pattern.Matches("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekTest_Case()
        {
            var pattern = new PatternBuilder<string>()
                .Seek("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal)
                .Build();

            Assert.IsNull(pattern.Matches("bar").FirstOrDefault());
            Assert.AreEqual("foo not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pattern.Matches("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pattern.Matches("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual("foo not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void SeekChars_Test()
        {
            var pattern = new PatternBuilder<int>()
                .Seek(Chars.Digits)
                .CaptureUntil(Chars.Letters, (context) => context.Match = int.Parse(context.MatchText!))
                .Build();

            Assert.AreEqual(0, pattern.Matches("bar").Count());
            Assert.AreEqual(123, pattern.Matches("this is 123 not 456 asdfasdf").First());
            Assert.AreEqual(456, pattern.Matches("this is 123 not 456 asdfasdf").Skip(1).First());
        }



        [TestMethod]
        public void CaptureUntil_Test()
        {
            var pattern = new PatternBuilder<string>()
                .Seek('e')
                .CaptureUntil("lm")
                .Build();

            Assert.AreEqual("efghijk", pattern.Matches("abcdefghijklmnopqrstuvwxyz").First());
        }

        [TestMethod]
        public void CapturePast_Test()
        {
            var pattern = new PatternBuilder<string>()
                .Seek('e')
                .CaptureUntilPast("lm")
                .Build();
            Assert.AreEqual("efghijklm", pattern.Matches("abcdefghijklmnopqrstuvwxyz").First());
        }


        [TestMethod]
        public void CaptureChars_Test()
        {
            var pattern = new PatternBuilder<int>()
                .Seek(Chars.Digits)
                .Capture(Chars.Digits, (context) => context.Match = int.Parse(context.MatchText!))
                .Build();

            Assert.AreEqual(0, pattern.Matches("bar").Count());
            Assert.AreEqual(123, pattern.Matches("this is 123 not 456").First());
            Assert.AreEqual(456, pattern.Matches("this is 123 not 456").Skip(1).First());
        }

        [TestMethod]
        public void CaptureUntilChars_Test()
        {
            var pattern = new PatternBuilder<string>()
                .Seek('b')
                .CaptureUntil("f".ToArray())
                .Build();

            Assert.AreEqual("bcde", pattern.Matches("abcdefg").First());
        }

        [TestMethod]
        public void CaptureUntilPastChars_Test()
        {
            var pattern = new PatternBuilder<string>()
                .Seek('b')
                .CaptureUntilPast("f".ToArray())
                .Build();

            Assert.AreEqual("bcdef", pattern.Matches("abcdefg").First());
        }

        [TestMethod]
        public void SkipTest_NoCase()
        {
            var pattern = new PatternBuilder<string>()
                .Seek("foo")
                .Skip("foo")
                .CaptureUntil("bar")
                .Build();

            Assert.IsNull(pattern.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SkipTest_Case()
        {
            var pattern = new PatternBuilder<string>()
                .Seek("foo", StringComparison.Ordinal)
                .Skip("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal)
                .Build();

            Assert.IsNull(pattern.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pattern.Matches("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pattern.Matches("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void SeekPastTest_NoCase()
        {
            var pattern = new PatternBuilder<string>()
                .SeekPast("foo")
                .CaptureUntil("bar")
                .Build();

            Assert.IsNull(pattern.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekPastTest_Case()
        {
            var pattern = new PatternBuilder<string>()
                .SeekPast("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal)
                .Build();

            Assert.IsNull(pattern.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pattern.Matches("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pattern.Matches("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual(" not ", pattern.Matches("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void ComplexType()
        {
            var pattern = new PatternBuilder<ALink>()
                .Seek("<a")
                .SeekPast("href=")
                .Skip(Chars.Quotes)
                .CaptureUntil(">'\"".ToArray(), (context) => context.Match.Href = context.MatchText.Trim())
                .Skip(Chars.Quotes)
                .SeekPast(">")
                .CaptureUntil("</a", (context) => context.Match.Text = context.MatchText.Trim())
                .Build();

            Assert.IsNull(pattern.Matches("this is a test").FirstOrDefault());
            var match = pattern.Matches("this is a <a href=http://foo.com>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);
            match = pattern.Matches("this is a <a href='http://foo.com'>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);
            match = pattern.Matches("this is a <a href=\"http://foo.com\">link</a:>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);

            var matches = pattern.Matches("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
            Assert.AreEqual("http://foo.com", matches.First().Href);
            Assert.AreEqual("link1", matches.First().Text);
            Assert.AreEqual("http://bar.com", matches.Skip(1).First().Href);
            Assert.AreEqual("link2", matches.Skip(1).First().Text);

        }

        [TestMethod]
        public void StringCapture()
        {
            var pattern = new PatternBuilder<string>()
             .Seek("<a")
             .SeekPast("href=")
             .Skip(Chars.Quotes)
             .CaptureUntil(">'\"".ToArray())
             .Skip(Chars.Quotes)
             .SeekPast(">")
             .Build();

            Assert.IsNull(pattern.Matches("this is a test").FirstOrDefault());
            var match = pattern.Matches("this is a <a href=http://foo.com>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match);
            match = pattern.Matches("this is a <a href='http://foo.com'>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match);
            match = pattern.Matches("this is a <a href=\"http://foo.com\">link</a:>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match);

            var matches = pattern.Matches("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
            Assert.AreEqual("http://foo.com", matches.First());
            Assert.AreEqual("http://bar.com", matches.Skip(1).First());
        }

        [TestMethod]
        public void CustomTest()
        {
            // extract a pattern of odd
            var pattern = new PatternBuilder<bool>()
                .Custom((context) =>
                {
                    context.Match = ((int)context.CurrentChar) % 2 == 1;
                    context.MatchText = context.CurrentChar.ToString();
                    if (context.Match)
                        context.HasMatch = true;
                    context.Pos++;
                })
                .Build();

            var results = pattern.Matches("abcde").ToList();
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void Complex_Test()
        {
            string output = @"4c:ed:fb:7c:a0:dc	5765	-66	[WPA2-PSK-CCMP][WPS][ESS]	Laird-McConnell_5G
86:ea:ed:b2:c3:87	5765	-47	[WPA2-PSK-CCMP][WPS][ESS][P2P]	DIRECT-roku-T35-64961B
4c:ed:fb:7c:a0:90	2432	-52	[WPA2-PSK-CCMP][WPS][ESS]	Laird-McConnell
4c:ed:fb:7c:a0:d8	2457	-62	[WPA2-PSK-CCMP][WPS][ESS]	Laird-McConnell
86:ea:ed:b2:c5:e5	5180	-84	[WPA2-PSK-CCMP][WPS][ESS][P2P]	DIRECT-roku-6F8-2F5B61
b6:95:75:36:91:b2	2427	-82	[WPA-PSK-CCMP+TKIP][WPA2-PSK-CCMP+TKIP][ESS]	CMesh_Guest
ba:36:bc:45:c1:b1	2462	-80	[WPA2-PSK-CCMP][WPS][ESS]	dk9
4c:ed:fb:7c:a0:94	5785	-70	[WPA2-PSK-CCMP][WPS][ESS]	Laird-McConnell_5G
6c:72:20:3e:b2:5d	2437	-82	[WPA-PSK-CCMP+TKIP][WPA2-PSK-CCMP+TKIP][ESS]	Culverhouse Guest
6c:72:20:3e:b2:5c	2437	-82	[WPA-PSK-CCMP+TKIP][WPA2-PSK-CCMP+TKIP][WPS][ESS]	Jaggar's Wifi Network
0a:a7:c0:3f:31:54	2437	-75	[WPA2-PSK-CCMP][ESS]	Culverhouse1000
";
            var pattern = new PatternBuilder<AccessPoint>()
                .CaptureUntil(Chars.Whitespace, (c) => c.Match.Bssid = c.MatchText!)
                .Skip(Chars.Whitespace)
                    .CaptureUntil(Chars.Whitespace, (c) => c.Match.Frequency = int.Parse(c.MatchText!))
                .Skip(Chars.Whitespace)
                    .CaptureUntil(Chars.Whitespace, (c) => c.Match.Signal = int.Parse(c.MatchText!))
                .SeekPast("[")
                    .CaptureUntil(Chars.Whitespace, (c) => c.Match.Flags = c.MatchText.TrimEnd(']').Split("][", StringSplitOptions.RemoveEmptyEntries).ToList())
                .Skip(Chars.Whitespace)
                    .CaptureUntil(Chars.EOL, (c) => c.Match.SSID = c.MatchText!)
                .Skip(Chars.EOL)
                .Build();

            var accessPoints = pattern.Matches(output).ToList(); 
            Assert.AreEqual(11, accessPoints.Count());
        }

        public class ALink
        {
            public string Text { get; set; } = String.Empty;
            public string Href { get; set; } = String.Empty;
        }

        public class AccessPoint
        {
            public string Bssid { get; set; } = String.Empty;
            public int Frequency { get; set; } 
            public int Signal { get; set; }
            public List<string> Flags { get; set; } = new List<string>();
            public string SSID { get; set; } = String.Empty;
        }

    }
}