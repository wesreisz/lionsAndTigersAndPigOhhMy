using Microsoft.Hadoop.MapReduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount
{
    /*
     * Converts the classic wordcount example from java into C#
     * In order to execute, first upload from the input directory 
     * in https://github.com/wesreisz/lionsAndTigersAndPigOhhMy
     * the bible text file, then press run in visual studio.
     * 
     * NOTE: This will target a local azure hd insight instance 
     * and will overwrite the output directory under Output/wordcount
     * The result will be sorted alphebetically... to sort by number
     * I cheated and used the linux sort command from cygwin
     * sort file.txt -nk2
     */
    class Program
    {
        public class WordCountMapper : MapperBase
        {
            private static int one = 1;
            public override void Map(string inputLine, MapperContext context)
            {
                string[] words = inputLine.Split(' ');
                foreach (String word in words) {
                    if (word.Length>1) {
                        //outputs each word into a single line with the number 1
                        //skip spaces
                        context.EmitKeyValue(word, one.ToString());
                    }
                }
            }
        }
        public class WordCountReducer : ReducerCombinerBase
        {
            public override void Reduce(string key, IEnumerable<string> values, ReducerCombinerContext context)
            {
                //sum all the ones
               int sum = 0;
               foreach(String value in values)
               {
                  sum += int.Parse(value);
               }
               context.EmitKeyValue(key, sum.ToString());
            }
        }
        public class WordCountjob : HadoopJob<WordCountMapper, WordCountReducer>
        {
            public override HadoopJobConfiguration Configure(ExecutorContext context)
            {
                HadoopJobConfiguration config = new HadoopJobConfiguration();
                config.InputPath = "Input/wordcount";
                config.OutputFolder = "Output/wordcount";
                return config;
            }
        }

        static IHadoop connect2Azure()
        {
            return Hadoop.Connect(
                new Uri("https://reiszeast.azurehdinsight.net:563"), "admin", "N0password",
                "reiszeast", "http://reiszeast.blob.core.windows.net/sample",
                "3d0bab5b-fb65-456e-88d1-ed692a127391",
                "sample",
                true
            );
        }

        static void Main(string[] args)
        {
            //var hadoop = Hadoop.Connect();
            var hadoop = connect2Azure();
            var result = hadoop.MapReduceJob.ExecuteJob<WordCountjob>();
            
            Console.In.Read();
        }
    }
}
