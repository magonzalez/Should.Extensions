using NUnit.Framework;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.AssertExtensions
{
    [TestFixture]
    public class HasSameSharedPropertyValuesTests
    {
        [Test]
        public void ShouldReturnTrueWhenActualHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelOne>();
            var expected = actual.ToTestModelTwo();

            var result = actual.HasSameSharedPropertyValues(expected);

            result.ShouldBeTrue();
        }

        [Test]
        public void ShouldReturnTrueWhenExpectedHasAdditionalProperties()
        {
            var actual = TestModelBuilder.Build<TestModelTwo>();
            var expected = actual.ToTestModelOne();

            var result = actual.HasSameSharedPropertyValues(expected);

            result.ShouldBeTrue();
        }
    }
}
