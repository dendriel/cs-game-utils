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
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Moves the character towards the given destination.
	/// </summary>
	public class MoveToDestnAction : BehaviorAction
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// A Func to retrieve the current destination.
		/// </summary>
		Func<Vector3> GetDestnFunc;

		/// <summary>
		/// If the character is supposed to run instead of walking.
		/// </summary>
		bool run;

		/// <summary>
		/// If the character is supposed to move up and down to reach its destination.
		/// </summary>
		bool axisMvmt;

		/// <summary>
		/// Create a new MoveToDestnAction.
		/// </summary>
		/// <param name="_charDriver">The driver of the character that will move.</param>
		/// <param name="_GetDestnFunc">A Func to retrieve the current destination.</param>
		/// <param name="_run">If the character is supposed to run (default is to walk).</param>
		/// <param name="_axisMvmt">If the character is supposed to move up and down to reach its destination.</param>
		public MoveToDestnAction(ICharacterDriver _charDriver, Func<Vector3> _GetDestnFunc, bool _run = false, bool _axisMvmt = false)
		{
			charDriver = _charDriver;
			GetDestnFunc = _GetDestnFunc;
			run = _run;
			axisMvmt = _axisMvmt;

			_Action = MoveToDestnExec;
		}

		BehaviorReturnCode MoveToDestnExec()
		{
			// We may use X/8 of the offset distance to be considered close to a position. This way, we may ensure
			// the character will stop moving only if it's inside the destination area.
			//
			// I left this action binded to IsArrivedAtDestnConditional because they are often used together.
			float offset = IsArrivedAtDestnConditional.Offset / 8;

			Vector3 charPos = charDriver.GetPosition();

			// Horizontal movement.
			float destnX = GetDestnFunc().x;
			float currPosX = charPos.x;
			if ((destnX - offset) > currPosX) {
				if (run) {
					charDriver.RunRight();
				} else {
					charDriver.WalkRight();
				}
			} else if ((destnX + offset) < currPosX){
				if (run) {
					charDriver.RunLeft();
				} else {
					charDriver.WalkLeft();
				}
			}

			// Vertical movement.
			if (axisMvmt) {
				float destnY = GetDestnFunc().y;
				float currPosY = charPos.y;
            
				if ((destnY - offset) > currPosY) {
					if (run) {
						charDriver.RunUp();
					} else {
						charDriver.WalkUp();
					}
				} else if ((destnY + offset) < currPosY){
					if (run) {
						charDriver.RunDown();
					} else {
						charDriver.WalkDown();
					}
				}
			}

			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils