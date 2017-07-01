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
		/// Offset of the collision box.
		/// </summary>
		Vector2 offset;

		/// <summary>
		/// Target transform.
		/// </summary>
		LayerMask targetLayer;

		/// <summary>
		/// Use a circle as the testing area.
		/// </summary>
		float radius;

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
			offset = Vector2.zero;
			_Bool = IsTargetInRangeTest;
		}

		/// <summary>
		/// Create a new IsTargetInRangeConditional.
		/// </summary>
		/// <param name="_charDriver">The character driver from the onwer of this action.</param>
		/// <param name="_targetLayer">The target layer mask (to check collisions).</param>
		/// <param name="_range">The range (as a box).</param>
		/// <param name="_offset">Add an offset to the collision box position.</param>
		public IsTargetInRangeConditional(ICharacterDriver _charDriver, LayerMask _targetLayer, Vector2 _range, Vector2 _offset)
		{
			charDriver = _charDriver;
			targetLayer = _targetLayer;
			range = _range;
			radius = 0;
			offset = _offset;
			_Bool = IsTargetInRangeTest;
		}


		/// <summary>
		/// Create a new IsTargetInRangeConditional.
		/// </summary>
		/// <param name="_charDriver">The character driver from the onwer of this action.</param>
		/// <param name="_targetLayer">The target layer mask (to check collisions).</param>
		/// <param name="_radius">The range (as a circle).</param>
		/// <param name="_offset">Add an offset to the collision box position.</param>
		public IsTargetInRangeConditional(ICharacterDriver _charDriver, LayerMask _targetLayer, float _radius, Vector2 _offset)
		{
			charDriver = _charDriver;
			targetLayer = _targetLayer;
			radius = _radius;
			range = Vector2.zero;
			offset = _offset;
			_Bool = IsTargetInRangeTest;
		}

		bool IsTargetInRangeTest()
		{
			RaycastOrigins raycastOrigins = charDriver.GetRaycastOrigins();
			bool isFacingRight = charDriver.IsFacingRight();
			Vector2 center = ((isFacingRight) ? raycastOrigins.rightCenter : raycastOrigins.leftCenter);

			// Add the offset.
			center.x += offset.x * ((isFacingRight) ? 1 : -1);
			center.y += offset.y;

			Vector2 dir = ((isFacingRight) ? Vector2.right : Vector2.left);

			RaycastHit2D hit;
			if (radius == 0) {
				// Align the cast to one of the sides.
				center.x += (range.x / 2) * ((isFacingRight) ? 1 : -1);
				hit = Physics2D.BoxCast(center, range, 0, dir, 0, targetLayer);
			} else {
				// Align the cast to one of the sides.
				center.x += (radius / 2) * ((isFacingRight) ? 1 : -1);
				hit = Physics2D.CircleCast(center, radius, dir, 0, targetLayer);
			}

			//Debug.Log("Hit: " + hit.transform);
			
			// May be necessary to check if the target is alive!
			return (hit.transform != null);
		}
	}
} // namespace CSGameUtils
