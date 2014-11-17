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

            List<decimal> prices = new List<decimal>();

            while (endIndex >= 0)
            {
                if (entries.ElementAt(endIndex).Date > startTime)
                {
                    prices.Add(entries.ElementAt(endIndex).Price);
                    endIndex--;
                }
                else
                {
                    break;
                }
            }

            return calculateAveragePrice(prices);
        }

        private decimal calculateAveragePrice(List<decimal> prices)
        {
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
    }
}
