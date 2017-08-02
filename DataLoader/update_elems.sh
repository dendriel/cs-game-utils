###
 # Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 #
 #	This file is part of Data Loader.
 #	
 #	Data Loader is free software: you can redistribute it and/or modify
 #	it under the terms of the GNU General Public License as published by
 #	the Free Software Foundation, either version 3 of the License, or
 #	(at your option) any later version.
 #
 #	Data Loader is distributed in the hope that it will be useful,
 #	but WITHOUT ANY WARRANTY; without even the implied warranty of
 #	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 #	GNU General Public License for more details.
 #
 #	You should have received a copy of the GNU General Public License
 #	along with Data Loader. If not, see<http://www.gnu.org/licenses/>.
###

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