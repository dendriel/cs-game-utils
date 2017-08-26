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
	/// Set a bool to true/false.
	/// Used in combination to IsBoolConditional.
	/// </summary>
	public class SetBoolAction : BehaviorAction
	{
		/// <summary>
		/// Action to set a bool.
		/// </summary>
		Action<bool> SetBool;

		/// <summary>
		/// Value to be set in the bool.
		/// </summary>
		bool value;

		/// <summary>
		/// Create a new SetBoolAction.
		/// </summary>
		/// <param name="setBool">Action to set the bool.</param>
		/// <param name="value">Value to be set.</param>
		public SetBoolAction(Action<bool> _SetBool, bool _value = true)
		{
			SetBool = _SetBool;
			value = _value;
			_Action = SetBoolExec;
		}
    
		protected virtual BehaviorReturnCode SetBoolExec()
		{
			SetBool(value);
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils

