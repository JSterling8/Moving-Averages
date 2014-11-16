using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingAverages
{
    public class MovingAverages
    {        
        static void Main(string[] args)
        {
            Dataset dataset = new Dataset();
            dataset.load("MovingAveragesData.csv");

            Calculation calculation = new Calculation( dataset );
            //Dataset results = calculation.calculateAllMovingAverages( 10 );
        }
    }
}
