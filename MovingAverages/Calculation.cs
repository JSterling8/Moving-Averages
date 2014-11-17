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

        public Calculation( Dataset dataset )
        {
            this.dataset = dataset;
        }

        // <summary>
        // Calculates the simple moving average of the dataset using a variable period such as 10 minutes
        // </summary>
        // <param name="minute">Period of the moving average, in minutes</param>
        // <returns>A dataset showing the calculated moving average over time</returns>
        public Dataset calculateAllMovingAverages( int minute )
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
            List<Entry> entries = dataset.getEntries();

            DateTime startTime = endTime.Subtract(new TimeSpan(00, (minute), 00));
            int endIndex = dataset.getIndexGivenDateTime(endTime);

            List<DateTime> dates = new List<DateTime>();
            List<decimal> prices = new List<decimal>();

            while (endIndex >= 0)
            {
                if (entries.ElementAt(endIndex).Date > startTime)
                {
                    dates.Add(entries.ElementAt(endIndex).Date);
                    prices.Add(entries.ElementAt(endIndex).Price);
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
            List<Entry> pricesAndDates = new List<Entry>();

            if (prices.Count < maxPossibleNumOfEntries)
            {
                pricesAndDates = interpolate(dates, prices, maxPossibleNumOfEntries);
                prices = new List<decimal>();

                foreach (Entry entry in pricesAndDates)
                {
                    prices.Add(entry.Price);
                }
            }

            decimal sum = 0;
            
            foreach(decimal data in prices){
                sum += data;
            }

            if (prices.Count == 0)
            {
                return -1;
            }
            else
            {
                return sum / ((decimal)prices.Count);
            }
        }

        private List<Entry> interpolate(List<DateTime> dates, List<decimal> prices, int maxPossibleNumOfEntries)
        {
            if (dates.ElementAt(0).Equals(new DateTime(2014, 08, 01, 00, 31, 00)))
            {
                // Find me debugger;
                int asfd = 0;
            }

            decimal[] indicesNeeded = new decimal[maxPossibleNumOfEntries - prices.Count];
            int numOfFoundNeededIndices = 0;
            decimal[] xValues = new decimal[dates.Count];
            decimal[] yValues = new decimal[dates.Count];

            // Find the indices that we need to fill
            for (int i = 0; i < dates.Count - 1; i++)
            {
                if (dates.ElementAt(i).Subtract(dates.ElementAt(i + 1)).Minutes > 1)
                {
                    indicesNeeded[numOfFoundNeededIndices] = ((decimal)i + ((decimal)i + 1m)) / 2.0m;
                    numOfFoundNeededIndices++;
                }        
                xValues[i] = i;
                yValues[i] = prices[i];
            }
            
            // Insert those values into our price and date list.
            for (int j = prices.Count - 1; j > 0; j--)
            {
                if(indicesNeeded.Contains((decimal)j - 0.5m)){
                    prices.Insert(j, LagrangeInterpolater
                        .getInterpolatedValueGivenCurrentValuesAndIndexToFind(xValues, yValues, (decimal)j - 0.5m));    
                    dates.Insert(j, dates.ElementAt(j).Subtract(new TimeSpan(0, 1, 0)));
                    j--;
                }
            }

            List<Entry> interpolatedList = new List<Entry>();
            for(int k = 0; k < prices.Count; k++){
                interpolatedList.Add(new Entry(dates.ElementAt(k), prices.ElementAt(k)));
            }

            return interpolatedList;
        }
    }
}
