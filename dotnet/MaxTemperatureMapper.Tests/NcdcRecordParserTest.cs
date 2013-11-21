using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaxTemperatureMapper;

namespace MaxTemperatureMapper.Tests
{
    [TestClass]
    public class NcdcRecordParserTest
    {

        private String testString =
            "0043011990999991950051518004+68750+023550FM-12+0382" +    // Year
            //              ^^^^ this is the year
            "99999V0203201N00261220001CN9999999N9-00111+99999999999"; // Temperature
            //        ^^ this is the temperature
        private String testString1 = 
            "0067011990999991950051507004+68750+023550FM-12+038299999V0203301N00671220001CN9999999N9+00001+99999999999";
        private String testString2 = 
            "0043011990999991950051512004+68750+023550FM-12+038299999V0203201N00671220001CN9999999N9+00221+99999999999";
        private String testString3 = 
            "0043011990999991950051518004+68750+023550FM-12+038299999V0203201N00261220001CN9999999N9-00111+99999999999";
        private String testString4 = 
            "0043012650999991949032412004+62300+010750FM-12+048599999V0202701N00461220001CN0500001N9+01111+99999999999";
        private String testString5 =
            "0043012650999991949032418004+62300+010750FM-12+048599999V0202701N00461220001CN0500001N9+00781+99999999999";
        [TestMethod]
        public void simpleYearTest()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString);
            Assert.AreEqual("1950", parser.getYear());
            //Assert.AreEqual("good", parser.Quality);
        }
        [TestMethod]
        public void simpleYearTest1()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString5);
            Assert.AreEqual("1949", parser.getYear());
            //Assert.AreEqual("good", parser.Quality);
        }
        [TestMethod]
        public void simpleTempTest() {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString);
            Assert.AreEqual(-111, parser.getAirTemperature());
        }
        [TestMethod]
        public void simpleTempTest1()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString4);
            Assert.AreEqual(1111, parser.getAirTemperature());
        }
        [TestMethod]
        public void simpleTempTest2()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString5);
            Assert.AreEqual(781, parser.getAirTemperature());
        }
        [TestMethod]
        public void isValidTest() {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString);
            Assert.AreEqual(true, parser.isValidTemperature());
        }
        [TestMethod]
        public void isValidTest1()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString1);
            Assert.AreEqual(true, parser.isValidTemperature());
        }
        [TestMethod]
        public void isValidTest2()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString2);
            Assert.AreEqual(true, parser.isValidTemperature());
        }
        [TestMethod]
        public void isValidTest3()
        {
            NcdcRecordParser parser = NcdcRecordParser.parse(testString3);
            Assert.AreEqual(true, parser.isValidTemperature());
        }
        [TestMethod]
        public void testTemperatureParse() {
            Assert.AreEqual(123,int.Parse("+123"));
            Assert.AreEqual(-123,int.Parse("-123"));
            Assert.AreEqual(125653, int.Parse("+125653"));
        }
    }
}
