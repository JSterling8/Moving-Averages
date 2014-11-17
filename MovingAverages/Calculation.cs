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
            if (minute < 0)
            {
                throw new ArgumentOutOfRangeException("A negative number of minutes was passed.  Minutes must be >= 0.");
            } else if (minute == 0){
                return dataset;
            }

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

            List<Entry> datesAndPrices = new List<Entry>();

            while (endIndex >= 0)
            {
                if (dataset.getEntries().ElementAt(endIndex).Date > startTime)
                {
                    datesAndPrices.Add(dataset.getEntries().ElementAt(endIndex));
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
                maxPossibleNumOfEntries = minute;
            }
            return calculateAveragePrice(datesAndPrices, maxPossibleNumOfEntries);
        }

        private decimal calculateAveragePrice(List<Entry> datesAndPrices, int maxPossibleNumOfEntries)
        {
            if (datesAndPrices.Count == 0)
            {
                return -1;
            }
            
            if (datesAndPrices.Count < maxPossibleNumOfEntries)
            {
                datesAndPrices = interpolate(datesAndPrices, maxPossibleNumOfEntries);

            }

            if (datesAndPrices.Count < maxPossibleNumOfEntries)
            {
                datesAndPrices = extrapolate(datesAndPrices, maxPossibleNumOfEntries);
            }

            decimal sum = 0;

            foreach (Entry entry in datesAndPrices)
            {
                sum += entry.Price;
            }

            return sum / ((decimal)datesAndPrices.Count);
        }

        private List<Entry> interpolate(List<Entry> datesAndPrices, int maxPossibleNumOfEntries)
        {
            decimal[] indicesNeeded = new decimal[maxPossibleNumOfEntries - datesAndPrices.Count];
            int numOfFoundNeededIndices = 0;

            decimal[] xValues = new decimal[maxPossibleNumOfEntries];
            decimal[] yValues = new decimal[maxPossibleNumOfEntries];

            // Find the indices that we need to fill
            for (int i = 0; i < datesAndPrices.Count - 1; i++)
            {
                if (datesAndPrices.ElementAt(i).Date.Subtract(datesAndPrices.ElementAt(i + 1).Date).Minutes > 1)
                {
                    indicesNeeded[numOfFoundNeededIndices] = i;
                }
                xValues[i] = i;
                yValues[i] = datesAndPrices.ElementAt(i).Price;
            }

            // Insert the values for the indices we found into our price and date list.
            for (int j = datesAndPrices.Count - 1; j > 0; j--)
            {
                if (indicesNeeded.Contains(j))
                {
                    decimal priceToInsert = LagrangeInterpolater.getInterpolatedValueGivenCurrentValuesAndIndexToFind(xValues, yValues, j);
                    DateTime dateToInsert = datesAndPrices.ElementAt(j).Date.Add(new TimeSpan(0, 1, 0));
                    datesAndPrices.Insert(j, new Entry(dateToInsert, priceToInsert));
                    j--;
                }
            }

            return datesAndPrices;
        }

        // This method only works for a list of contiguous prices (call interpolate() first to make your list contiguous).
        private List<Entry> extrapolate(List<Entry> datesAndPrices, int maxNumberOfEntries)
        {
            decimal[] xValues = new decimal[datesAndPrices.Count];
            decimal[] yValues = new decimal[datesAndPrices.Count];

            for (int i = 0; i < datesAndPrices.Count; i++)
            {
                xValues[i] = i;
                yValues[i] = datesAndPrices.ElementAt(i).Price;
            }

            int numberOfExtrapolationsToCalculate = maxNumberOfEntries - datesAndPrices.Count;

            for (int j = 0; j < numberOfExtrapolationsToCalculate; j++)
            {
                decimal priceToInsert = LagrangeInterpolater.getInterpolatedValueGivenCurrentValuesAndIndexToFind(xValues, yValues, 0 - j);
                DateTime dateToInsert = datesAndPrices.ElementAt(0).Date.Subtract(new TimeSpan(0, 1, 0));
                datesAndPrices.Insert(0, new Entry(dateToInsert, priceToInsert));
            }

            return datesAndPrices;
        }
    }
}
