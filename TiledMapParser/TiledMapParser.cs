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
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Provide methods to easy parsing tiles, objects and events from Tiled maps.
	/// 
	/// TiledMapParser uses SimpleJSON implementation from Bunny83. SimpleJSON source
	/// code can be found at https://github.com/Bunny83/SimpleJSON.
	/// 
	/// </summary>
	public static class TiledMapParser
	{
		/// <summary>
		/// Current root node.
		/// </summary>
		static JSONNode root;
    
		/// <summary>
		/// Current parsing layer index.
		/// </summary>
		static int layerIdx = 0;

		/// <summary>
		/// Current parsing layer.
		/// </summary>
		static JSONNode layerRoot { get { return root["layers"][layerIdx]; } }

		/// <summary>
		/// Current parsing layer's data.
		/// </summary>
		static JSONNode layerData { get { return layerRoot["data"]; } }

		/// <summary>
		/// Current parsing layer's objects.
		/// </summary>
		static JSONNode layerObjects { get { return layerRoot["objects"]; } }

		/// <summary>
		/// Current parsing tileset index.
		/// </summary>
		static int tilesetIdx = 0;

		/// <summary>
		/// Current parsing tileset.
		/// </summary>
		static JSONNode tilesetRoot { get { return root["tilesets"][tilesetIdx]; } }

		/// <summary>
		/// Current parsing tileset tile's properties.
		/// </summary>
		static JSONNode tilesetTileProperties { get { return tilesetRoot["tileproperties"]; } }
		
		/// <summary>
		/// Setup the JSON root node.
		/// </summary>
		/// <param name="_root">The node to be used while parsing.</param>
		public static void SetupRootNode(JSONNode _root)
		{
			root = _root;
		}

		/// <summary>
		/// Define the current layer to be parsed.
		/// </summary>
		/// <param name="_layerIdx">The index of the layer to be parsed.</param>
		public static void SetupCurrentLayer(int _layerIdx)
		{
			layerIdx = _layerIdx;
		}

		/// <summary>
		/// Define the current tileset to be used in parsing.
		/// </summary>
		/// <param name="_tilsetIdx">The index of the tileset to be used.</param>
		public static void SetupCurrentTileset(int _tilsetIdx)
		{
			tilesetIdx = _tilsetIdx;
		}

		/// <summary>
		/// Retrieve the map's width.
		/// </summary>
		/// <returns>The map width.</returns>
		public static int GetMapWidth()
		{
			Assert.IsNotNull(root, "Must first setup the root node before parsing the data.");
			return Convert.ToInt32((int)root["width"]);
		}

		/// <summary>
		/// Retrieve the map's height.
		/// </summary>
		/// <returns>The map height.</returns>
		public static int GetMapHeight()
		{
			Assert.IsNotNull(root, "Must first setup the root node before parsing the data.");
			return Convert.ToInt32((int)root["height"]);
		}

		/// <summary>
		/// Retrieve the map's tile width.
		/// </summary>
		/// <returns>The tiles width.</returns>
		public static int GetTileWidth()
		{
			Assert.IsNotNull(root, "Must first setup the root node before parsing the data.");
			return Convert.ToInt32((int)root["tilewidth"]);
		}

		/// <summary>
		/// Retrieve the map's tile height.
		/// </summary>
		/// <returns>The tiles height.</returns>
		public static int GetTileHeight()
		{
			Assert.IsNotNull(root, "Must first setup the root node before parsing the data.");
			return Convert.ToInt32((int)root["tileheight"]);
		}

		/// <summary>
		/// Retrive the map's layers amount.
		/// </summary>
		/// <returns>The amount of layers in the map.</returns>
		public static int GetLayersCount()
		{
			return Convert.ToInt32(root["layers"].Count);
		}

		/// <summary>
		/// Retrieve the current layer name.
		/// </summary>
		/// <returns>The current layer name.</returns>
		public static string GetLayerName()
		{
			return Convert.ToString((string)layerRoot["name"]);
		}

		/// <summary>
		/// Retrieve the current layer objects amount.
		/// </summary>
		/// <returns>The current layer objects amount.</returns>
		public static int GetLayerObjectsCount()
		{
			return Convert.ToInt32(layerObjects.Count);
		}

		/// <summary>
		/// Retrieve the object GID.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object GID.</returns>
		public static int GetObjectGID(int objectIdx)
		{
			return Convert.ToInt32((int)layerObjects[objectIdx]["gid"]);
		}

		/// <summary>
		/// Retrieve the object ID.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object ID.</returns>
		public static int GetObjectID(int objectIdx)
		{
			return Convert.ToInt32((int)layerObjects[objectIdx]["id"]);
		}

		/// <summary>
		/// Retrieve the object rotation.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object rotation.</returns>
		public static int GetObjectRotation(int objectIdx)
		{
			return (int)Convert.ToSingle((int)layerObjects[objectIdx]["rotation"]);
		}

		/// <summary>
		/// Retrieve the object x value. Don't forget to add FixRotX() offset to find the real object X.
		/// When rotation the object from tiled, the object sprite moves around, but its axis doesn't follow
		/// the sprite. Thus, the X position may not correspont to the object sprite appearing in the map.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object x value.</returns>
		public static float GetObjectX(int objectIdx)
		{
			return Convert.ToSingle((int)layerObjects[objectIdx]["x"]);
		}

		/// <summary>
		/// Retrieve the object y value.
		/// When rotation the object from tiled, the object sprite moves around, but its axis doesn't follow
		/// the sprite. Thus, the Y position may not correspont to the object sprite appearing in the map.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object y value.</returns>
		public static float GetObjectY(int objectIdx)
		{
			return Convert.ToSingle((int)layerObjects[objectIdx]["y"]);
		}

		/// <summary>
		/// Retrieve the object identifier to access the common object properties.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The id to access the object data in its tileset.</returns>
		public static int GetLayerObjectTileID(int objectIdx)
		{
			return GetObjectGID(objectIdx) - GetTilesetFirstGID();
		}

		/// <summary>
		/// Checks if the unique property exists for the given object.
		/// </summary>
		/// <param name="objectIdx">The object identifier.</param>
		/// <param name="property">The property to check.</param>
		/// <returns>true if the property exists; false otherwise.</returns>
		public static bool ObjectHasUniqueProperty(int objectIdx, string property)
		{
			JSONNode propertyNode = layerObjects[objectIdx]["properties"][property];
			return (propertyNode != null);
		}

		/// <summary>
		/// Retrieve a unique property of type T from an object in the current layer.
		/// </summary>
		/// <param name="objectIdx">The object identifier.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <param param name="ConvFunc">The method to convert the Node to T type.</param>
		/// <returns>The requested object property.</returns>
		public static T GetObjectUniqueProperty<T>(int objectIdx, string property, Func<string, T> ConvFunc)
		{
			return ConvFunc(layerObjects[objectIdx]["properties"][property]);
		}

		/// <summary>
		/// Retrieve an integer property from an object in the current layer.
		/// </summary>
		/// <param name="objectIdx">The object identifier.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <returns>The requested object property.</returns>
		public static int GetObjectIntUniqueProperty(int objectIdx, string property)
		{
			return GetObjectUniqueProperty<int>(objectIdx, property, Convert.ToInt32);
		}

		/// <summary>
		/// Retrieve a bool property from an object in the current layer.
		/// </summary>
		/// <param name="objectIdx">The object identifier.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <returns>The requested object property.</returns>
		public static bool GetObjectBoolUniqueProperty(int objectIdx, string property)
		{
			return GetObjectUniqueProperty<bool>(objectIdx, property, Convert.ToBoolean);
		}

		/// <summary>
		/// Retrieve a string property from an object in the current layer.
		/// </summary>
		/// <param name="objectIdx">The object identifier.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <returns>The requested object property.</returns>
		public static string GetObjectStringUniqueProperty(int objectIdx, string property)
		{
			return GetObjectUniqueProperty<string>(objectIdx, property, Convert.ToString);
		}


		/// <summary>
		/// Retrive the tile ID at the given index from the current parsing layer.
		/// </summary>
		/// <param name="idx">The tile index.</param>
		/// <returns>The tile ID.</returns>
		public static int GetTileID(int idx)
		{
			return Convert.ToInt32((int)layerData[idx]);
		}

		/// <summary>
		/// Retrieve the current tilset first GID.
		/// </summary>
		/// <returns>The current tileset first GID.</returns>
		public static int GetTilesetFirstGID()
		{
			return Convert.ToInt32((int)tilesetRoot["firstgid"]);
		}

		/// <summary>
		/// Checks if the property exists for the given tileID.
		/// </summary>
		/// <param name="tileID">The tile ID.</param>
		/// <param name="property">The property to check.</param>
		/// <returns>true if the property exists; false otherwise.</returns>
		public static bool TileHasProperty(int tileID, string property)
		{
			JSONNode propertyNode = tilesetTileProperties[tileID.ToString()][property];
			return (propertyNode != null);
		}

		/// <summary>
		/// Retrieve a property of type T from a tile in the current tileset.
		/// </summary>
		/// <param name="tileID">The tile ID.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <param param name="ConvFunc">The method to convert the Node to T type.</param>
		/// <returns>The requested tile property.</returns>
		public static T GetTileProperty<T>(int tileID, string property, Func<string, T> ConvFunc)
		{
			return ConvFunc(tilesetTileProperties[tileID.ToString()][property]);
		}

		/// <summary>
		/// Retrieve an integer property from a tile in the current tileset.
		/// </summary>
		/// <param name="tileID">The tile ID.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <returns>The requested tile property.</returns>
		public static int GetTileIntProperty(int tileID, string property)
		{
			return GetTileProperty<int>(tileID, property, Convert.ToInt32);
		}

		/// <summary>
		/// Retrieve a boolean property from a tile in the current tileset.
		/// </summary>
		/// <param name="tileID">The tile ID.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <returns>The requested tile property.</returns>
		public static bool GetTileBoolProperty(int tileID, string property)
		{
			return GetTileProperty<bool>(tileID, property, Convert.ToBoolean);
		}

		/// <summary>
		/// Retrieve a string property from a tile in the current tileset.
		/// </summary>
		/// <param name="tileID">The tile ID.</param>
		/// <param name="property">The property to be retrieved.</param>
		/// <returns>The requested tile property.</returns>
		public static string GetTileStringProperty(int tileID, string property)
		{
			return GetTileProperty<string>(tileID, property, Convert.ToString);
		}

		/// <summary>
		/// Find the appropriate X offset for the given rotation.
		/// 
		/// Rotation an object in Tiled editor changes the imagem position. So, we need to add a position offset so
		/// the moveable property is applied correclty.
		/// </summary>
		/// <param name="rot">The rotation to check.</param>
		/// <returns>The offset to be added to the X object position.</returns>
		public static int FixRotX(int rot)
		{
			if ((rot == 270) || (rot == -90) || (rot == 180) || (rot == -180)) {
				return -1;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// Find the appropriate Y offset for the given rotation.
		/// 
		/// Rotation an object in Tiled editor changes the imagem position. So, we need to add a position offset so
		/// the moveable property is applied correclty.
		/// </summary>
		/// <param name="rot">The rotation to check.</param>
		/// <returns>The offset to be added to the Y object position.</returns>
		public static int FixRotY(int rot)
		{
			if ((rot == 90) || (rot == -270) || (rot == 180) || (rot == -180)) {
				return 1;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// Find the object X position in the map.
		/// 
		/// Allows to find the corresponding X position of the object in the Engine (Unity) coordinates.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object real X position.</returns>
		public static int FindObjectRealX(int objectIdx, int mapWidth, int tileWidth)
		{
			int objRot = GetObjectRotation(objectIdx);
			// Adding half tile width/height set the tile's pivot point to its center, instead of the upper-left corner. Thus, event if
			// the tile have an offset (to create more visual appeal), is map point will be where its center is located.
			return (Mathf.FloorToInt(GetObjectX(objectIdx) + mapWidth / 2) / tileWidth) + FixRotX(objRot);
		}

		/// <summary>
		/// Find the object Y position in the map.
		/// 
		/// Allows to find the corresponding Y position of the object in the Engine (Unity) coordinates.
		/// </summary>
		/// <param name="objectIdx">The objects index.</param>
		/// <returns>The object real Y position.</returns>
		public static int FindObjectRealY(int objectIdx, int mapWidth, int tileWidth)
		{
			int objRot = GetObjectRotation(objectIdx);
			// Adding half tile width/height set the tile's pivot point to its center, instead of the upper-left corner. Thus, event if
			// the tile have an offset (to create more visual appeal), is map point will be where its center is located.
			int realY = (Mathf.FloorToInt(GetObjectY(objectIdx) + mapWidth / 2) / tileWidth) + FixRotY(objRot);
			realY--; // The editor have an vertical offset that makes the first row (the first 64 px) go outside the map.
			return realY;
		}
	}
} // namespace CSGameUtils