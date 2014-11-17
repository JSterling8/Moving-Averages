using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovingAverages;
using System.Collections.Generic;

namespace MovingAveragesTest
{
    [TestClass]
    public class LagrangeTests
    {
        private const decimal TOLERANCE = 0.0000000000001m;

        private decimal[] simpleXValues;
        private decimal[] simpleYValues;
        private decimal[] complexXValues;
        private decimal[] complexYValues;

        [TestInitialize]
        public void TestInitialize()
        {
            simpleXValues = new decimal[10];
            simpleYValues = new decimal[10];
            for (int i = 0; i < 10; i++)
            {
                simpleXValues[i] = i;
                simpleYValues[i] = (2 * i) + 3;
            }

            complexXValues = new decimal[10];
            complexYValues = new decimal[10];
            for (int i = 0; i < 10; i++)
            {
                complexXValues[i] = i;
                complexYValues[i] = (2 * (decimal) Math.Pow(i, 3)) - (4 * (decimal)Math.Pow(i, 2)) + (3 * i) +  3;
            }

        }

        [TestMethod]
        public void CheckSimpleInterpolation()
        {
            for (decimal i = -50m; i <= 50m; i += .01m)
            {
                decimal correctAnswer = (2*i) + 3;
                decimal interpolatorsAnswer = LagrangeInterpolater.getInterpolatedValueGivenCurrentValuesAndIndexToFind(simpleXValues, simpleYValues, (decimal) i);
                decimal difference = Math.Abs(correctAnswer - interpolatorsAnswer);

                Assert.IsTrue(difference <= TOLERANCE, "The difference from the correct answer for input: " + i + " was: " + difference);
            }
        }

        [TestMethod] 
        public void CheckComplexInterpolation(){
            for (decimal i = -50m; i <= 50m; i += .01m)
            {
                decimal correctAnswer = (2 * (decimal)Math.Pow((double)i, 3)) - (4 * (decimal)Math.Pow((double)i, 2)) + (3 * i) + 3; ;
                decimal interpolatorsAnswer = LagrangeInterpolater.getInterpolatedValueGivenCurrentValuesAndIndexToFind(complexXValues, complexYValues, (decimal)i);
                decimal difference = Math.Abs(correctAnswer - interpolatorsAnswer);

                Assert.IsTrue(difference <= TOLERANCE, "The difference from the correct answer for input: " + i + " was: " + difference);
            }
        }
    }
}
