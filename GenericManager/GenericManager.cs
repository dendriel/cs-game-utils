using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Generic manager that loads data from json files and handles game objects.
	/// 
	/// This is a "two-in-one" that loads data elements from file, create instances to parse/store this data (elemsList) and
	/// spawn game objects in the scene from a given prefab (elemPrefab). *I know that isn't the best idea to mix things up, but
	/// this works like a charm as managers templates.
	/// 
	/// The rationale behind this manager is the following...
	/// 
	/// #1 feature - ElemAttr
	/// for instance, we have a game wich contains units, buildins and tiles.
	/// Each of these elements have pre-defined attributes. We can create a file for each one and writte its attributes in there.
	/// For the (const) data loaded from file, we may create a class (not monobehaviour) like UnitAttr, BuildingAttr or TileAttr.
	/// 
	/// *This manager will load a list from a JSON with the name of data files. For each occurrence will be created a class T1 that
	/// will receive the file name. T1 must load the given file and parse the data by itself.
	/// 
	/// Also, we may create another class to handle the element dynamic data and have a reference for its "base" data..
	/// Unit (which contains a file to UnitAttr), Building (which contains a file to BuildingAttr) or Tile  (which contains a
	/// file to TileAttr). T1 = Tile, Building or Unit; T2 = TileAttr, BuildingAttr or UnitAttr.
	/// 
	/// #2 feature - Elem (which shold contains ElemAttr)
	/// In addition to loading and storing data loaded from files, the manager also instantiates game objects and keep track of them.
	/// 
	/// WARNING: this class is fine for single player games, but won't work for networked games (NetworkBehavoiur won't get along with generic types).
	/// 
	/// </summary>
	/// <typeparam name="T1">For every data file will be an instance of this type to parse/store the data.</typeparam>
	/// <typeparam name="T2">Type to be returned while spawming in-game elements.</typeparam>
	public class GenericManager<T1, T2> : MonoBehaviour
	{
		/// <summary>
		/// List of loaded elements.
		/// </summary>
		protected List<T1> elemsList;

		/// <summary>
		/// Game object to hold in-game elements.
		/// </summary>
		protected GameObject elemHolder;

		/// <summary>
		/// Element prefab.
		/// </summary>
		protected GameObject elemPrefab;

		/// <summary>
		/// List of all created elements.
		/// </summary>
		protected List<GameObject> inGameElems;

		/// <summary>
		/// JSON file that list all elements to be loaded.
		/// </summary>
		protected string elemsListMetadataResPath = "";

		/// <summary>
		/// Name of the array in JSON file that cotains the list.
		/// </summary>
		protected string elemsListArrayName = "elems";

		/// <summary>
		/// Name of the Holder GO for elements created by this manager.
		/// </summary>
		protected string elemsHolderName = "";

		/// <summary>
		/// Name of the element prefab to be instantiated by this manager.
		/// </summary>
		protected string elemPrefabResPath = "";

		// Use this for initialization
		protected virtual void Start()
		{
			elemPrefab = (elemPrefabResPath != "") ? Resources.Load(elemPrefabResPath) as GameObject : null;
			elemHolder = (elemsHolderName != "") ? new GameObject(elemsHolderName) : null;
			inGameElems = new List<GameObject>();

			elemsList = DataLoader.LoadElems<T1>(elemsListMetadataResPath, elemsListArrayName);
		}

		/// <summary>
		/// Create a new element in the game.
		/// </summary>
		/// <param name="px">Horizontal position.</param>
		/// <param name="py">Vertical position.</param>
		/// <returns>The new element.</returns>
		public virtual T2 SpawnElem(float px, float py)
		{
			Assert.IsNotNull<GameObject>(elemPrefab, "Manager prefab was not initialized!");
			Assert.IsNotNull<GameObject>(elemHolder, "Elements holder was not initialized!");

			//elemPrefab.transform.position = new Vector3(px, py);
			//NetworkServer.Spawn(elemPrefab);

			GameObject newElemGO = Instantiate(elemPrefab, new Vector3(px, py), Quaternion.identity);
			newElemGO.transform.parent = elemHolder.transform;
			inGameElems.Add(newElemGO);

			return newElemGO.GetComponent<T2>();
		}


		/// <summary>
		/// Find a element in the given position.
		/// </summary>
		/// <param name="pos">The position of the element to be retrieved.</param>
		/// <returns>The element at the given position.</returns>
		public virtual T2 GetElemAtPos(Vector3 pos)
		{
			for (int i = 0; i < inGameElems.Count; i++) {
				if (inGameElems[i].transform.position == pos) {
					return inGameElems[i].GetComponent<T2>();
				}
			}

			throw new UnityException("Trying to retrieve an invalid element. Pos: " + pos);
		}
	}
}