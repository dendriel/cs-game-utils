/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Character Driver.
 *
 *	Character Driver is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Character Driver is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Character Driver. If not, see<http://www.gnu.org/licenses/>.
 */

namespace CSGameUtils
{
	/// <summary>
	/// This an interface for character drivers. It's useful when using Wasp Behavior engine. Many actions of
	/// this engine receives a CharacterDriver as parameter. So, in order to allow us to use the behavior engine
	/// in different projects (with differenct driver functionalities), this interface must be implemented. =]
	/// </summary>
	public interface ICharacterDriver
	{
		/// <summary>
		/// Move the character.
		/// </summary>
		void WalkRight();

		/// <summary>
		/// Move the character. Will automatically flip to the walking direction.
		/// </summary>
		void WalkLeft();

		/// <summary>
		/// Makes the character walk up.
		/// </summary>
		void WalkUp();

		/// <summary>
		/// Makes the character walk down.
		/// </summary>
		void WalkDown();

		/// <summary>
		/// Makes the character run to the right.
		/// </summary>
		void RunRight();

		/// <summary>
		/// Makes the character run to the left.
		/// </summary>
		void RunLeft();

		/// <summary>
		/// Makes the character run up.
		/// </summary>
		void RunUp();

		/// <summary>
		/// Makes the character run down.
		/// </summary>
		void RunDown();

		/// <summary>
		/// Makes the character jump.
		/// </summary>
		void Jump();

		/// <summary>
		/// Performs the Attack A.
		/// </summary>
		void AttackA();

		/// <summary>
		/// Performs the Attack B.
		/// </summary>
		void AttackB();
	}
} // namespace CSGameUtils
