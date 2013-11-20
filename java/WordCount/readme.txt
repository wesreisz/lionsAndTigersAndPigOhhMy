Steps:

hadoop fs -rm -r /home/reiszwt/samples/bible/output
mvn clean install
target/
hadoop jar WordCount-0.0.1-SNAPSHOT.jar com.wesleyreisz.bigdata.wordcount.v1.WordCount /home/reiszwt/samples/bible/bible+shakes.nopunc /home/reiszwt/samples/bible/output
hadoop fs -cat /home/reiszwt/samples/bible/output/part-0*
 