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

namespace CSGameUtils
{
	/// <summary>
	/// Conditional to check if the character is Hurt.
	/// </summary>
	public class IsHurtConditional : Conditional
	{
		/// <summary>
		/// Character driver (onwer of the condition).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// Create a new IsHurtConditional.
		/// </summary>
		/// <param name="_charDriver">The driver of the character to check if it is Hurt.</param>
		public IsHurtConditional(ICharacterDriver _charDriver)
		{
			charDriver = _charDriver;
			_Bool = IsHurtTest;
		}

		bool IsHurtTest()
		{
			return charDriver.IsBeingHurt();
		}
	}
} // namespace CSGameUtils