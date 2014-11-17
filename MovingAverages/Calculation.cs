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

        /// <summary>
        /// Calculates the simple moving average of the dataset using a variable period such as 10 minutes
        /// </summary>
        /// <param name="minutes">Period of the moving average, in minutes</param>
        /// <returns>A dataset showing the calculated moving average over time</returns>
        public Dataset calculateAllMovingAverages(int minutes)
        {
            if (minutes < 0)
            {
                throw new ArgumentOutOfRangeException("A negative number of minutes was passed.  Minutes must be >= 0.");
            } else if (minutes == 0){
                return dataset;
            }

            Dataset results = new Dataset();

            //TODO Calculate the moving Average here
            List<DateTime> dates = new List<DateTime>();
            List<decimal> prices = new List<decimal>();

            List<Entry> entries = dataset.getEntries();

            foreach (Entry entry in entries)
            {
                decimal average = calculateSingleAverageGivenDateTime(entry.Date, minutes);
                dates.Add(entry.Date);
                prices.Add(average);
            }

            results.load(dates, prices);

            return results;
        }

        /// <summary>
        /// Calculates the average value in the Dataset between a given endTime and (endTime - (minutes-1))
        /// </summary>
        /// <param name="endTime">The last time in the moving average 
        /// (i.e., in the moving average for 12:01, 12:02, and 12:03, 12:03 would be the endTime.</param>
        /// <param name="minutes">The number of minutes in the moving average</param>
        /// <returns>The average value in the moving average that ends at the given endTime and starts at (endTime - (minutes-1))</returns>
        private decimal calculateSingleAverageGivenDateTime(DateTime endTime, int minutes)
        {
            DateTime startTime = endTime.Subtract(new TimeSpan(00, (minutes - 1), 00));
            int endIndex = dataset.getIndexGivenDateTime(endTime);

            List<Entry> datesAndPrices = new List<Entry>();

            while (endIndex >= 0)
            {
                if (dataset.getEntries().ElementAt(endIndex).Date >= startTime)
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
            if (endIndex >= minutes - 1)
            {
                maxPossibleNumOfEntries = minutes;
            } 
            else
            {
                maxPossibleNumOfEntries = minutes;
            }

            return calculateAveragePrice(datesAndPrices, maxPossibleNumOfEntries);
        }

        /// <summary>
        /// Given a dataset and max number of possible entries over the time period the dataset is supposed to cover, 
        /// this method interpolates and extrapolates the dataset as necessary, then calculates an average price.
        /// </summary>
        /// <param name="endTime">The last time in the moving average 
        /// (i.e., in the moving average for 12:01, 12:02, and 12:03, 12:03 would be the endTime.</param>
        /// <param name="minutes">The number of minutes in the moving average</param>
        /// <returns>The average value in the moving average that ends at the given endTime and starts at (endTime - (minutes-1))</returns>
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
            // If we only have one or two dates and prices, we want to look forwards to see if we can find a few more, but only if they're contiguous.
            if (datesAndPrices.Count <= maxNumberOfEntries / 2)
            {
                for (int i = 1; i <= maxNumberOfEntries / 2; i++)
                {
                    int nextValuesIndex = dataset.getIndexGivenDateTime(datesAndPrices.Last().Date) + i;
                    if (nextValuesIndex < dataset.getEntries().Count &&
                        dataset.getEntries().ElementAt(nextValuesIndex).Date.Subtract(datesAndPrices.Last().Date) == new TimeSpan(0, i, 0))
                    {
                        datesAndPrices.Add(dataset.getEntries().ElementAt(nextValuesIndex));
                    }
                    else
                    {
                        // With the current implementation, we have to break if even one is more than a minute off, otherwise our xValues and yValues 
                        // will be skewed below. Realistically, this *can* be improved upon (getting 5 forward values without checking for the minute 
                        // different, then interpolating them).
                        break;
                    }
                }
            }

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
                decimal priceToInsert = LagrangeInterpolater.getInterpolatedValueGivenCurrentValuesAndIndexToFind(xValues, yValues, 0 - j - 1);
                DateTime dateToInsert = datesAndPrices.ElementAt(0).Date.Subtract(new TimeSpan(0, 1, 0));
                datesAndPrices.Insert(0, new Entry(dateToInsert, priceToInsert));
            }

            return datesAndPrices;
        }
    }
}
