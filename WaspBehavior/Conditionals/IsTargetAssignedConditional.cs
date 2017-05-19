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
using System;

namespace CSGameUtils
{
	/// <summary>
	/// Check if there a assigned target.
	/// </summary>
	public class IsTargetAssignedConditional : Conditional
	{
		/// <summary>
		/// A Func to retrieve the current target.
		/// </summary>
		Func<GameObject> GetTargetFunc;

		/// <summary>
		/// Create a new IsTargetAssignedConditional condition.
		/// </summary>
		/// <param name="_GetTargetFunc">A Func to retrieve the current target GameObject.</param>
		public IsTargetAssignedConditional(Func<GameObject> _GetTargetFunc)
		{
			GetTargetFunc = _GetTargetFunc;
			_Bool = IsTargetAssignedTest;
		}

		bool IsTargetAssignedTest()
		{
			GameObject target = GetTargetFunc();

			if ((target != null) && target.activeSelf) {
				return true;
			} else {
				return false;
			}
		}
	}
} // namespace CSGameUtils
