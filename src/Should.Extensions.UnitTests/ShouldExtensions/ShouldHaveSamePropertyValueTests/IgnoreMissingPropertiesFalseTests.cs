using NUnit.Framework;
using Should.Core.Exceptions;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.ShouldExtensions.ShouldHaveSamePropertyValueTests
{
    [TestFixture]
    public class IgnoreMissingPropertiesFalseTests
    {
        [Test]
        public void ShouldThrowAssertExceptionWhenActualHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo();

            Assert.Throws<AssertException>(() => actual.ShouldHaveSamePropertyValues(expected, false));
        }

        [Test]
        public void ShouldThrowAssertExceptionWhenExpectedHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = actual.ToTestModelOne();

            Assert.Throws<AssertException>(() => actual.ShouldHaveSamePropertyValues(expected, false));
        }
    }
}
