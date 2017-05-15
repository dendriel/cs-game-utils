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
	/// Execute the Attack A method from the given character.
	/// </summary>
	public class AttackAAction : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		protected ICharacterDriver charDriver;
    
		/// <summary>
		/// Create a new AttackAAction.
		/// </summary>
		/// <param name="_charDriver">The driver of the character that will perform the attack.</param>
		public AttackAAction(ICharacterDriver _charDriver)
		{
			charDriver = _charDriver;
			_Action = AttackAExec;
		}
    
		protected virtual BehaviorReturnCode AttackAExec()
		{
			charDriver.AttackA();
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils

