using NUnit.Framework;
using Should.Core.Exceptions;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.ShouldExtensions
{
    [TestFixture]
    public class ShouldHaveSamePropertiesTests
    {
        [Test]
        public void ShouldHandleNullActualValue()
        {
            var expected = TestModelBuilder.Build<TestModelOne>();

            Assert.DoesNotThrow(() => ((TestModelOne)null).ShouldHaveSameProperties(expected));
        }

        [Test]
        public void ShouldHandleNullExpectedValue()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();

            Assert.DoesNotThrow(() => actual.ShouldHaveSameProperties((TestModelOne)null));
        }

        [Test]
        public void ShouldHandleNullActualAndExpectedValue()
        {
            Assert.DoesNotThrow(() => ((TestModelOne)null).ShouldHaveSameProperties((TestModelOne)null));
        }

        [Test]
        public void ShouldNotThrowForSameTypes()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = TestModelBuilder.Build<TestModelOne>();

            Assert.DoesNotThrow(() => actual.ShouldHaveSameProperties(expected));
        }

        [Test]
        public void ShouldNotThrowForTypesWithSameProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = TestModelBuilder.Build<TestModelThree>();

            Assert.DoesNotThrow(() => actual.ShouldHaveSameProperties(expected));
        }

        [Test]
        public void ShouldThrowForTypesWithDifferentProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = TestModelBuilder.Build<TestModelTwo>();

            Assert.Throws<AssertException>(() => actual.ShouldHaveSameProperties(expected));
        }
    }
}
