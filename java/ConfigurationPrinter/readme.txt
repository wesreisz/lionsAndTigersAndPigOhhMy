 #prints the configuration paramaters for this hadoop
 #cluster
 
 mvn clean install
 export HADOOP_CLASSPATH=target/classes/
 hadoop com.wesleyreisz.lionsAndTigersAndPig.ConfigurationPrinter -conf ~/conf/hadoop-localhost.xml
 hadoop com.wesleyreisz.lionsAndTigersAndPig.ConfigurationPrinter -D color=yellow | grep color 