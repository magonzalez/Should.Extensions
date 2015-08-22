using System;
using NUnit.Framework;
using Should.Core.Exceptions;

namespace Should.Extensions.UnitTests.ShouldExtensions
{
    [TestFixture]
    public class ShouldEqualDateTests
    {
        [TestCase("2015/08/01", "2015/08/01")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/01T00:00:00-5:00")]
        [TestCase("2015/08/01T07:34:42-5:00", "2015/08/01T07:34:42-5:00")]
        public void ShouldNotThrowWhenDatesAreEqual(string strActual, string strExpected)
        {
            var actual = DateTime.Parse(strActual);
            var expected = DateTime.Parse(strExpected);

            Assert.DoesNotThrow(() => actual.ShouldEqualDate(expected));
        }

        [TestCase("2015/08/01", "2015/08/02")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/08/02T00:00:00-5:00")]
        [TestCase("2015/08/01T00:00:00-5:00", "2015/09/02T00:00:00-5:00")]
        [TestCase("2015/08/01T00:00:00-5:00", "2016/08/02T00:00:00-5:00")]
        public void ShouldThrowWhenDatesAreEqual(string strActual, string strExpected)
        {
            var actual = DateTime.Parse(strActual);
            var expected = DateTime.Parse(strExpected);

            Assert.Throws<EqualException>(() => actual.ShouldEqualDate(expected));
        }
    }
}
