/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Action Timer.
 *
 *	Action Timer is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Action Timer is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Action Timer. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;
using System;
using System.Collections;


namespace CSGameUtils
{
	/// <summary>
	/// Handles ActionTimer data. This is a timer that triggers a given callback after some time.
	/// </summary>
	[Serializable]
	public class ActionTimer<T>
	{
		/// <summary>
		/// The minimum time of timer.
		/// </summary>
		[SerializeField]
		float minimum = 1f;
		public float Minimum { get { return minimum; } }

		/// <summary>
		/// The maximum time of timer.
		/// </summary>
		[SerializeField]
		float maximum = 2f;
		public float Maximum { get { return maximum; } }

		/// <summary>
		/// Tells if timer is in progress.
		/// </summary>
		bool isWaiting;
		public bool IsWaiting { get { return isWaiting; } }

		/// <summary>
		/// Tells if the timer has ended.
		/// </summary>
		public bool HasEnded { get; private set; }

		/// <summary>
		/// The coroutine that runs the cooldown.
		/// </summary>
		Coroutine cooldownCoroutine;

		/// <summary>
		/// Callback to be called when the timer goes out.
		/// </summary>
		Action actionDoneCallback;

		/// <summary>
		/// Callback to be called when the timer goes out. Accepts one parameter.
		/// </summary>
		Action<T> actionDoneWithParamCallback;

		/// <summary>
		/// The data to be passed to the callback.
		/// </summary>
		T DataToReturn { get; set; }

		/// <summary>
		/// Wait for unscaled time.
		/// </summary>
		bool useRealTime;

		/// <summary>
		/// Initializes a new instance of the ActionTimer class.
		/// </summary>
		/// <param name="min">Minimum time to wait.</param>
		/// <param name="max">Maximum time to wait.</param>
		/// <param name="actionCb">Callback to be called when the timer finishes.</param>
		/// <param name="data">Data to be passed to the actionCb.</param>
		/// <param name="useRealTime">Use unscaled time.</param>
		public ActionTimer(float min, float max, Action<T> actionCb, T data, bool useRealTime = false)
		{
			actionDoneWithParamCallback = actionCb;
			DataToReturn = data;

			DefaultConstructor(min, max, useRealTime);
		}

		/// <summary>
		/// Initializes a new instance of the ActionTimer class.
		/// </summary>
		/// <param name="timeToWait">Time to wait before triggering the callback.</param>
		/// <param name="actionCb">Callback to be called when the timer finishes.</param>
		/// <param name="data">Data to be passed to the actionCb.</param>
		/// <param name="useRealTime">Use unscaled time.</param>
		public ActionTimer(float timeToWait, Action<T> actionCb, T data, bool useRealTime = false)
		{
			actionDoneWithParamCallback = actionCb;
			DataToReturn = data;

			DefaultConstructor(timeToWait, timeToWait, useRealTime);
		}

		/// <summary>
		/// Initializes a new instance of the ActionTimer class.
		/// </summary>
		/// <param name="min">Minimum time to wait.</param>
		/// <param name="max">Maximum time to wait.</param>
		/// <param name="actionCb">Callback to be called when the timer finishes (whithout parameter).</param>
		/// <param name="useRealTime">Use unscaled time.</param>
		public ActionTimer(float min, float max, Action actionCb, bool useRealTime = false)
		{
			actionDoneCallback = actionCb;
			DefaultConstructor(min, max, useRealTime);
		}

		/// <summary>
		/// Initializes a new instance of the ActionTimer class.
		/// </summary>
		/// <param name="max">Time to wait.</param>
		/// <param name="actionCb">Callback to be called when the timer finishes (whithout parameter).</param>
		/// <param name="useRealTime">Use unscaled time.</param>
		public ActionTimer(float timeToWait, Action actionCb, bool useRealTime = false)
		{
			actionDoneCallback = actionCb;
			DefaultConstructor(timeToWait, timeToWait, useRealTime);
		}

		/// <summary>
		/// Default constructor (for what is shared between the other constructors).
		/// </summary>
		/// <param name="min">Minimum time to wait.</param>
		/// <param name="max">Maximum time to wait.</param>
		/// <param name="_useRealTime">Use unscaled time.</param>
		void DefaultConstructor(float min, float max, bool _useRealTime = false)
		{
			minimum = min;
			maximum = max;
			isWaiting = false;
			HasEnded = false;
			useRealTime = _useRealTime;

		}

		/// <summary>
		/// Calculates the cooldown value.
		/// </summary>
		/// <returns>The cooldown value.</returns>
		float TimeToWait ()
		{
			if (minimum == maximum) {
				return maximum;
			} else {
				return UnityEngine.Random.Range(minimum, maximum);
			}
		}

		/// <summary>
		/// Update minimum and maximum waiting time.
		/// </summary>
		/// <param name="min">Minimum time to wait.</param>
		/// <param name="max">Maximum time to wait.</param>
		public void Set(float min, float max)
		{
			minimum = min;
			maximum = max;
		}

		/// <summary>
		/// Starts the cooldown.
		/// </summary>
		/// <param name="behavior">Behavior.</param>
		public void Start (MonoBehaviour behavior)
		{
			if (cooldownCoroutine != null) behavior.StopCoroutine(cooldownCoroutine);
			cooldownCoroutine = behavior.StartCoroutine(WaitCooldown());
			HasEnded = false;
		}

		/// <summary>
		/// Interrupts the cooldown.
		/// </summary>
		public void Stop (MonoBehaviour behavior)
		{
			if (cooldownCoroutine != null) behavior.StopCoroutine(cooldownCoroutine);
			cooldownCoroutine = null;
			isWaiting = false;
			HasEnded = false;
		}

		/// <summary>
		/// Waits the cooldown.
		/// </summary>
		/// <returns>The cooldown.</returns>
		IEnumerator WaitCooldown ()
		{
			isWaiting = true;
			
			if (useRealTime) {
				yield return new WaitForSecondsRealtime(TimeToWait());
			} else {
				yield return new WaitForSeconds(TimeToWait());
			}

			isWaiting = false;
			HasEnded = true;
			if (actionDoneCallback != null) {
				actionDoneCallback();
			} else {
				actionDoneWithParamCallback(DataToReturn);
			}
		}
	}

	/// <summary>
	/// ActionTimer wrapper so we don't need to setup a Type if we won't be using it.
	/// </summary>
	public class ActionTimer : ActionTimer<float>
	{
		/// <summary>
		/// Initializes a new instance of the ActionTimer class.
		/// </summary>
		/// <param name="min">Minimum time to wait.</param>
		/// <param name="max">Maximum time to wait.</param>
		/// <param name="actionCb">Callback to be called when the timer finishes (whithout parameter).</param>
		/// <param name="useRealTime">Use unscaled time.</param>
		public ActionTimer(float timeToWait, Action actionCb, bool useRealTime = false) : base(timeToWait, actionCb, useRealTime)
		{ }
	}
} // namespace CSGameUtils