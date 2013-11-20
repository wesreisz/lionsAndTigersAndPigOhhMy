using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaxTemperatureMapper;

namespace MaxTemperatureMapper.Tests
{
    [TestClass]
    public class NcdcRecordParserTest
    {

        private String value =
            "0043011990999991950051518004+68750+023550FM-12+0382" +    // Year
            //              ^^^^ this is the year
            "99999V0203201N00261220001CN9999999N9-00111+99999999999"; // Temperature
            //        ^^ this is the temperature
        [TestMethod]
        public void simpleYearTest()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(value);
            Assert.AreEqual("1950", parser.getYear());
            //Assert.AreEqual("good", parser.Quality);
        }
        [TestMethod]
        public void simpleTempTest() {
            NcdcRecordParser parser = NcdcRecordParser.parse(value);
            Assert.AreEqual(-111, parser.getAirTemperature());
        }

    }
}
