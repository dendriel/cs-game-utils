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

/**
 * WARNING: Wasp Behavior uses a Behavior Library implementation by Thomas H. Jonell. I could not find the 
 * original git repository for its implementation, but there is a fork in
 * here: https://github.com/listentorick/UnityBehaviorLibrary
 * 
 * *The attached version of Behavior Library was modified by me at some points.
 */
namespace CSGameUtils
{
	/// <summary>
	/// A behavior event is an action that returns Success only when its effect has been finished.
	/// 
	/// For instance, an image alpha fading finalizes when all the image alpha is zero, so the action
	/// will return success. Before this, the action returns running.
	/// 
	/// (useful for statefull composites in animations; in other words, mostly useful for animations or
	/// in-game conditionals).
	/// </summary>
	public class WaspBehaviorEvent : BehaviorAction
	{
		/// <summary>
		/// Flag that determines if the event was already triggered.
		/// </summary>
		bool isTriggered = false;

		/// <summary>
		/// Flag that determines if the event is concluded.
		/// </summary>
		protected bool isConcluded = false;

		/// <summary>
		/// Always trigger the Exec() funcionality. Check isConcluded flag after.
		/// </summary>
		protected bool alwaysExec = false;

		public WaspBehaviorEvent()
		{
			_Action = Trigger;
		}

		protected BehaviorReturnCode Trigger()
		{
			if (!isTriggered || alwaysExec) {
				isTriggered = true;
				Exec();

			}

			if (CheckConcluded()) {
				return BehaviorReturnCode.Success;
			}

			return BehaviorReturnCode.Running;
		}

		protected virtual void Exec()
		{
			throw new Exception("Exec() must be implemented!");
		}

		/// <summary>
		/// Conclude this action. May be used as callback for actions that generate events when finished.
		/// </summary>
		/// <param name="value">Unused.</param>
		public virtual void Conclude(float value)
		{
			isConcluded = true;
		}

		/// <summary>
		/// Checks if the action outcome has been achieved. May be override to test conclusion conditions.
		/// </summary>
		/// <returns>true if concluded; false otherwise</returns>
		protected virtual bool CheckConcluded()
		{
			return isConcluded;
		}
	}
} // namespace CSGameUtils
