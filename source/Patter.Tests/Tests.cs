using Patter.Operations;

namespace Patter.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void SeekTest_NoCase()
        {
            var pat = new Patter<string>()
                .Seek("foo")
                .CaptureUntil("bar");

            Assert.IsNull(pat.Matches("bar").FirstOrDefault());
            Assert.AreEqual("foo not ", pat.Matches("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual("FOO not ", pat.Matches("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekTest_Case()
        {
            var pat = new Patter<string>()
                .Seek("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Matches("bar").FirstOrDefault());
            Assert.AreEqual("foo not ", pat.Matches("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Matches("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Matches("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual("foo not ", pat.Matches("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void SeekChars_Test()
        {
            var pat = new Patter<int>()
                .SeekChars(Chars.Digits)
                .CaptureUntilChars(Chars.Letters, (context) => context.Match = int.Parse(context.MatchText));

            Assert.AreEqual(0, pat.Matches("bar").Count());
            Assert.AreEqual(123, pat.Matches("this is 123 not 456 asdfasdf").First());
            Assert.AreEqual(456, pat.Matches("this is 123 not 456 asdfasdf").Skip(1).First());
        }

        [TestMethod]
        public void CaptureUntil_Test()
        {
            var pat = new Patter<string>()
                .SeekChars("e")
                .CaptureUntil("lm");

            Assert.AreEqual("efghijk", pat.Matches("abcdefghijklmnopqrstuvwxyz").First());
        }

        [TestMethod]
        public void CapturePast_Test()
        {
            var pat = new Patter<string>()
                .SeekChars("e")
                .CaptureUntilPast("lm");
            Assert.AreEqual("efghijklm", pat.Matches("abcdefghijklmnopqrstuvwxyz").First());
        }


        [TestMethod]
        public void CaptureChars_Test()
        {
            var pat = new Patter<int>()
                .SeekChars(Chars.Digits)
                .CaptureChars(Chars.Digits, (context) => context.Match = int.Parse(context.MatchText));

            Assert.AreEqual(0, pat.Matches("bar").Count());
            Assert.AreEqual(123, pat.Matches("this is 123 not 456").First());
            Assert.AreEqual(456, pat.Matches("this is 123 not 456").Skip(1).First());
        }

        [TestMethod]
        public void CaptureUntilChars_Test()
        {
            var pat = new Patter<string>()
                .SeekChars("b")
                .CaptureUntilChars("f");

            Assert.AreEqual("bcde", pat.Matches("abcdefg").First());
        }

        [TestMethod]
        public void CaptureUntilPastChars_Test()
        {
            var pat = new Patter<string>()
                .SeekChars("b")
                .CaptureUntilPastChars("f");

            Assert.AreEqual("bcdef", pat.Matches("abcdefg").First());
        }

        [TestMethod]
        public void SkipTest_NoCase()
        {
            var pat = new Patter<string>()
                .Seek("foo")
                .Skip("foo")
                .CaptureUntil("bar");

            Assert.IsNull(pat.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SkipTest_Case()
        {
            var pat = new Patter<string>()
                .Seek("foo", StringComparison.Ordinal)
                .Skip("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Matches("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Matches("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void SeekPastTest_NoCase()
        {
            var pat = new Patter<string>()
                .SeekPast("foo")
                .CaptureUntil("bar");

            Assert.IsNull(pat.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekPastTest_Case()
        {
            var pat = new Patter<string>()
                .SeekPast("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Matches("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Matches("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Matches("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Matches("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void ComplexType()
        {
            var pat = new Patter<ALink>()
                .Seek("<a")
                .SeekPast("href=")
                .SkipChars(Chars.Quotes)
                .CaptureUntilChars(">'\"", (context) => context.Match.Href = context.MatchText.Trim())
                .SkipChars(Chars.Quotes)
                .SeekPast(">")
                .CaptureUntil("</a", (context) => context.Match.Text = context.MatchText.Trim());

            Assert.IsNull(pat.Matches("this is a test").FirstOrDefault());
            var match = pat.Matches("this is a <a href=http://foo.com>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);
            match = pat.Matches("this is a <a href='http://foo.com'>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);
            match = pat.Matches("this is a <a href=\"http://foo.com\">link</a:>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);

            var matches = pat.Matches("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
            Assert.AreEqual("http://foo.com", matches.First().Href);
            Assert.AreEqual("link1", matches.First().Text);
            Assert.AreEqual("http://bar.com", matches.Skip(1).First().Href);
            Assert.AreEqual("link2", matches.Skip(1).First().Text);

        }

        [TestMethod]
        public void StringCapture()
        {
            var pat = new Patter<string>()
             .Seek("<a")
             .SeekPast("href=")
             .SkipChars(Chars.Quotes)
             .CaptureUntilChars(">'\"")
             .SkipChars(Chars.Quotes)
             .SeekPast(">");

            Assert.IsNull(pat.Matches("this is a test").FirstOrDefault());
            var match = pat.Matches("this is a <a href=http://foo.com>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match);
            match = pat.Matches("this is a <a href='http://foo.com'>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match);
            match = pat.Matches("this is a <a href=\"http://foo.com\">link</a:>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match);

            var matches = pat.Matches("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
            Assert.AreEqual("http://foo.com", matches.First());
            Assert.AreEqual("http://bar.com", matches.Skip(1).First());
        }

        [TestMethod]
        public void CustomTest()
        {
            // extract a pattern of odd
            var pat = new Patter<bool>()
                .Custom((context) =>
                {
                    context.Match = ((int)context.CurrentChar) % 2 == 1;
                    context.MatchText = context.CurrentChar.ToString();
                    if (context.Match)
                        context.HasMatch = true;
                    context.Pos++;
                });

            var results = pat.Matches("abcde").ToList();
            Assert.AreEqual(3, results.Count);
        }

        public class ALink
        {
            public string Text { get; set; }
            public string Href { get; set; }
        }
    }
}