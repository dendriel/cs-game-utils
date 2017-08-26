/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Wasp Behavior.
 *
 *	Wasp Behavior is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Wasp Behavior is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Wasp Behavior. If not, see<http://www.gnu.org/licenses/>.
 */
using BehaviorLibrary.Components.Conditionals;
using System;
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Conditional to check if current bool is true/false.
	/// 
	/// Useful to trigger a part of the sequence only once. Use in combination to SetBoolAction.
	/// </summary>
	public class IsBoolConditional : Conditional
	{
		/// <summary>
		/// Function to retrieve a bool.
		/// </summary>
		Func<bool> GetBool;

		/// <summary>
		/// Create a new IsBoolConditional.
		/// </summary>
		/// <param name="getBool">Function to retrieve the bool to be tested.</param>
		public IsBoolConditional(Func<bool> _GetBool)
		{
			GetBool = _GetBool;
			_Bool = IsBoolTestTest;
		}

		bool IsBoolTestTest()
		{
			return GetBool();
		}
	}
} // namespace CSGameUtils