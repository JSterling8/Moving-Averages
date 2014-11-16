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
    }
}
