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
	/// Setup the next (or the first) destination from a route.
	/// </summary>
	public class SetupNextDestnAction<T> : BehaviorAction
	{
		/// <summary>
		/// A Function to retrieve a update list with the points to be visited.
		/// </summary>
		Func<T[]> GetDestnPoints;
    
		/// <summary>
		/// The current point index to be visited.
		/// </summary>
		Func<int> GetDestnIdx;

		/// <summary>
		/// The current destination.
		/// </summary>
		Action<T> SetDesntFunc;

		/// <summary>
		/// Create a new SetupNextDestnAction.
		/// </summary>
		/// <param name="_GetDestnPoints">A Function to retrieve a update list with the points to be visited.</param>
		/// <param name="startingPointToPatrolIdx">The first patrolling point.</param>
		/// /// <param name="_SetCurrDestnIdx">A method to set the current destination index.</param>
		/// <param name="_SetDesntFunc">A method to define the next destination to visit.</param>
		public SetupNextDestnAction(Func<T[]> _GetDestnPoints, Func<int> _GetDestnIdx, Action<T> _SetDesntFunc)
		{
			GetDestnPoints = _GetDestnPoints;
			GetDestnIdx = _GetDestnIdx;
			SetDesntFunc = _SetDesntFunc;

			_Action = SetupNextDestnExec;
		}
    
		BehaviorReturnCode SetupNextDestnExec()
		{
			int currDestnPointIdx = GetDestnIdx();
			T[] destnPoints = GetDestnPoints();

			// Set next destn point.
			SetDesntFunc(destnPoints[currDestnPointIdx]);

			//Debug.Log("Setup next destn: " + GetDestnPoints().Length + " - next: " + destnPoints[currDestnPointIdx]);
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils
