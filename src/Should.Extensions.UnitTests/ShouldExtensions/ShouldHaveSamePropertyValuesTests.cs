using NUnit.Framework;
using Should.Core.Exceptions;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.ShouldExtensions.ShouldHaveSamePropertyValueTests
{
    [TestFixture]
    public class ShouldHaveSamePropertyValuesTests
    {
        [Test]
        public void ShouldThrowAssertExceptionWhenActualHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo();

            Assert.Throws<AssertException>(() => actual.ShouldHaveSamePropertyValues(expected));
        }

        [Test]
        public void ShouldThrowAssertExceptionWhenExpectedHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = actual.ToTestModelOne();

            Assert.Throws<AssertException>(() => actual.ShouldHaveSamePropertyValues(expected));
        }

        [Test]
        public void ShouldThrowWhenValuesAreDifferent()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo()
                .ToTestModelOne();

            Assert.Throws<EqualException>(() => actual.ShouldHaveSamePropertyValues(expected));
        }

        [Test]
        public void ShouldNotThrowWhenValuesAreTheSame()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.Clone();

            Assert.DoesNotThrow(() => actual.ShouldHaveSamePropertyValues(expected));
        }
    }
}
