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
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Send a message to a script.
	/// </summary>
	public class SendMessageAction : BehaviorAction
	{
		/// <summary>
		/// A reference for the GO that will receive the message.
		/// </summary>
		protected GameObject targetGoRef;

		/// <summary>
		/// The message to be sent.
		/// </summary>
		protected string message;

		/// <summary>
		/// The message parameter, if any.
		/// </summary>
		protected object param;

		/// <summary>
		/// Create a new SendMessageAction.
		/// </summary>
		/// <param name="_targetGoRef">A reference for the GO that will receive the message.</param>
		/// <param name="_message">The message to be sent.</param>
		/// <param name="_param">The message parameter, if any.</param>
		public SendMessageAction(GameObject _targetGoRef, string _message, object _param = null)
		{
			targetGoRef = _targetGoRef;
			message = _message;
			param = _param;

			_Action = SendMessageExec;
		}
    
		protected virtual BehaviorReturnCode SendMessageExec()
		{
			targetGoRef.SendMessage(message, param);
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils

