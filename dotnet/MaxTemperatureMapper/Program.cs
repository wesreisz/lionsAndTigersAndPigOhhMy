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
                return localConfig();
                //return azureConfig();
            }
            static HadoopJobConfiguration localConfig()
            {
                HadoopJobConfiguration config = new HadoopJobConfiguration();
                config.InputPath = "input/ncdc";
                config.OutputFolder = "output/ncdc";
                return config;
            }
            static HadoopJobConfiguration azureConfig()
            {
                HadoopJobConfiguration config = new HadoopJobConfiguration();
                config.InputPath = "asv://working@reiszeast1.blob.core.windows.net/input/ncdc";
                config.OutputFolder = "asv://working@reiszeast1.blob.core.windows.net/output/ncdc";
                return config;
            }
        }

        static IHadoop connect2Azure()
        {
            return Hadoop.Connect(
                new Uri("https://reiszeast.azurehdinsight.net"),
                "admin", "hadoop", "Password!2",
                "reiszeast1.blob.core.windows.net",
                "T78PCkv/3zvCcGASIEE9h9yzdUumRrnZZm5A8SAIVocn/W11WkwJ9JXKRU3RF7TT+3KfecMy4NDV1Ddfk4OCkg==",
                "working",
                true
            );
        }
        static void Main(string[] args)
        {
            var hadoop = Hadoop.Connect();
            //var hadoop = connect2Azure();
            var result = hadoop.MapReduceJob.ExecuteJob<MaxTemperatureJob>();

            Console.In.Read();
        }
    }
}
