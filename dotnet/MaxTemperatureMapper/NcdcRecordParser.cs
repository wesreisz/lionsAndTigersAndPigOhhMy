using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MaxTemperatureMapper
{
    public class NcdcRecordParser
    {
        public static int MISSING_TEMPERATURE = 99999;
        private String _year;
        private int _airTemperature; 
	    private String _quality;

        private NcdcRecordParser() { }

        public static NcdcRecordParser parse(String input)
        {
            NcdcRecordParser record = new NcdcRecordParser();
            record._year = input.Substring(15, 4);
            record._airTemperature = int.Parse(input.Substring(87, 6).Replace("+",""));
            record._quality = input.Substring(93, 5); 
		    
            return record;
        }
        public bool isValidTemperature()
        {
            Match match = Regex.Match(_quality, "[01459]");
            Boolean isTempGood = _airTemperature != MISSING_TEMPERATURE;
            return isTempGood && match.Success;
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
