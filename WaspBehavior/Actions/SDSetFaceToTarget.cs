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
	/// Turn the character face to the target.
	/// </summary>
	public class SDSetFaceToTarget : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		protected ICharacterDriver charDriver;

		/// <summary>
		/// A Func to retrieve the current target.
		/// </summary>
		Func<Vector3> GetTargetFunc;

		/// <summary>
		/// Turn the back to the target instead of the face.
		/// </summary>
		bool reverse;

		/// <summary>
		/// Create a new AttackAAction.
		/// </summary>
		/// <param name="_charDriver">The driver of the character that will perform the attack.</param>
		/// <param name="_GetTargetFunc">A Func to retrieve the current target transform.</param>
		/// <param name="_reverse">Set the back to the target isnted of the face?</param>
		public SDSetFaceToTarget(ICharacterDriver _charDriver, Func<Vector3> _GetTargetFunc, bool _reverse = false)
		{
			charDriver = _charDriver;
			GetTargetFunc = _GetTargetFunc;
			reverse = _reverse;

			_Action = SDSetFaceToTargetExec;
		}
    
		protected virtual BehaviorReturnCode SDSetFaceToTargetExec()
		{
			float originX = charDriver.GetPosition().x;
			float targetX = GetTargetFunc().x;
			
			// If target is to the right and we are not facing right.
			if ((((targetX >= originX) && !charDriver.IsFacingRight()) ||
				// If target is to the left and we are facing right
				(targetX < originX && charDriver.IsFacingRight()))) {
				
				// If we entered in this block, the character isn't facing its target. So we need to check
				// if the reverse flag is set (which means that we don't need to face the target.
				if (!reverse) {
					charDriver.Flip();
				}
			}
			
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils

