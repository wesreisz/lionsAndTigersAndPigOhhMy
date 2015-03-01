package com.wesleyreisz.lionsAndTigersAndPig;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.Arrays;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.FileUtil;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.IntWritable;
import org.apache.hadoop.io.LongWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapred.OutputLogFilter;
import org.apache.hadoop.mrunit.mapreduce.MapDriver;
import org.apache.hadoop.mrunit.mapreduce.ReduceDriver;
import org.hamcrest.Matchers;
import org.junit.Assert;
import org.junit.Test;

public class MaxTemperatureMapperTest {
	@Test
	public void processesValidRecord1() throws IOException, InterruptedException {
		Text value = new Text("0029029070999991904010106004+64333+023450FM-12+000599999V0203401N009819999999N0000001N9+00111+99999101741ADDGF100991999999999999999999"); // Temperature

		new MapDriver<LongWritable, Text, Text, IntWritable>()
				.withMapper(new MaxTemperatureMapper())
				.withInputValue(value)
				.withOutput(new Text("1904"), new IntWritable(11))
				.runTest();
	}

	@Test
	public void processesValidRecord2() throws IOException, InterruptedException {
		Text value = new Text("0029029070999991904010213004+64333+023450FM-12+000599999V0202001N008219999999N0000001N9-00281+99999102631ADDGF108991999999999999999999"); // Temperature

		new MapDriver<LongWritable, Text, Text, IntWritable>()
				.withMapper(new MaxTemperatureMapper())
				.withInputValue(value)
				.withOutput(new Text("1904"), new IntWritable(-28))
				.runTest();
	}

	@Test
	public void processesValidRecord3() throws IOException, InterruptedException {
		Text value = new Text("0029029720999991904010220004+60450+022267FM-12+001499999V0209991C000019999999N0000001N9-01111+99999102961ADDGF100991999999999999999999"); // Temperature

		new MapDriver<LongWritable, Text, Text, IntWritable>()
				.withMapper(new MaxTemperatureMapper())
				.withInputValue(value)
				.withOutput(new Text("1904"), new IntWritable(-111))
				.runTest();
	}

	@Test
	public void processesValidRecord() throws IOException, InterruptedException {
		Text value = new Text(
				"0043011990999991950051518004+68750+023550FM-12+0382" +    // Year
		                      //^^^^ this is the year
			    "99999V0203201N00261220001CN9999999N9-00111+99999999999"); // Temperature
		                  // this is the temperature ^^^^^
		
		new MapDriver<LongWritable, Text, Text, IntWritable>()
				.withMapper(new MaxTemperatureMapper())
				.withInputValue(value)
				.withOutput(new Text("1950"), new IntWritable(-11))
				.runTest();
	}
	@Test
	public void ignoresMissingTemperatureRecord() throws IOException, InterruptedException {
		Text value = new Text("0043011990999991950051518004+68750+023550FM-12+0382" +
				"99999V0203201N00261220001CN9999999N9+99991+99999999999");

		new MapDriver<LongWritable, Text, Text, IntWritable>()
			.withMapper(new MaxTemperatureMapper()) 
			.withInputValue(value)
			.runTest();
	}
	
	@Test
	public void returnsMaximumIntegerInValues() throws IOException,InterruptedException {
		new ReduceDriver<Text, IntWritable, Text, IntWritable>()
			.withReducer(new MaxTemperatureReducer())
			.withInputKey(new Text("1950"))
			.withInputValues(Arrays.asList(new IntWritable(10), new IntWritable(5),new IntWritable(20))) 
			.withOutput(new Text("1950"), new IntWritable(20))
			.runTest();
	}
	
	@Test
	public void test() throws Exception {
		//From config file...
		//hadoop com.wesleyreisz.lionsAndTigersAndPig.MaxTemperatureDriver 
		  //-fs file:/// 
		  //-Dmapreduce.framework.name=local 
		  //-Dyarn.resourcemanager.address=local 
		  //-Dmapreduce.jobtracker.address=local 
		  //../input/ncdc/micro/ output

		Configuration conf = new Configuration(); 
		conf.set("fs.default.name", "file:///"); 
		conf.set("mapreduce.framework.name", "local");
		conf.set("yarn.resourcemanager.address", "local");
		conf.set("mapreduce.jobtracker.address", "local");
		
		Path input = new Path("../input/ncdc/micro/"); 
		Path output = new Path("output");
		
		FileSystem fs = FileSystem.getLocal(conf); 
		fs.delete(output, true); // delete old output
	
		MaxTemperatureDriver driver = new MaxTemperatureDriver(); 
		driver.setConf(conf);
		
		int exitCode = driver.run(new String[] { input.toString(), output.toString() });
		
		Assert.assertThat(exitCode, Matchers.is(0));
		
		checkOutput(conf, output); 
	}
	private void checkOutput(Configuration conf, Path output) throws IOException {
		String location =  MaxTemperatureMapperTest.class
								.getProtectionDomain()
								.getCodeSource()
								.getLocation()
								.getPath();
	    
		FileSystem fs = FileSystem.getLocal(conf);
	    Path[] outputFiles = FileUtil.stat2Paths(
	        fs.listStatus(output, new OutputLogFilter()));
	    Assert.assertThat(outputFiles.length, Matchers.is(2));

	    BufferedReader actual = null;
	    for(Path outputFile: outputFiles){
	    	if (outputFile.getName().contains("part-")){
	    		System.out.println("input file: " + outputFile.toString().replace("file:",""));
	    		actual = new BufferedReader(
	    			new FileReader(outputFile.toString().replace("file:", ""))
	    		);
	    	}
	    }
	    System.out.println("output file: " + location);
	    BufferedReader expected =  new BufferedReader(
	    		new FileReader(location+"expected.txt")
	    	);
	    		
	    String expectedLine;
	    while ((expectedLine = expected.readLine()) != null) {
	    	Assert.assertThat(actual.readLine(), Matchers.is(expectedLine));
	    }
	    Assert.assertThat(actual.readLine(), Matchers.nullValue());
	    actual.close();
	    expected.close();
	  }
}
	
