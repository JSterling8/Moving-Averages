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

        /// <summary>
        /// Calculates the simple moving average of the dataset using a variable period such as 10 minutes
        /// </summary>
        /// <param name="minute">Period of the moving average, in minutes</param>
        /// <returns>A dataset showing the calculated moving average over time</returns>
        public Dataset calculate( int minute )
        {
            Dataset results = new Dataset();

            //TODO Calculate the moving Average here
            List<Entry> dataList = results.getEntries();




            return results;
        }
    }
}
