readme.txt


//call from commandline
mvn clean install
export HADOOP_CLASSPATH=target/classes
echo $HADOOP_CLASSPATH
#hadoop com.wesleyreisz.lionsAndTigersAndPig.MaxTemperatureDriver -fs file:/// -Dmapreduce.framework.name=local -Dyarn.resourcemanager.address=local  -Dmapreduce.jobtracker.address=local ../input/ncdc/micro/ output

#local filesystem
hadoop com.wesleyreisz.lionsAndTigersAndPig.MaxTemperatureDriver -conf=../conf/hadoop-local.xml  ../input/ncdc/all/ output

#localhost
hadoop jar target/hadoop-examples.jar com.wesleyreisz.lionsAndTigersAndPig.MaxTemperatureDriver -conf=../conf/hadoop-localhost.xml /home/reiszwt/ncdc/ /home/reiszwt/ncdc/output


cd output/
ls
