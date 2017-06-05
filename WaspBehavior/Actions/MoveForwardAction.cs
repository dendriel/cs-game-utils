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

namespace CSGameUtils
{
	/// <summary>
	/// Moves the character to its facing direction.
	/// </summary>
	public class MoveForwardAction : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		protected ICharacterDriver charDriver;

		/// <summary>
		/// Character movement type.
		/// </summary>
		bool run;

		/// <summary>
		/// Create a new MoveForwardAction.
		/// </summary>
		/// <param name="_charDriver">The driver of the character that will perform the attack.</param>
		/// <param name="_run">The character movement is to run?</param>
		public MoveForwardAction(ICharacterDriver _charDriver, bool _run = false)
		{
			charDriver = _charDriver;
			run = _run;

			_Action = MoveForwardExec;
		}
    
		protected virtual BehaviorReturnCode MoveForwardExec()
		{
			if (charDriver.IsFacingRight()) {
				if (run) {
					charDriver.RunRight();
				} else {
					charDriver.WalkRight();
				}
			} else {
				if (run) {
					charDriver.RunLeft();
				} else {
					charDriver.WalkLeft();
				}
			}

			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils

