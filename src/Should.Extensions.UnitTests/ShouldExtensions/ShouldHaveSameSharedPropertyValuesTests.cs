using NUnit.Framework;

using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.ShouldExtensions.ShouldHaveSamePropertyValueTests
{
    [TestFixture]
    public class ShouldHaveSameSharedPropertyValuesTests
    {
        [Test]
        public void ShouldNotThrowAssertExceptionWhenActualHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo();

            Assert.DoesNotThrow(() => actual.ShouldHaveSameSharedPropertyValues(expected));
        }

        [Test]
        public void ShouldNotThrowAssertExceptionWhenExpectedHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = actual.ToTestModelOne();

            Assert.DoesNotThrow(() => actual.ShouldHaveSameSharedPropertyValues(expected));
        }
    }
}
