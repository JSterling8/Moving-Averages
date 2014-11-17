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
        private Calculation calulation;
        private Dataset results;

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

            calulation = new Calculation(reliableDataset);
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
        public void CheckBasicCalculation()
        {
            results = calulation.calculateAllMovingAverages(10);
            Assert.AreEqual(1.645M, results.getEntries().Last().Price);
            
        }
        
        [TestMethod]
        public void CheckExtrapolatedCalculation()
        {
            results = calulation.calculateAllMovingAverages(10);

            decimal expectedValue = 1.545M;
            decimal actualValue = results.getEntries().First().Price;
            decimal difference = Math.Abs(expectedValue - actualValue);
            Assert.IsTrue(difference <= TOLERANCE, "The difference between the expected and actual value was too great." + Environment.NewLine +
                                                            "The maximum allowed difference is: " + TOLERANCE + Environment.NewLine +
                                                            "The actual difference was: " + difference);
        }

        [TestMethod]
        public void CheckWhenRangeIsEqualToSize()
        {
            Dataset results = calulation.calculateAllMovingAverages(reliableDataset.getEntries().Count);

            decimal expectedValue = 1.595M;
            decimal actualValue = results.getEntries().Last().Price;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void CheckRangeOfZero()
        {
            results = calulation.calculateAllMovingAverages(0);

            for (int i = 0; i < results.getEntries().Count; i++)
            {
                Assert.AreEqual(reliableDataset.getEntries().ElementAt(i), results.getEntries().ElementAt(i));
            }
        }

        [TestMethod]
        public void CheckNegativeRange()
        {
            ArgumentOutOfRangeException exceptionWeShouldCatch = null;

            try{
                results = calulation.calculateAllMovingAverages(-1);
            }
            catch (ArgumentOutOfRangeException e)
            {
                exceptionWeShouldCatch = e;
            }
            finally
            {
                Assert.IsNotNull(exceptionWeShouldCatch);   
            }
        }
    }
}
