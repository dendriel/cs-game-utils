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
using BehaviorLibrary;
using BehaviorLibrary.Components.Actions;
using System;
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Pick the next destination from a route. The route is defined as an array of points of type T.
	/// </summary>
	public class PickNextDestnAction<T> : BehaviorAction
	{
		/// <summary>
		/// A Function to retrieve a update list with the points to be visited.
		/// </summary>
		Func<T[]> GetDestnPoints;

		/// <summary>
		/// The current point index to be visited.
		/// </summary>
		Action<int> SetCurrDestnIdx;
		Func<int> GetCurrDestnIdx;

		/// <summary>
		/// If loop flag is false, the counter will decrease instead of being reset to zero when it reachs the limit.
		/// </summary>
		bool loop;

		/// <summary>
		/// Destination increase unity step.
		/// </summary>
		int step;

		/// <summary>
		/// Create a new PickNextDestnAction.
		/// </summary>
		/// <param name="_GetDestnPoints">A Function to retrieve a updated list with the points to be visited.</param>
		/// <param name="_GetCurrDestnIdx">A method to get the current destination index.</param>
		/// /// <param name="_SetCurrDestnIdx">A method to set the current destination index.</param>
		/// <param param name="_loop">true: reset the index when it reaches the limit; false: start counting back when the index reaches the limit.</param>
		/// <param name="_step">Index increment step.</param>
		public PickNextDestnAction(Func<T[]> _GetDestnPoints, Func<int> _GetCurrDestnIdx, Action<int> _SetCurrDestnIdx, bool _loop = true, int _step = 1)
		{
			GetDestnPoints = _GetDestnPoints;
			GetCurrDestnIdx = _GetCurrDestnIdx;
			SetCurrDestnIdx = _SetCurrDestnIdx;
			loop = _loop;
			step = _step;

			_Action = PickNextDestnExec;
		}

		BehaviorReturnCode PickNextDestnExec()
		{
			T[] destnPoints = GetDestnPoints();
			int currDestnPointIdx = GetCurrDestnIdx();
			int maxDestnPoints = destnPoints.Length - 1;

			if (loop) {
				if (currDestnPointIdx == maxDestnPoints) {
					currDestnPointIdx = 0; // Reset destn point index.
				} else {
					currDestnPointIdx += step; // Pick next destn point.
				}
			} else {
				if (currDestnPointIdx == 0) {
					step = Mathf.Abs(step); // set a positive step.
				} else if (currDestnPointIdx == maxDestnPoints) {
					step *= -1; // set a negative step.
				}
				// Pick next destn point.
				currDestnPointIdx += step;
			}

			// Set next destn point.
			SetCurrDestnIdx(currDestnPointIdx);

			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils
