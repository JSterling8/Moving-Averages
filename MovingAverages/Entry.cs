using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingAverages
{
    public class Entry
    {        
        public DateTime Date { get; private set; }
        public decimal Price { get; private set; }

        public Entry ( DateTime date, decimal price )
        {
            this.Date = date;
            this.Price = price;
        }

        public override string ToString()
        {
            return Date + " = " + Price;
        }
    }
}
