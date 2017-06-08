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
using System;
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Send a message to a script.
	/// </summary>
	public class SendMessageGetParamAction<T> : SendMessageAction
	{
		/// <summary>
		/// A Func to retrieve the parameter to be sent.
		/// </summary>
		Func<T> GetParam;

		/// <summary>
		/// Create a new SendMessageGetParamAction.
		/// </summary>
		/// <param name="_targetGoRef">A reference for the GO that will receive the message.</param>
		/// <param name="_GetParam">Retrieve the game object to be sent.</param>
		public SendMessageGetParamAction(GameObject _targetGoRef, string _message, Func<T> _GetParam) :
			base(_targetGoRef, _message)
		{
			// The parameter will be set right before sending the message.

			GetParam = _GetParam;			
		}
    
		protected override BehaviorReturnCode SendMessageExec()
		{
			// Setup param before sending the message.
			param = GetParam();

			return base.SendMessageExec();
		}
	}
} // namespace CSGameUtils

