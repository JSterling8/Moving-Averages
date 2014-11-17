using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingAverages
{
    public class LagrangeInterpolater
    {
        public static decimal getInterpolatedValueGivenListAndInsertionIndex(decimal[] xValues, decimal[] yValues, decimal fOfXLookingFor)
        {
            decimal[] f = new decimal[xValues.Length];

            for (int i = 0; i < xValues.Length; i++)
            {
                decimal temp = 1;
                for (int j = 0; j < xValues.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else
                    {
                        temp = temp * ((fOfXLookingFor - xValues[j]) / (xValues[i] - xValues[j]));
                    }
                }
                f[i] = yValues[i] * temp;
            }

            decimal sum = 0;
            for (int k = 0; k < xValues.Length; k++)
            {
                sum += f[k];
            }

            return sum;
        }
    }
}
