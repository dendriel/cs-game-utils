# For the folder where this script is located:
#
# Find each file with ".json" extension and create a list from its names.
# This list will be saved in a file with ${filename}.
#
# The list can be loaded with GenericManager/DataLoader.
#
# *Copy this file to a folder with data files to be loaded and execute it
# every time a new data file is added.

filename="elems.json"
tempfile=${filename}".tmp"

rm -f $filename
rm -f $tempfile # if exists.

echo "{
  \"elems\": [" > $tempfile

for file in ./*; do	
	if [[ $file == *".json" ]]; then
		arr=(${file//./ })
		file=(${arr//\//})
		echo "	"${file}"," >> $tempfile
	fi
done

echo "  ]
}" >> $tempfile

cat $tempfile > $filename

rm -f $tempfile

cat $filename