using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingAverages
{
    public class Calculation
    {
        protected Dataset dataset;

        public Calculation(Dataset dataset)
        {
            this.dataset = dataset;
        }

        // <summary>
        // Calculates the simple moving average of the dataset using a variable period such as 10 minutes
        // </summary>
        // <param name="minute">Period of the moving average, in minutes</param>
        // <returns>A dataset showing the calculated moving average over time</returns>
        public Dataset calculateAllMovingAverages(int minute)
        {
            Dataset results = new Dataset();

            //TODO Calculate the moving Average here
            List<DateTime> dates = new List<DateTime>();
            List<decimal> prices = new List<decimal>();

            List<Entry> entries = dataset.getEntries();

            foreach (Entry entry in entries)
            {
                decimal average = calculateSingleAverageGivenDateTime(entry.Date, minute);
                dates.Add(entry.Date);
                prices.Add(average);
            }

            results.load(dates, prices);

            return results;
        }

        private decimal calculateSingleAverageGivenDateTime(DateTime endTime, int minute)
        {
            DateTime startTime = endTime.Subtract(new TimeSpan(00, (minute), 00));
            int endIndex = dataset.getIndexGivenDateTime(endTime);

            List<DateTime> dates = new List<DateTime>();
            List<decimal> prices = new List<decimal>();

            while (endIndex >= 0)
            {
                if (dataset.getEntries().ElementAt(endIndex).Date > startTime)
                {
                    dates.Add(dataset.getEntries().ElementAt(endIndex).Date);
                    prices.Add(dataset.getEntries().ElementAt(endIndex).Price);
                    endIndex--;
                }
                else
                {
                    break;
                }
            }

            int maxPossibleNumOfEntries;
            if (endIndex >= minute - 1)
            {
                maxPossibleNumOfEntries = minute;
            }
            else
            {
                maxPossibleNumOfEntries = endIndex;
            }
            return calculateAveragePrice(dates, prices, maxPossibleNumOfEntries);
        }

        private decimal calculateAveragePrice(List<DateTime> dates, List<decimal> prices, int maxPossibleNumOfEntries)
        {
            if (prices.Count == 0)
            {
                return -1;
            }

            if (prices.Count < maxPossibleNumOfEntries)
            {
                prices = interpolate(dates, prices, maxPossibleNumOfEntries);
            }

            decimal sum = 0;

            foreach (decimal data in prices)
            {
                sum += data;
            }


            return sum / ((decimal)prices.Count);

        }

        private List<decimal> interpolate(List<DateTime> dates, List<decimal> prices, int maxPossibleNumOfEntries)
        {
            if (dates.ElementAt(0).Equals(new DateTime(2014, 08, 04, 23, 48, 00)))
            {
                // Find me debugger;
                int asfd = 0;
            }

            decimal[] indicesNeeded = new decimal[maxPossibleNumOfEntries - prices.Count];
            int numOfFoundNeededIndices = 0;

            decimal[] xValues = new decimal[maxPossibleNumOfEntries];
            decimal[] yValues = new decimal[maxPossibleNumOfEntries];

            // Find the indices that we need to fill
            for (int i = 0; i < dates.Count - 1; i++)
            {
                if (dates.ElementAt(i).Subtract(dates.ElementAt(i + 1)).Minutes > 1)
                {
                    indicesNeeded[numOfFoundNeededIndices] = i;
                }
                xValues[i] = i;
                yValues[i] = prices.ElementAt(i);
            }

            // Insert those values into our price and date list.
            for (int j = prices.Count - 1; j > 0; j--)
            {
                if (indicesNeeded.Contains(j))
                {
                    prices.Insert(j, LagrangeInterpolater.getInterpolatedValueGivenCurrentValuesAndIndexToFind(xValues, yValues, j));
                    dates.Insert(j, dates.ElementAt(j).Add(new TimeSpan(0, 1, 0)));
                    j--;
                }
            }

            return prices;
        }
    }
}
