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
                .CaptureValue((context, text) => context.CurrentMatch = text.Trim(), "bar");

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual("foo not", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.AreEqual("FOO not", pat.Match("this is a FOO not BAR").FirstOrDefault());
        }

        [TestMethod]
        public void SeekTest_Case()
        {
            var pat = new Patter<string>()
                .Seek("foo", StringComparison.Ordinal)
                .CaptureValue((context, text) => context.CurrentMatch = text.Trim(), "bar", StringComparison.Ordinal);

            Assert.IsNull(pat.Match("bar").FirstOrDefault());
            Assert.AreEqual("foo not", pat.Match("this is a foo not bar").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a FOO not BAR").FirstOrDefault());
            Assert.IsNull(pat.Match("this is a foo not BAR").FirstOrDefault());
            Assert.AreEqual("foo not", pat.Match("this is a foo not bar").FirstOrDefault());
        }
    }
}