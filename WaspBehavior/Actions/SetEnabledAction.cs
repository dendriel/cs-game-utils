﻿using BehaviorLibrary;
using BehaviorLibrary.Components.Actions;
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Activate/Deactivate a component.
	/// </summary>
	public class SetEnabledAction : BehaviorAction
	{
		/// <summary>
		/// The component to be set.
		/// </summary>
		MonoBehaviour component;

		/// <summary>
		/// The new state of the component.
		/// </summary>
		bool active;

		/// <summary>
		/// Create a new SetEnabledAction.
		/// </summary>
		/// <param name="_component">The component to be set.</param>
		/// <param name="_active">The new state of the component.</param>
		public SetEnabledAction(MonoBehaviour _component, bool _active = true)
		{
			Assert.IsNotNull(_component, "Behavior to be enabled can't be null!");
			component = _component;
			active = _active;

			_Action = SetEnabledExec;
		}

		BehaviorReturnCode SetEnabledExec()
		{
			component.enabled = active;
			return BehaviorReturnCode.Success;
		}
	}
} // namespace CSGameUtils