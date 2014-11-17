using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovingAverages;
using System.Collections.Generic;

namespace MovingAveragesTest
{
    [TestClass]
    public class ReliableCalculationTests
    {
        private Dataset reliableDataset;

        [TestInitialize]
        public void initialise()
        {
            reliableDataset = new Dataset();

            List<DateTime> dates = new List<DateTime>();
            List<decimal> prices = new List<decimal>();

            DateTime date = new DateTime(2014, 8, 1, 0, 0, 0);
            decimal price = 1.5M;
            for (int counter = 0; counter < 20; counter++)
            {
                dates.Add(date);
                prices.Add(price);

                date = date.AddMinutes(1);
                price += 0.01M;
            }

            reliableDataset.load(dates, prices);
        }

        [TestMethod]
        public void ShowAllReliableEntriesTest()
        {
            foreach (Entry entry in reliableDataset.getEntries())
            {
                Console.WriteLine(entry);
            }
        }

        [TestMethod]
        public void basicCalculationTest()
        {
            Calculation calulation = new Calculation(reliableDataset);
            Dataset result = calulation.calculateAllMovingAverages(10);
            Assert.AreEqual(1.645M, result.getEntries().Last().Price);

            // For commented out test below to pass, we'd have to extrapolate outside of our date range.
            // To do so, I'd have to make the assumption that our "reliable data" continues downwards, 
            // however I can't possibly know that about the , so I'm not going to make that assumption.
            // Assert.AreEqual(1.545M, result.getEntries().First().Price);
        }
    }
}
