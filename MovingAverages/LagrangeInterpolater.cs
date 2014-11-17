using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingAverages
{
    public class LagrangeInterpolater
    {

        /// <summary>
        /// This method was adopted from the code here: http://www.dailyfreecode.com/code/lagranges-interpolation-method-finding-2376.aspx
        /// </summary>
        /// <param name="xValues">The x values from a function</param>
        /// <param name="yValues">The f(x) values from a function</param>
        /// <param name="fOfXLookingFor">The f(x) value to look for in the function</param>
        /// <returns></returns>
        public static decimal getInterpolatedValueGivenCurrentValuesAndIndexToFind(decimal[] xValues, decimal[] yValues, decimal fOfXLookingFor)
        {
            if (xValues.Length != yValues.Length)
            {
                throw new ArgumentException("xValues and yValues must be the same size.");
            }

            int n = xValues.Length;

            decimal[] f = new decimal[xValues.Length];

            for (int i = 0; i < n; i++)
            {
                decimal temp = 1;
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else if (xValues[i] - xValues[j] != 0)
                    {
                        temp = temp * ((fOfXLookingFor - xValues[j]) / (xValues[i] - xValues[j]));
                    }
                    
                }
                f[i] = yValues[i] * temp;
            }

            decimal sum = 0;
            for (int k = 0; k < n; k++)
            {
                sum += f[k];
            }

            return sum;
        }
    }
}
