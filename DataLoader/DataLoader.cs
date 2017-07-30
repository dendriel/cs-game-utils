/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 *
 *	This file is part of Data Loader.
 *	
 *	Data Loader is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Data Loader is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Data Loader. If not, see<http://www.gnu.org/licenses/>.
 */

using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Load data from JSON file.
	/// </summary>
	public static class DataLoader
	{
		/// <summary>
		/// Load data from JSON files and create corresponding classes to hold the data.
		/// </summary>
		/// <param name="metadataResPath">The path to the JSON with a list of all data files to be loaded.</param>
		/// <param name="arrayTagName">Tag from the list with data filenames inside JSON.</param>
		/// <typeparam name="T">Must have a constructor T(string) that will receive the name of the file
		/// from where data must be loaded and parsed.</typeparam>
		/// <returns>A list with loaded data.</returns>
		public static List<T> LoadElems<T>(string metadataResPath, string arrayTagName = "elems")
		{
			List<T> elemsList = new List<T>();

			TextAsset rawDataFile = Resources.Load(metadataResPath) as TextAsset;
			string rawData = rawDataFile.text;
			JSONNode data = JSON.Parse(rawData);

			int listLength = data[arrayTagName].Count;
			for (int i = 0; i < listLength; i++) {
				elemsList.Add((T)Activator.CreateInstance(typeof(T), args: (string)data[arrayTagName][i]));
			}

			return elemsList;
		}
	}
}
