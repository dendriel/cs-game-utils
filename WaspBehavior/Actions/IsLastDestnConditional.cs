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
	/// Check if the current destination is the last one.
	/// </summary>
	public class IsLastDestnConditional<T> : Conditional
	{
		/// <summary>
		/// A Function to retrieve a update list with the points to be visited.
		/// </summary>
		Func<T[]> GetDestnPoints;

		/// <summary>
		/// The current point index to be visited.
		/// </summary>
		Func<int> GetCurrDestnIdx;

		/// <summary>
		/// Create a new IsLastDestnConditional condition.
		/// </summary>
		/// <param name="_GetDestnPoints">A Function to retrieve a update list with the points to be visited.</param>
		/// <param name="_GetCurrDestnIdx">A Function to retrieve current destination index.</param>
		public IsLastDestnConditional(Func<T[]> _GetDestnPoints, Func<int> _GetCurrDestnIdx)
		{
			GetDestnPoints = _GetDestnPoints;
			GetCurrDestnIdx = _GetCurrDestnIdx;

			_Bool = IsLastDestnTest;
		}

		bool IsLastDestnTest()
		{
			int currDestnIdx = GetCurrDestnIdx();
			int destnsLen = GetDestnPoints().Length;

			//Debug.Log("IsLastDestnTest: " + (currDestnIdx == (GetDestnPoints().Length - 1)) + " - currDesnIdx: " + currDestnIdx + " - Destns: " + GetDestnPoints().Length);

			// Check if there are any destinations to go. Sometimes, the SP can be calculated right after the character arrived the last destination, so won't be anywhere else to go.
			//Debug.Log("Is last destn? curr: " + currDestnIdx  +" - " + ((currDestnIdx == (destnsLen - 1)) || (destnsLen == 0)));
			return ((currDestnIdx == (destnsLen - 1)) || (destnsLen == 0));
		}
	}
} // namespace CSGameUtils
