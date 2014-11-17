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
        private const decimal TOLERANCE = 0.0000000000001m;
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
            Dataset results = calulation.calculateAllMovingAverages(10);
            Assert.AreEqual(1.645M, results.getEntries().Last().Price);

            // Requires extrapolation
            decimal expectedValue = 1.545M;
            decimal actualValue = results.getEntries().First().Price;
            decimal difference = Math.Abs(expectedValue - actualValue);
            Assert.IsTrue(difference <= TOLERANCE, "The difference between the expected and actual value was too great." + Environment.NewLine +
                                                            "The maximum allowed difference is: " + TOLERANCE + Environment.NewLine +
                                                            "The actual difference was: " + difference);
        }
    }
}
