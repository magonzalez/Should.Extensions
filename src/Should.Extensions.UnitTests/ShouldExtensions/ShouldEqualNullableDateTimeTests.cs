using System;
using NUnit.Framework;
using Should.Core.Exceptions;

namespace Should.Extensions.UnitTests.ShouldExtensions
{
    [TestFixture]
    public class ShouldEqualNullableDateTimeTests
    {
        [TestCase("2015/08/01", "2015/08/01")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/01T00:00:00-5:00")]
        public void ShouldNotThrowWhenDatesAreEqual(string strActual, string strExpected)
        {
            DateTime? expected = DateTime.Parse(strExpected);
            DateTime? actual = DateTime.Parse(strActual);

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
            DateTime? expected = DateTime.Parse(strExpected);
            DateTime? actual = DateTime.Parse(strActual);

            Assert.Throws<EqualException>(() => actual.ShouldEqualDateTime(expected));
        }

        [Test]
        public void ShouldNotThrowWhenActualIsOffBySeconds()
        {
            DateTime? expected = DateTime.Parse("2015/08/01T00:00:01-5:00");
            DateTime? actual = DateTime.Parse("2015/08/01T00:00:00-5:00");

            Assert.DoesNotThrow(() => actual.ShouldEqualDateTime(expected));
        }

        [Test]
        public void ShouldThrowWhenActualIsNullAndExpectedIsNotNull()
        {
            DateTime? expected = DateTime.Parse("2015/08/01T00:00:01-5:00");
            DateTime? actual = null;

            Assert.Throws<TrueException>(() => actual.ShouldEqualDateTime(expected));
        }

        [Test]
        public void ShouldThrowWhenActualIsNotNullAndExpectedIsNull()
        {
            DateTime? expected = null;
            DateTime? actual = DateTime.Parse("2015/08/01T00:00:01-5:00");

            Assert.Throws<FalseException>(() => actual.ShouldEqualDateTime(expected));
        }

        [Test]
        public void ShouldAllowNullValues()
        {
            DateTime? expected = null;
            DateTime? actual = null;

            Assert.DoesNotThrow(() => actual.ShouldEqualDateTime(expected));
        }
    }
}
