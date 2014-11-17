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
        protected Dataset reliableDataset;

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

            // FIXME Implement interpolation and extrapolation so that these tests pass.
            // Assert.AreEqual(1.545M, result.getEntries().First().Price);
            Assert.AreEqual(1.645M, result.getEntries().Last().Price);
        }

        //TODO Split this out into multiple tests.
        [TestMethod]
        public void checkBasicCases()
        {
            reliableDataset = new Dataset();
            reliableDataset.load("MovingAveragesData.csv");

            Calculation calculation = new Calculation(reliableDataset);
            Dataset results = calculation.calculateAllMovingAverages(10);

            // Nothing preceding
            Assert.AreEqual(1.33877M, results.getEntries().First().Price);

            // Evenly spaced data
            Assert.AreEqual(1.341999M, results.getEntries().ElementAt(2840).Price);

            // Evenly spaced and across 2 days
            Assert.AreEqual(1.342014M, results.getEntries().ElementAt(2842).Price);

            // Unevenly spaced data (i.e., 23:46, 23:48, 23:49...)
            Assert.AreEqual(1.34203125M, results.getEntries().ElementAt(2832).Price);
        }
    }
}
