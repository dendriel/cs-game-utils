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
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary;

namespace CSGameUtils
{
	/// <summary>
	/// Request a character to stop its movement.
	/// </summary>
	public class StopMovementAction : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// Clear the character velocity.
		/// </summary>
		bool zeroVelocity;

		/// <summary>
		/// Create a new StopMovementAction.
		/// </summary>
		/// <param name="_charDriver">The driver of the character that will perform the attack.</param>
		/// <param name="_zeroVelocity">Also request to remove any X and Y velocity from character.</param>
		public StopMovementAction(ICharacterDriver _charDriver, bool _zeroVelocity = false)
		{
			charDriver = _charDriver;
			zeroVelocity = _zeroVelocity;

			_Action = StopMovementExec;
		}

		BehaviorReturnCode StopMovementExec()
		{
			charDriver.StopMoving(zeroVelocity);
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils
