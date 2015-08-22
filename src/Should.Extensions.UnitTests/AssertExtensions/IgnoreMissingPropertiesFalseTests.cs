using NUnit.Framework;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.AssertExtensions
{
    [TestFixture]
    public class IgnoreMissingPropertiesFalseTests
    {
        [Test]
        public void ShouldReturnFalseWhenActualHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo();

            var result = actual.HasSamePropertyValues(expected, false);

            result.ShouldBeFalse();
        }

        [Test]
        public void ShouldReturnFalseWhenExpectedHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = actual.ToTestModelOne();

            var result = actual.HasSamePropertyValues(expected, false);

            result.ShouldBeFalse();
        }
    }
}
