using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amadeus;

namespace Amadeus.Tests
{
    [TestClass]
    public class AmadeusTests
    {
        Game game;

        [TestInitialize]
        public void SetUp() {
            game = new Game();
            game.PlanetCount = 34;
            game.EdgeCount = 59;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }
    }
}
