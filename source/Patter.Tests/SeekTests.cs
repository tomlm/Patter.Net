using Patter.Operations;

namespace Patter.Tests
{
    [TestClass]
    public class SeekTests
    {
        [TestMethod]
        public void SeekTest_NoCase()
        {
            var pat = new Patter<string>()
                .Seek("foo")
                .CaptureUntil("bar");

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual("foo not ", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual("FOO not ", pat.Match("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekTest_Case()
        {
            var pat = new Patter<string>()
                .Seek("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual("foo not ", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual("foo not ", pat.Match("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void SeekChars_Test()
        {
            var pat = new Patter<int>()
                .SeekChars(Chars.Digits)
                .CaptureUntilChars(Chars.Letters, (context) => context.Match = int.Parse(context.MatchText));

            Assert.AreEqual(0, pat.Match("bar").Count());
            Assert.AreEqual(123, pat.Match("this is 123 not 456 asdfasdf").First());
            Assert.AreEqual(456, pat.Match("this is 123 not 456 asdfasdf").Skip(1).First());
        }

        [TestMethod]
        public void CaptureChars_Test()
        {
            var pat = new Patter<int>()
                .SeekChars(Chars.Digits)
                .CaptureChars(Chars.Digits, (context) => context.Match = int.Parse(context.MatchText));

            Assert.AreEqual(0, pat.Match("bar").Count());
            Assert.AreEqual(123, pat.Match("this is 123 not 456").First());
            Assert.AreEqual(456, pat.Match("this is 123 not 456").Skip(1).First());
        }


        [TestMethod]
        public void SkipTest_NoCase()
        {
            var pat = new Patter<string>()
                .Seek("foo")
                .Skip("foo")
                .CaptureUntil("bar");

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SkipTest_Case()
        {
            var pat = new Patter<string>()
                .Seek("foo", StringComparison.Ordinal)
                .Skip("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void SeekAndSkipTest_NoCase()
        {
            var pat = new Patter<string>()
                .SeekAndSkip("foo")
                .CaptureUntil("bar");

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekAndSkipTest_Case()
        {
            var pat = new Patter<string>()
                .SeekAndSkip("foo", StringComparison.Ordinal)
                .CaptureUntil("bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual(" not ", pat.Match("this is a foo not bar").FirstOrDefault());
        }

        [TestMethod]
        public void ComplexType()
        {
            var pat = new Patter<ALink>()
                .Seek("<a")
                .SeekAndSkip("href=")
                .SkipChars(Chars.Quotes)
                .CaptureToChars(">'\"", (context) => context.Match.Href = context.MatchText.Trim())
                .SkipChars(Chars.Quotes)
                .SeekAndSkip(">")
                .CaptureUntil("</a", (context) => context.Match.Text = context.MatchText.Trim());

            Assert.IsNull(pat.Match("this is a test").FirstOrDefault());
            var match = pat.Match("this is a <a href=http://foo.com>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);
            match = pat.Match("this is a <a href='http://foo.com'>link</a>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);
            match = pat.Match("this is a <a href=\"http://foo.com\">link</a:>").FirstOrDefault();
            Assert.AreEqual("http://foo.com", match.Href);
            Assert.AreEqual("link", match.Text);

            var matches = pat.Match("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
            Assert.AreEqual("http://foo.com", matches.First().Href);
            Assert.AreEqual("link1", matches.First().Text);
            Assert.AreEqual("http://bar.com", matches.Skip(1).First().Href);
            Assert.AreEqual("link2", matches.Skip(1).First().Text);

        }

        public class ALink
        {
            public string Text { get; set; }
            public string Href { get; set; }
        }
    }
}