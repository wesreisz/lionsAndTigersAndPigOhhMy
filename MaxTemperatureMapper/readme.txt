readme.txt


//call from commandline
mvn clean install
export HADOOP_CLASSPATH=target/classes
echo $HADOOP_CLASSPATH
hadoop com.wesleyreisz.lionsAndTigersAndPig.MaxTemperatureDriver -fs file:/// -Dmapreduce.framework.name=local -Dyarn.resourcemanager.address=local  -Dmapreduce.jobtracker.address=local ../input/ncdc/micro/ output
cd output/
ls
