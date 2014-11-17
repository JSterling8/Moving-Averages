using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovingAverages;
using System.Collections.Generic;

namespace MovingAveragesTest
{
    [TestClass]
    public class ProvidedDataCalculationTests
    {
        private const decimal TOLERANCE = 0.0000000000001m;
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
            decimal expectedValue = 1.335539m;
            decimal actualValue = results.getEntries().First().Price;
            decimal difference = Math.Abs(expectedValue - actualValue);
            Assert.IsTrue(difference <= TOLERANCE, "The difference between the expected and actual value was too great." + Environment.NewLine + 
                                                            "The maximum allowed difference is: " + TOLERANCE + Environment.NewLine +
                                                            "The actual difference was: " + difference);

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

            // TODO change expected value to account for interpolation and extrapolation
            // Unevenly spaced data (i.e., 23:46, 23:48, 23:49...) that also needs extrapolation
            decimal expectedValue = 1.342257M;
            decimal actualValue = results.getEntries().ElementAt(2832).Price;
            decimal difference = Math.Abs(expectedValue - actualValue);
            Assert.IsTrue(difference <= TOLERANCE, "The difference between the expected and actual value was too great." + Environment.NewLine +
                                                            "The maximum allowed difference is: " + TOLERANCE + Environment.NewLine +
                                                            "The actual difference was: " + difference);
        }
    }
}
