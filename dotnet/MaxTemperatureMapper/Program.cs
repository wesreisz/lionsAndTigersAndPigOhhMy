using Microsoft.Hadoop.MapReduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxTemperatureMapper
{
    public class Program
    {
        public class MaxTemperatureMapper : MapperBase
        {
            public override void Map(string inputLine, MapperContext context)
            {
                NcdcRecordParser record = NcdcRecordParser.parse(inputLine);
                if (record.isValidTemperature()) {
                    context.EmitKeyValue(
                        record.getYear().ToString(), 
                        record.getAirTemperature().ToString()
                       );
                }
            }
        }
        public class MaxTemperatureReduce : ReducerCombinerBase 
        {
            public override void Reduce(string key, IEnumerable<string> temperatures, ReducerCombinerContext context)
            {
                int max_value = int.MinValue;
                foreach (String temperature in temperatures)
                {
                    if (!temperature.Equals(NcdcRecordParser.MISSING_TEMPERATURE))
                    {
                        max_value = Math.Max(max_value, int.Parse(temperature));
                    }
                }
                context.EmitKeyValue(key, max_value.ToString());
            }
        }
        public class MaxTemperatureJob : HadoopJob<MaxTemperatureMapper, MaxTemperatureReduce> 
        {
            public override HadoopJobConfiguration Configure(ExecutorContext context)
            {
                HadoopJobConfiguration config = new HadoopJobConfiguration();

                config.InputPath = "Input/ncdc";
                config.OutputFolder = "Output/ncdc";
                return config;
            }
        }
        static void Main(string[] args)
        {
            var hadoop = Hadoop.Connect();
            var result = hadoop.MapReduceJob.ExecuteJob<MaxTemperatureJob>();

            Console.In.Read();
        }
    }
}
