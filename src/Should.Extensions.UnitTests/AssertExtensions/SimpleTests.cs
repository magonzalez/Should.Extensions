using NUnit.Framework;
using Should.Extensions.UnitTests.Models;

namespace Should.Extensions.UnitTests.AssertExtensions
{
    [TestFixture]
    public class SimpleTests
    {
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
