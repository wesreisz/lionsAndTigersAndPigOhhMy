DATASET_FOLDER=/Users/wesleyreisz/work/fetchfiles

for i in {1901..1920}
do
 cd $DATASET_FOLDER
 mkdir files/$i
 wget -r -np -nH .cut-dirs=3 -R index.html ftp://ftp.ncdc.noaa.gov/pub/data/noaa/$i/
 cd pub/data/noaa/$i/
 cp *.gz $DATASET_FOLDER/files/$i/
 rm -r $DATASET_FOLDER/pub/
done
