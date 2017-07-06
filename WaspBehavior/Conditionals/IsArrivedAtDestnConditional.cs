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
	/// Check if the character arrived at the given destination (pass as transform because it may be switched outside this class).
	/// </summary>
	public class IsArrivedAtDestnConditional : Conditional
	{
		/// <summary>
		/// Offset to be considered close to a destination. Defined arbitrarily (or by some try and error =).
		/// </summary>
		public const float Offset = 0.4f;

		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// A Func to retrieve the current destination.
		/// </summary>
		Func<Vector3> GetDestnFunc;

		/// <summary>
		/// Will check both, horizontal and vertical positioning.
		/// </summary>
		bool checkAxis;

		/// <summary>
		/// Offset to be used while checking if arrived.
		/// </summary>
		float offset;

		/// <summary>
		/// Create a new IsArrivedAtDestnConditional.
		/// </summary>
		/// <param name="_charDriver">The character driver (transform) to check.</param>
		/// <param name="_GetDestnFunc">A Func to retrieve the current destination.</param>
		/// <param name="_checkAxis">Will check both, horizontal and vertical positioning. (default is false; check only
		/// horizontal axis).</param>
		public IsArrivedAtDestnConditional(ICharacterDriver _charDriver, Func<Vector3> _GetDestnFunc, bool _checkAxis = false, float _offset = Offset)
		{
			charDriver = _charDriver;
			GetDestnFunc = _GetDestnFunc;
			checkAxis = _checkAxis;
			offset = _offset;

			_Bool = IsArrivedAtDestinationTest;
		}

		bool IsArrivedAtDestinationTest()
		{
			return IsArrivedAtDestination(GetDestnFunc(), charDriver.GetPosition(), offset, checkAxis);
		}

		/// <summary>
		/// Check if the currPos is close destn;
		/// </summary>
		/// <param name="destn">The destination to check.</param>
		/// <param name="currPos">The position to check.</param>
		/// <param name="offset">Offset to create a range "close to" the destination.</param>
		/// <param name="checkAxis">Check the Y axis.</param>
		/// <returns>True if arrived at the destination; false otherwise.</returns>
		public static bool IsArrivedAtDestination(Vector3 destn, Vector3 currPos, float offset, bool checkAxis = false)
		{
			float minDestnBoundX = destn.x - offset;
			float maxDestnBoundX = destn.x + offset;
			float currPosX = currPos.x;

			//Debug.Log("Destn: " + destn + " - arrived: " + ((currPosX >= minDestnBoundX) && (currPosX <= maxDestnBoundX)));

			// Check if is inside X target.
			if ((currPosX >= minDestnBoundX) && (currPosX <= maxDestnBoundX)) {
				// If must consider Y axis.
				if (checkAxis) {
					float minDestnBoundY = destn.y - offset;
					float maxDestnBoundY = destn.y + offset;
					float currPosY = currPos.y;
					// Check if inside Y target. (already inside X, so return true or false depending on Y).
					return ((currPosY >= minDestnBoundY) && (currPosY <= maxDestnBoundY));
				} else {
					// Doesn't depends on Y target and is already inside X.
					return true;
				}
			} else {
				return false;
			}
		}
	}
} // namespace CSGameUtils
