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

namespace CSGameUtils
{
	/// <summary>
	/// Set a index to given value.
	/// </summary>
	public class ResetIndexAction : BehaviorAction
	{
		/// <summary>
		/// An Action to allow to update the index.
		/// </summary>
		Action<int> SetIdx;

		/// <summary>
		/// The value to be set.
		/// </summary>
		int newIdxValue;

		/// <summary>
		/// Create a new ResetIndexAction.
		/// </summary>
		/// <param name="_SetIdx">An Action to update the index.</param>
		/// <param name="_newIdxValue">The new value of the index.</param>
		public ResetIndexAction(Action<int> _SetIdx, int _newIdxValue = 0)
		{
			SetIdx = _SetIdx;
			newIdxValue = _newIdxValue;

			_Action = ResetIndexExec;
		}
    
		BehaviorReturnCode ResetIndexExec()
		{
			SetIdx(newIdxValue);

			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils
