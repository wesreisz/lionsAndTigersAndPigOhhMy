using Microsoft.Hadoop.MapReduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * This code sample was taken from a video at:
 * http://www.youtube.com/watch?v=uyi41nrhlhw
 * Intro to Hadoop MapReduce with C#
 * by Rob Kerr
 */

namespace SquareRoot
{
    class Program
    {
        public class SqrtMapper : MapperBase
        {
            public override void Map(string inputLine, MapperContext context)
            {
                int inputValue = int.Parse(inputLine);

                double sqrt = Math.Sqrt((double)inputValue);

                context.EmitKeyValue(inputValue.ToString(), sqrt.ToString());            
            }
        }

        public class Sqrtjob : HadoopJob<SqrtMapper> 
        {

            public override HadoopJobConfiguration Configure(ExecutorContext context)
            {
                HadoopJobConfiguration config = new HadoopJobConfiguration();
                config.InputPath = "Input/sqrt";
                config.OutputFolder = "Output/sqrt";
                return config; 
            }
        }

        static void Main(string[] args)
        {
            var hadoop = Hadoop.Connect();
            var result = hadoop.MapReduceJob.ExecuteJob<Sqrtjob>();
        }
    }
}
