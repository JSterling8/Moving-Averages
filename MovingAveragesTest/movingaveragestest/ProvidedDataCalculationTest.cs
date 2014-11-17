using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovingAverages;
using System.Collections.Generic;

namespace MovingAveragesTest
{
    [TestClass]
    public class ProvidedDataCalculationTest
    {
        private Dataset providedData;
        private Calculation calculation;

        [TestInitialize]
        public void initialise()
        {
            providedData = new Dataset();
            providedData.load("MovingAveragesData.csv");

            calculation = new Calculation(providedData);
        }

        [TestMethod]
        public void checkBasicCases()
        {
            Dataset results = calculation.calculateAllMovingAverages(10);

            // Nothing preceding
            Assert.AreEqual(1.33877M, results.getEntries().First().Price);

            // Evenly spaced data
            Assert.AreEqual(1.341999M, results.getEntries().ElementAt(2840).Price);

            // Evenly spaced and across 2 days
            Assert.AreEqual(1.342014M, results.getEntries().ElementAt(2842).Price);
        }

        [TestMethod]
        public void checkUnevenlySpacedData()
        {
            providedData = new Dataset();
            providedData.load("MovingAveragesData.csv");

            Dataset results = calculation.calculateAllMovingAverages(10);

            // TODO change expected value to account for interpolation
            // Unevenly spaced data (i.e., 23:46, 23:48, 23:49...)
            Assert.AreEqual(1.34203125M, results.getEntries().ElementAt(2832).Price);
        }
    }
}
