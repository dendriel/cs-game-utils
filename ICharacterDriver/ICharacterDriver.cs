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

using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Points that define the character bounds. Useful for raycasting.
	/// </summary>
	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
		public Vector2 leftCenter, rightCenter;
		public Vector2 bottomCenter, topCenter;
	}

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
		/// Most useful to force a transition to idle. (walk = false).
		/// </summary>
		/// <param name="zeroVelocity">Set velocity to zero.</param>
		void StopMoving(bool zeroVelocity = false);

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

		/// <summary>
		/// Flip the character (change its facing direction).
		/// </summary>
		void Flip();

		/// <summary>
		/// The character is facing to the right?
		/// </summary>
		bool IsFacingRight();

		/// <summary>
		/// Take a hit.
		/// </summary>
		/// <param name="hitProperty">Properties of the hit.</param>
		/// <returns>true if the character acknoledged the (and was) hit. false otherwise.</returns>
		bool TakeHit<T>(T hitProperty);

		/// <summary>
		/// Get the characters position.
		/// </summary>
		/// <returns>transform.position</returns>
		Vector3 GetPosition();

		/// <summary>
		/// Get a MonoBehavior from the character. Useful for starting coroutines in AI behaviors.
		/// </summary>
		/// <returns>A MonoBehavior reference.</returns>
		MonoBehaviour GetMonoBehavior();

		/// <summary>
		/// Get the character's raycast origins.
		/// </summary>
		/// <returns>The updated raycast origins from the character.</returns>
		RaycastOrigins GetRaycastOrigins();

		/// <summary>
		/// Check if the character is in the idle state. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		bool IsIdle();

		/// <summary>
		/// Check if the character is in the walking state. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		bool IsWalking();

		/// <summary>
		/// Check if the character is in the running state. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		bool IsRunning();

		/// <summary>
		/// Check if the character is in the attacking state. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		bool IsAttacking();

		/// <summary>
		/// Check if the character is in the jumping state. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		bool IsJumping();

		/// <summary>
		/// Check if the character is falling. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if he is falling; false otherwise.</returns>
		bool IsFalling();

		/// <summary>
		/// Check if the character is in the landing state. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if it is landing; false otherwise.</returns>
		bool IsLanding();

		/// <summary>
		/// Check if the character is being hurt. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if he is being hurt; false otherwise.</returns>
		bool IsBeingHurt();

		/// <summary>
		/// Check if the character is dead. Useful for AI behaviors.
		/// </summary>
		/// <returns>true if the character is dead; false otherwise.</returns>
		bool IsDead();
	}
} // namespace CSGameUtils
