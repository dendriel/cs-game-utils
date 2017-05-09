/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	Tiled Map Parser is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Tiled Map Parser is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Tiled Map Parser. If not, see<http://www.gnu.org/licenses/>.
 */
using SimpleJSON;
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Provides an example in how to use the TileMapParser.
	/// </summary>
	public class TiledMapParserExample : MonoBehaviour
	{
		string mapToLoad = "TestingJSON";
		int mapWidth;
		int mapHeight;
		int tileWidth;
		int tileHeight;
		string moveableProperty = "Moveable";
		string weightProperty = "Weight";

		/// <summary>
		/// Provide information about the moveable property of the tiles.
		/// </summary>
		bool[,] moveableTilesMatrix;

		/// <summary>
		/// Provide information about weight of the tiles.
		/// </summary>
		int[,] tilesWeightMatrix;

		void Start ()
		{
			/**
			 * To run this example, it's necessary to create a "Resources" folder under "Assets" and create
			 * a copy of TestingJSON.json in there. "Assets/Resources/TestingJSON.json"
			 */
			TextAsset rawDataFile = Resources.Load(mapToLoad) as TextAsset;
			string rawData = rawDataFile.text;
			JSONNode data = JSON.Parse(rawData);

			/**
			 * FIRST of all, it's necessary to setup the JSON root node in TiledMapParser.
			 * 
			 * *because TiledMapParser is a static class, there is no need to instantiate it, nor save its reference.*
			 */
			TiledMapParser.SetupRootNode(data);

			// Retrieve basic attributes (that are nativelly provided by Tiled maps).
			mapWidth = TiledMapParser.GetMapWidth();
			mapHeight = TiledMapParser.GetMapHeight();
			tileWidth = TiledMapParser.GetTileWidth();
			tileHeight = TiledMapParser.GetTileHeight();

			Debug.Log("Current map width: " + mapWidth + " - height: " + mapHeight + " - tile width: " + tileWidth + " - tile height: " + tileHeight);

			ParseLayers(data);
		}

		/// <summary>
		/// Retrieve each map layer and forward it to the appropriate function.
		/// </summary>
		/// <param name="data">The map's data to be parsed.</param>
		void ParseLayers(JSONNode data)
		{
			// Retrieve layers property (also provided by Tiled maps).
			int layersCount = TiledMapParser.GetLayersCount();

			Debug.Log("Amount of layers is: " + layersCount);


			// Create the moveable matrix. This is used to check what tile is moveable (also used to calculate Shortest Path).
			moveableTilesMatrix = new bool[mapWidth, mapHeight];
			// Create the moveable matrix. This is used to check what is the tile weight (used to calculate Shortest Path).
			tilesWeightMatrix = new int[mapWidth, mapHeight];

			// Process each layer.
			for (int i = 0; i < layersCount; i++) {

				/**
				 * SECOND, we need to set the layer we are parsing. We already set the data we are processing in SetupRootNode(),
				 * now we set the layer to be worked.
				 */
				TiledMapParser.SetupCurrentLayer(i);

				// Retrieve layer property (also provided by Tiled maps).
				string layerName = TiledMapParser.GetLayerName();

				Debug.Log("Parsing layer: \"" + layerName + "\"");

				switch (layerName) {
					case "TilesLayer0":
					case "TilesLayer1":
					case "TilesLayer2":
					Debug.Log("Parse tiles layer: " + layerName);
						ParseTileLayer(i);
					break;
					case "TilesObjectsLayer0":
						// This is much as ParseTileLayer(). We check if there is objects in each map position. In positive case, we
						// retrieve the moveable and weight property. These are "tile objects" and don't interact with the characters.
						//ParseTileObjectLayer(i);
					break;
					case "ObjectsLayer":
						Debug.Log("Parse objects layer: " + layerName);
						ParseObjects(i);
					break;
					case "EventsLayer":
						// Much same as ParseObjects().
						//Debug.Log("Parse events layer: " + EventsLayerIdx);
						//EventsParser.ParseLayer(i);
					break;
					default:
						Assert.IsTrue(false, "Received unhandled layer. Name: " + layerName + " - ID: " + i);
					break;
				}
			}
		}

		/// <summary>
		/// Parse tiles layer.
		/// </summary>
		/// <param name="tilesedIdx">The corresponding tileset index.</param>
		void ParseTileLayer(int tilesedIdx)
		{
			/**
			 * THIRD, we need to set which tileset we should use to retrieve information about tiles/objects in the map.
			 * 
			 * In the generated JSON map from Tiled, we have a matrix that represents "what are" in the scenario. Each
			 * "thing" in the scenario have a unique ID. But this ID is binded to a specific tileset. We need to identify
			 * which tileset is used in which layer so we can retrieve the correct properties for each tile/object.
			 */
			TiledMapParser.SetupCurrentTileset(tilesedIdx);

			// Retrieve node properties.
			for (int tilePosY = 0; tilePosY < mapHeight; tilePosY++) {
				for (int tilePosX = 0; tilePosX < mapWidth; tilePosX++) {

					// Find out the tile ID.
					int tileCount = (tilePosY * mapWidth) + tilePosX;
					int tileID = TiledMapParser.GetTileID(tileCount) - 1; // 0 is reserved for empty tiles and doesn't appear in the layers data.

					// Check and process the Moveable Property (custom property. Informs if a character can walk over the tile).
					if (TiledMapParser.TileHasProperty(tileID, moveableProperty)) {
						// If the property was found for this tile, retrieve it!
						moveableTilesMatrix[tilePosX, tilePosY] = TiledMapParser.GetTileBoolProperty(tileID, moveableProperty);
					}

					// Check and process the Moveable Property (custom property. Informs the tile's weight).
					if (TiledMapParser.TileHasProperty(tileID, weightProperty)) {
						tilesWeightMatrix[tilePosX, tilePosY] = TiledMapParser.GetTileIntProperty(tileID, weightProperty);
					}
				}
			}
		}

		/// <summary>
		/// Parse objects from the map.
		/// </summary>
		/// <param name="tilesedIdx">The corresponding tileset index.</param>
		public void ParseObjects(int tilesedIdx)
		{
			/**
			 * Set tileset index for this layer.
			 */
			TiledMapParser.SetupCurrentTileset(tilesedIdx);

			// Assert layer name.
			Assert.AreEqual<string>("ObjectsLayer", TiledMapParser.GetLayerName());

			// "objects" is also a special layer from tile that we can create to spawn objects.
			int objCount = TiledMapParser.GetLayerObjectsCount();
			for (int objIdx = 0; objIdx < objCount; objIdx++) {

				/**
				 * WHILE parsing objects, we have the object index in our JSON data file, but we must find the corresponding
				 * tileset identifier for the object in order to retrieve its properties.
				 */
				int tileID = TiledMapParser.GetLayerObjectTileID(objIdx);

				// Some shared properties provided by Tiled.
				int objRot = TiledMapParser.GetObjectRotation(objIdx);

				// FindObjectRealX/Y allows to find the corresponding X and Y position of the object in the Unity coordinates.
				// The object is put in the center of the tile.
				int objPosX = TiledMapParser.FindObjectRealX(objIdx, mapWidth, tileWidth);
				int objPosY = TiledMapParser.FindObjectRealY(objIdx, mapWidth, tileWidth);
				
				Debug.Log("Spawn: " + tileID + " pos X: " + objPosX + " - pos Y: " + objPosY + " - obj rot: " + objRot);

				/** This is very specific stuff. I want to show two things here:
				 * 
				 * Here, we can parse a string property directly as an Enumerator.
				 * 
				 * ObjectType objType = (ObjectType)Enum.Parse(typeof(ObjectType), TiledMapParser.GetTileStringProperty(tileID, ObjectTypeProperty));
				 */

				 /** Here, we are retrieving a unique property from the object. This property does NOT come from the tileset, but from object's data.
				 * Unique properties are set directly in the object in the map. It's useful to set custom properties for each object (as the kind of
				 * items a chest will drop when opened).
				 * 
				 */ int objAmount = TiledMapParser.GetObjectIntUniqueProperty(objIdx, "Amount");
				Debug.Log("Object amount: " + objAmount);
			}

			Debug.Log("Total parsed objects: " + objCount);
		}
	}
} // namespace CSGameUtils