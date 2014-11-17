using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovingAverages;

namespace MovingAveragesTest
{
    [TestClass]
    public class DatasetTests
    {
        private Dataset dataset;

        [TestInitialize]
        public void TestInitialize()
        {
            dataset = new Dataset();
            dataset.load("MovingAveragesData.csv"); 
        }

        [TestMethod]
        public void CheckLoadCount()
        {
            Assert.AreEqual(2843, dataset.getEntries().Count );
        }

        [TestMethod]
        public void CheckFirstEntryPrice()
        {
            Assert.AreEqual(1.33877M, dataset.getEntries().First().Price);
        }

        [TestMethod]
        public void ShowAllEntriesTest()
        {
            foreach( Entry entry in dataset.getEntries() )
            {
                Console.WriteLine(entry);
            }
        }

        [TestMethod]
        public void CheckValidIndexLookup()
        {
            // Invalid range (before any valid data)
            Assert.AreEqual(-1, dataset.getIndexGivenDateTime(new DateTime(2013, 08, 01, 00, 01, 00)));

            // Beginning
            Assert.AreEqual(0, dataset.getIndexGivenDateTime(new DateTime(2014, 08, 01, 00, 01, 00)));
            Assert.AreEqual(1, dataset.getIndexGivenDateTime(new DateTime(2014, 08, 01, 00, 02, 00)));
            Assert.AreEqual(2, dataset.getIndexGivenDateTime(new DateTime(2014, 08, 01, 00, 03, 00)));

            // Middleish
            Assert.AreEqual(1340, dataset.getIndexGivenDateTime(new DateTime(2014, 08, 03, 22, 30, 00)));

            //End
            Assert.AreEqual(2841, dataset.getIndexGivenDateTime(new DateTime(2014, 08, 04, 23, 59, 00)));
            Assert.AreEqual(2842, dataset.getIndexGivenDateTime(new DateTime(2014, 08, 05, 00, 00, 00)));
        }

        [TestMethod]
        public void CheckLookupOfInvalidIndexes()
        {
            // Invalid date (before any valid data)
            Assert.AreEqual(-1, dataset.getIndexGivenDateTime(new DateTime(2013, 08, 01, 00, 01, 00)));

            // Invalid date (in middle where there's no date)
            Assert.AreEqual(-1, dataset.getIndexGivenDateTime(new DateTime(2013, 08, 02, 00, 01, 00)));

            // Invalid date (after any valid data)
            Assert.AreEqual(-1, dataset.getIndexGivenDateTime(new DateTime(2015, 08, 01, 00, 01, 00)));
        }
    }    
}
