using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxTemperatureMapper
{
    public class NcdcRecordParser
    {
        private static int MISSING_TEMPERATURE = 9999;
        private String _year;
        private int _airTemperature; 
	    private String _quality;

        private NcdcRecordParser() { }

        public static NcdcRecordParser parse(String input)
        {
            
            NcdcRecordParser record = new NcdcRecordParser();
            record._year = input.Substring(15, 4);
            // Remove leading plus sign as parseInt doesn't like them 
		    if (input[87] == '+') {
			    record._airTemperature = int.Parse(input.Substring(88, 5)); 
		    } else {
			    record._airTemperature = int.Parse( input.Substring(87, 6));
                record._quality = input.Substring(93, 5); 
		    }
            return record;
        }
        public bool isValidTemperature()
        {
            return _airTemperature != MISSING_TEMPERATURE && _quality.Equals("01459");
        }
        public String getYear()
        {
            return _year;
        }
        public int getAirTemperature()
        {
            return _airTemperature;
        }

    }
}
