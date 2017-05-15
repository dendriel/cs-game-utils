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
	/// Wait some time.
	/// </summary>
	public class WaitSomeTimeAction : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// The cooldown that will handle the waiting.
		/// </summary>
		Cooldown cooldown;
    
		/// <summary>
		/// Create a new WaitSomeTimeAction.
		/// </summary>
		/// <param name="_charDriver">The character that will wait.</param>
		/// <param name="_cooldown">The cooldown resource that controls the waiting.</param>
		public WaitSomeTimeAction(ICharacterDriver _charDriver, Cooldown _cooldown)
		{
			charDriver = _charDriver;
			cooldown = _cooldown;

			_Action = WaitSomeTimeExec;
		}

		BehaviorReturnCode WaitSomeTimeExec()
		{
			charDriver.StopMoving();
			cooldown.Start(charDriver.GetMonoBehavior());

			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils