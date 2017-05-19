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
	/// Side Scrolling Pick a position in front of a target.
	/// </summary>
	public class SDPickPointInFrontOfTargetAction : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		protected ICharacterDriver charDriver;

		/// <summary>
		/// A Func to retrieve the current target.
		/// </summary>
		Func<GameObject> GetTargetFunc;

		/// <summary>
		/// A Func to set the current destination.
		/// </summary>
		Action<Vector3> SetDestnFunc;

		/// <summary>
		/// Offset (distance from the target pos to be "in front" of him).
		/// </summary>
		Vector2 offset;

		/// <summary>
		/// Create a new AttackAAction.
		/// </summary>
		/// <param name="_charDriver">The driver of the character that owns the action. Use to calculate orientation (left or right).</param>
		/// <param name="_GetTargetFunc">A Func to retrieve the current target GameObject.</param>
		/// <param name="_SetCurrDestnIdx">A method to set the current destination index.</param>
		/// <param name="offset">The picked position will be "curr target pos" plus this offset.</param>
		public SDPickPointInFrontOfTargetAction(ICharacterDriver _charDriver, Func<GameObject> _GetTargetFunc, Action<Vector3> _SetDestnFunc, Vector2 _offset)
		{
			charDriver = _charDriver;
			GetTargetFunc = _GetTargetFunc;
			SetDestnFunc = _SetDestnFunc;

			_Action = PickPointInFrontOfTargetExec;
		}

		protected virtual BehaviorReturnCode PickPointInFrontOfTargetExec()
		{
			GameObject targetGO = GetTargetFunc();
			if (targetGO == null) {
				return BehaviorReturnCode.Failure;
			}

			Vector2 charPos = charDriver.GetPosition();
			Vector2 targetPos = targetGO.transform.position;

			// Add relative offset.
			Vector3 newDest = targetPos + (offset * ((charPos.x > targetPos.x)? 1 : -1));

			SetDestnFunc(newDest);

			return BehaviorReturnCode.Success;
		}


	}
} // namespace CSGameUtils
