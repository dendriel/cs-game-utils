Tiled Map Parser

Files:
+ TiledMapParser.cs
+ TiledMapParserExample.cs
+ TestingJSON.json

TiledMapParser aims to provide facilities for parsing JSONs maps generated from Tiled Map Editor.

Check out TiledMapParserExample.cs for an example in how to use Tiled Map Parser.

The following properties are provided:

- Retrieve map properties
	* map width and height
	* tile width and height
	* Layers count
	* Layers name
	* Layer objects count
- Retrieve tile properties
	* Tile ID
	* Check if tile has a property
	* Get tile property (int, bool and string. But you can easily create new getters with different types)
- Retrieve objects properties
	* Object ID and GID
	* Object X and Y
	* Object rotation
	* Fixed properties from the tileset (same method as for retrieving tile properties)
- Retrive objects unique properties (properties you set directily in an object)
	* Check if object have a unique property
	* Retrieve the object unique property (int, bool and string. But you can easily create new getters with different types)
- Find object real X and Y
	* Translate the object tile coordinates to Unity coordinates
- Fix object rotation
	* Find object X and Y offset in order to fix its position if rotated. When a object is rotated in Tiled, its sprite is displayed with another rotation, but also in another tile. Even if the sprite is in another tile, its position doesnt really change, because its rotate around an axis that is not its center. These methos allows to find out the position in with the sprite is displayed.


