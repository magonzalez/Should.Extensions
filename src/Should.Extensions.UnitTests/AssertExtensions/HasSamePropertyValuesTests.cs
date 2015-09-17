using NUnit.Framework;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.AssertExtensions
{
    [TestFixture]
    public class HasSamePropertyValuesTests
    {
        [Test]
        public void ShouldReturnFalseWhenActualHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo();

            var result = actual.HasSamePropertyValues(expected);

            result.ShouldBeFalse();
        }

        [Test]
        public void ShouldReturnFalseWhenExpectedHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = actual.ToTestModelOne();

            var result = actual.HasSamePropertyValues(expected);

            result.ShouldBeFalse();
        }

        [Test]
        public void ShouldReturnFalseWhenValuesAreDifferent()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo()
                .ToTestModelOne();

            var result = actual.HasSamePropertyValues(expected);

            result.ShouldBeFalse();
        }

        [Test]
        public void ShouldReturnTrueWhenValuesAreTheSame()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.Clone();

            var result = actual.HasSamePropertyValues(expected);

            result.ShouldBeTrue();
        }
    }
}
