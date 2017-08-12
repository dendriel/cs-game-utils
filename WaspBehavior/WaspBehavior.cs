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
using UnityEngine;
using BehaviorLibrary;
using System;

/**
 * WARNING: Wasp Behavior uses a Behavior Library implementation from by H. Jonell. I could not find the 
 * original git repository for its implementation, but there is a fork in
 * here: https://github.com/listentorick/UnityBehaviorLibrary
 * 
 * *The attached version of Behavior Library was modified by me at some points.
 */
namespace CSGameUtils
{
	/// <summary>
	/// Wasp behavior aims to easy the development of game agents (or AIs). Basically, you must extend this class
	/// and implement the "BuildBehavior" method. In there, you will setup you behavior tree and set it inside the
	/// "behavior" parameter "behavior = new Behavior(BehaviorComponent).
	/// 
	/// Check the "Behaviors" folder for examples.
	/// </summary>
	public abstract class WaspBehavior: MonoBehaviour
	{
		protected Behavior behavior;

		/// <summary>
		/// This behavior may be enabled by "send message" callback/command?
		/// Useful when we have more than one behavior in a GO. Leave true for the first behavior
		/// that must be executed.
		/// </summary>
		public bool EnabledByCmd = true;

		protected virtual void OnEnable()
		{
			// Sanitize every time the behavior is enabled (otherwise may contain some garbage from previous execution).
			Sanitize();
		}

		protected virtual void Start()
		{
			// Sanitize when starting. Because in the first execution OnEnable() will sanitize with empty parameter values.
			Sanitize();
		}

		protected virtual void Update()
		{
			Behave();
		}

		public virtual void Sanitize()
		{
			BuildBehavior();
		}

		public BehaviorReturnCode Behave()
		{
			return behavior.Behave();
		}

		protected abstract void BuildBehavior();

		/// <summary>
		/// Callback to be called from a SendMessage() event. Command: "SetEnabledCmd"
		/// 
		/// Will work only if EnabeldBtCmd is set as true.
		/// </summary>
		/// <param name="enabled">"true" - enable; "false" - disable.</param>
		public virtual void SetEnabledCmd(string enabled)
		{
			if (EnabledByCmd) {
				this.enabled = Convert.ToBoolean(enabled);
			}
		}
	}
} // namespace CSGameUtils
