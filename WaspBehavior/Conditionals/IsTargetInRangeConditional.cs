﻿/**
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
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Check if the target is inside the given range.
	/// 
	/// The range vector allows to test if the target is inside a "box" in front of the character.
	///      ______  x,y dim from "range" parameter.
	///  _0_| Test |
	///  || | Area |
	///  /\ |______|
	/// </summary>
	public class IsTargetInRangeConditional : Conditional
	{
		/// <summary>
		/// Character driver (onwer of the condition).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// Range to check. X is the horizontal range. Y is the vertical interval.
		/// </summary>
		Vector2 range;

		/// <summary>
		/// Target transform.
		/// </summary>
		LayerMask targetLayer;

		/// <summary>
		/// Create a new IsTargetInRangeConditional.
		/// </summary>
		/// <param name="_charDriver">The character driver from the onwer of this action.</param>
		/// <param name="_targetLayer">The target layer mask (to check collisions).</param>
		/// <param name="_range">The range (as a box).</param>
		public IsTargetInRangeConditional(ICharacterDriver _charDriver, LayerMask _targetLayer, Vector2 _range)
		{
			charDriver = _charDriver;
			targetLayer = _targetLayer;
			range = _range;
			_Bool = IsTargetInRangeTest;
		}

		bool IsTargetInRangeTest()
		{
			RaycastOrigins raycastOrigins = charDriver.GetRaycastOrigins();
			bool isFacingRight = charDriver.IsFacingRight();
			Vector2 center = ((isFacingRight) ? raycastOrigins.rightCenter : raycastOrigins.leftCenter);

			// Align the box to one of the sides.
			center.x += (range.x / 2) * ((isFacingRight) ? 1 : -1);

			// Check if the target is inside range.
			RaycastHit2D hit = Physics2D.BoxCast(center, range, 0, ((isFacingRight) ? Vector2.right : Vector2.left), 0, targetLayer);

			return (hit.transform != null);
		}
	}
} // namespace CSGameUtils