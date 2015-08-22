using System;

using NUnit.Framework;
using Should.Core.Exceptions;

namespace Should.Extensions.UnitTests.ShouldExtensions
{
    [TestFixture]
    public class ShouldEqualDateTimeTests
    {
        [TestCase("2015/08/01", "2015/08/01")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/01T00:00:00-5:00")]
        public void ShouldNotThrowWhenDatesAreEqual(string strActual, string strExpected)
        {
            var expected = DateTime.Parse(strExpected);
            var actual = DateTime.Parse(strActual);

            Assert.DoesNotThrow(() => actual.ShouldEqualDateTime(expected));
        }

        [TestCase("2015/08/01", "2015/08/02")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/01T01:00:00-5:00")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/01T00:01:00-5:00")]
        [TestCase("2015/08/01T00:00:01-5:00", "2015/08/01T00:00:00-5:00")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/02T00:00:00-5:00")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/09/02T00:00:00-5:00")]
        [TestCase("2015/08/01T00:00:00-5:00", "2016/08/02T00:00:00-5:00")]
        public void ShouldThrowWhenDatesAreEqual(string strActual, string strExpected)
        {
            var expected = DateTime.Parse(strExpected);
            var actual = DateTime.Parse(strActual);

            Assert.Throws<EqualException>(() => actual.ShouldEqualDateTime(expected));
        }

        [Test]
        public void ShouldNotThrowWhenActualIsOffBySeconds()
        {
            var expected = DateTime.Parse("2015/08/01T00:00:01-5:00");
            var actual = DateTime.Parse("2015/08/01T00:00:00-5:00");

            Assert.DoesNotThrow(() => actual.ShouldEqualDateTime(expected));
        }
    }
}
