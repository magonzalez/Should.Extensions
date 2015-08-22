using FizzWare.NBuilder;
using FizzWare.NBuilder.PropertyNaming;
using NUnit.Framework;

namespace Should.Extensions.UnitTests
{
    [SetUpFixture]
    public class UnitTestSetupFixture
    {
        [SetUp]
        public void SetUp()
        {
            BuilderSetup.SetDefaultPropertyNamer(new RandomValuePropertyNamer());
        }
    }
}
