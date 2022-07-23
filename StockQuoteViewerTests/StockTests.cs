using NUnit.Framework;

namespace StockQuoteViewer.Tests
{
    [TestFixture()]
    public class StockTests
    {
        [Test()]
        public void LimitUpTest()
        {
            var stock = new Stock("0050", 20.60m);

            Assert.AreEqual(22.65m, stock.LimitUp);
        }

        [Test()]
        public void LimitDownTest()
        {
            var stock = new Stock("0050", 20.60m);

            Assert.AreEqual(18.55m, stock.LimitDown);
        }
    }
}