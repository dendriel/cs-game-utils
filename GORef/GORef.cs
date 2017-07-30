/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 *
 *	Game Object Reference is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Game Object Reference is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Game Object Reference. If not, see<http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Provides a way to store the reference of game objects that must start disabled.
	/// 
	/// WARNING: it's safer to access these references from OnEnable() on. (or set GORef to start earlier in Script execution order).
	/// </summary>
	public class GORef : MonoBehaviour
	{
		/// <summary>
		/// Store game objects references.
		/// string: GameObject key.
		/// GameObject: reference.
		/// </summary>
		static Dictionary<string, GameObject> GORefs;
		
		/// <summary>
		/// Clears the GO references. Useful when (before) loading a new scene.
		/// </summary>
		/// <returns>The GO references.</returns>
		public static void ClearGORefs()
		{
			GORefs = null;
		}

		/// <summary>
		/// Get the reference from a game object.
		/// </summary>
		/// <param name="key">The key used to refer to the game object.</param>
		/// <returns>The game object reference.</returns>
		public static GameObject GetRef(string key)
		{
			Assert.IsTrue(GORefs != null, "WARNING: GORefs not initialized!");

			Assert.IsTrue(GORefs.ContainsKey(key), "Received an invalid dictionary key. The key is not registered: " + key);
			return GORefs[key];
		}

		/// <summary>
		/// Get a component in the game object referenced by "key".
		/// </summary>
		/// <typeparam name="T">The type of component to retrieve.</typeparam>
		/// <param name="key">The key used to refer to the game object.</param>
		/// <returns>The requested compoenent.</returns>
		public static T GetComponentInRef<T>(string key)
		{
			Assert.IsTrue(GORefs != null, "WARNING: GORefs not initialized!");

			GameObject go = GetRef(key);
			T comp = go.GetComponent<T>();
			if (comp == null) throw new UnityException("Could not retrieve the requested component from the go with key: " + key);

			return comp;
		}

		/// <summary>
		/// Save a reference in the dictionary. May be used from non-monobehavior to store GO references.
		/// 
		/// Updates the key if already in the dictionary.
		/// </summary>
		/// <param name="key">The key used to refer to the game object.</param>
		/// <param name="go">The game object to be saved.</param>
		public static void AddRef(string key, GameObject go)
		{
			Assert.IsTrue(GORefs != null, "WARNING: GORefs not initialized!");

			// This condition requires that every time we load a scene we must call ClearGORef(). Now, we are updating the keys instead of throwing exceptions.
			//Assert.IsFalse(GORefs.ContainsKey(key), "Trying to register an invalid dictionary key. The key is already registered: " + key);

			if (GORefs.ContainsKey(key)) {
				GORefs[key] = go;
			} else {
				GORefs.Add(key, go);
			}
			//Debug.Log ("Added ref: " + key);
		}

		//*************************************************************************************************

		/// <summary>
		/// Use the game object current tag.
		/// </summary>
		[SerializeField]
		[Tooltip("If this option is set, the key field will be ignored.")]
		bool useTag = true;

		/// <summary>
		/// The key used to retrieve this game object.
		/// </summary>
		[SerializeField]
		string key;

		/// <summary>
		/// The object must disable self after saving its reference?
		/// 
		/// Some objects are in a higher hierarchy group. Maybe a parent object must be disabled instead.
		/// This approach doesn't work very well. The parent object may be accessed and disabled before its child
		/// store the GO reference.
		/// </summary>
		[SerializeField]
		bool disableSelf = true;

		/// <summary>
		/// The reference was already registered?
		/// </summary>
		bool alreadyRegistered = false;

		/// <summary>
		/// Register here so another scripts can use the reference from Start() method on.
		/// </summary>
		void Awake()
		{
			if (alreadyRegistered) return;

			// Initialize the dictionary, if needed.
			InitDict();

			if (useTag) {
				key = gameObject.tag;
			} else {
				Assert.IsTrue(key.Length > 0, "Invalid key used to save the GO reference.");
			}

			AddRef(key, gameObject);
			alreadyRegistered = true;

			if (disableSelf) {
				this.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Register OnEnable so another scripts can use the reference from Start() method on.
		/// </summary>
		public static void InitDict()
		{
			// First registration.
			if (GORefs == null) {
				GORefs = new Dictionary<string, GameObject>();
			}
		}
	}
}