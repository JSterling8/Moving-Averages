using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingAverages
{
    public class Dataset
    {
        protected List<Entry> entries = new List<Entry>();

        public Dataset()
        {

        }

        public void load( string filename )
        {
            string[] inputs = File.ReadAllLines(filename);
            load(inputs);
        }

        public void load( string[] inputs )
        {            
            foreach (string input in inputs )
            {
                string[] split = input.Split(',');
                DateTime date = DateTime.ParseExact(split[0], "dd/MM/yyyy HH:mm:ss",  CultureInfo.InvariantCulture );
                decimal price = Convert.ToDecimal(split[1]);
                entries.Add(new Entry(date, price));
            }
        }

        public void load( List<DateTime> dates, List<decimal> prices )
        {
            for( int index = 0; index < dates.Count; index++ )
            {
                Entry entry = new Entry(dates[index], prices[index]);
                entries.Add(entry);
            }
        }

        public List<Entry> getEntries()
        {
            return entries;
        }

        /// <summary>
        /// Performs a simple binary search of the data in order to find the index of a given DateTime.
        /// This method assumes all DateTime entries are unique.
        /// </summary>
        /// <param name="dateTimeToFind">The DateTime whose index you wish to find.</param>
        /// <returns>The index of entries for the DateTime provided.  Returns -1 if the index can't be found.</returns>
        public int getIndexGivenDateTime(DateTime dateTimeToFind)
        {
            if (entries.Count == 0)
            {
                throw new NullReferenceException("You're attempting to look up and entry before adding any entries.");
            }

            int minIndex = 0;
            int maxIndex = entries.Count - 1;
            int maxNumOfChecks = (int) Math.Log(entries.Count, 2) + 1;
            int numOfChecksMade = 0;

            while (true)
            {
                int indexChecking = (maxIndex + minIndex) / 2;

                if (numOfChecksMade > maxNumOfChecks || minIndex < 0 || maxIndex >= entries.Count)
                {
                    return -1;
                } else if (entries.ElementAt(indexChecking).Date.Equals(dateTimeToFind))
                {
                    return indexChecking;
                }
                else if (entries.ElementAt(indexChecking).Date > dateTimeToFind)
                {
                    maxIndex = indexChecking - 1;
                } else {
                    minIndex = indexChecking + 1;
                }
                numOfChecksMade++;
            }
        }
    }
}
