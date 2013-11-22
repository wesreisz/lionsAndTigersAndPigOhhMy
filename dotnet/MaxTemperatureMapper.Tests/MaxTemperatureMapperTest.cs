using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Hadoop.MapReduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxTemperatureMapper.Tests
{
    /*
     * Take a look a the following blog to learn more about
     * how to unit test these jobs
     * https://github.com/WindowsAzure-Samples/HDInsight-Labs-Preview/blob/master/HandsOnLabs.md#Lab1
     */

    [TestClass]
    public  class MaxTemperatureMapperTest
    {
        [TestMethod]
	    public void processesValidRecord() {
		    String value = 
			    "0043011990999991950051518004+68750+023550FM-12+038299999V0203201N00261220001CN9999999N9-00111+99999999999";
            var actualOutput = StreamingUnit.Execute<MaxTemperatureMapper.Program.MaxTemperatureMapper>(new[] {value});
            var mapperResult = actualOutput.MapperResult.Single().Split('\t');
            //key is emitted as first tab sep  value
            Assert.AreEqual("1950",mapperResult[0]);
            Assert.AreEqual("-111", mapperResult[1]);
	    }

        [TestMethod]
        public void processesValidRecord1()
        {
            String value =
               "0043011990999991950051518004+68750+023550FM-12+038299999V0203201N00261220001CN9999999N9+99999+99999999999";
            var actualOutput = StreamingUnit.Execute<MaxTemperatureMapper.Program.MaxTemperatureMapper>(new[] { value });
            var mapperResult = actualOutput.MapperResult.Single().Split('\t');
            Assert.AreEqual("1950", mapperResult[0]);
            Assert.AreEqual("99999", mapperResult[1]);
        }
    }
}
