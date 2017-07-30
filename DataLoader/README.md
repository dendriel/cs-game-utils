Data Loader

Files:
+ DataLoader.cs
+ update_elems.sh

Data Loader loads a list from a JSON file and create an instance T for each list member. T must
have a constructor that receives a string with a filename. Then, T can load and parse an element
data from this file.

Update elements (update_elems.sh) can be used to create the list parsed by DataLoader.