/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	Animation Utilities is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Animation Utilities is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Animation Utilities. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Utilities for setting up animator and animations.
	/// 
	/// The main feature of this class is "SetupAnimationEvents" that enable us to setup animations from code.
	/// </summary>
	public static class AnimUtils 
	{
		/// <summary>
		/// Aniamtion event callback data bundle.
		/// </summary>
		public struct CallbackData
		{
			/// <summary>
			/// Callback to be triggered.
			/// </summary>
			public string CallbackName { get; private set; }

			/// <summary>
			/// Animation time to trigger the callback.
			/// </summary>
			public float TimeTotriggerInSec { get; private set; }
		
			/// <summary>
			/// Float parameter.
			/// </summary>
			public float FloatParamValue { get; private set; }
			// Flag to use this variable as callback parameter.
			public bool SetFloat { get; private set; }

			/// <summary>
			/// Int parameter.
			/// </summary>
			public int IntParamValue { get; private set; }
			// Flag to use this variable as callback parameter.
			public bool SetInt { get; private set; }

			/// <summary>
			/// String parameter.
			/// </summary>
			public string StringParamValue { get; private set; }
			// Flag to use this variable as callback parameter.
			public bool SetString { get; private set; }

			/// <summary>
			/// Create a new animation event callback data bundle.
			/// </summary>
			/// <param name="_callbackName">Callback to be triggered.</param>
			/// <param name="_timeTotriggerInSec">Animation time to trigger the callback.</param>
			public CallbackData(string _callbackName, float _timeTotriggerInSec)
			{
				TimeTotriggerInSec = _timeTotriggerInSec;
				CallbackName = _callbackName;
				FloatParamValue = -1;
				SetFloat = false;
				IntParamValue = -1;
				SetInt = false;
				StringParamValue = "";
				SetString = false;
			}

			/// <summary>
			/// Create a new animation event callback data bundle. The callback receives a float parameter.
			/// </summary>
			/// <param name="_callbackName">Callback to be triggered.</param>
			/// <param name="_timeTotriggerInSec">Animation time to trigger the callback.</param>
			/// <param name="_floatParamValue">Float parameter to be used in the callback.</param>
			public CallbackData(string _callbackName, float _timeTotriggerInSec, float _floatParamValue)
			{
				TimeTotriggerInSec = _timeTotriggerInSec;
				CallbackName = _callbackName;
				FloatParamValue = _floatParamValue;
				SetFloat = true;
				IntParamValue = -1;
				SetInt = false;
				StringParamValue = "";
				SetString = false;
			}

			/// <summary>
			/// Create a new animation event callback data bundle. The callback receives a float parameter.
			/// </summary>
			/// <param name="_callbackName">Callback to be triggered.</param>
			/// <param name="_timeTotriggerInSec">Animation time to trigger the callback.</param>
			/// <param name="_intParamValue">Int parameter to be used in the callback.</param>
			public CallbackData(string _callbackName, float _timeTotriggerInSec, int _intParamValue)
			{
				TimeTotriggerInSec = _timeTotriggerInSec;
				CallbackName = _callbackName;
				FloatParamValue = -1;
				SetFloat = false;
				IntParamValue = _intParamValue;
				SetInt = true;
				StringParamValue = "";
				SetString = false;
			}

			/// <summary>
			/// Create a new animation event callback data bundle. The callback receives a float parameter.
			/// </summary>
			/// <param name="_callbackName">Callback to be triggered.</param>
			/// <param name="_timeTotriggerInSec">Animation time to trigger the callback.</param>
			/// <param name="_stringParamValue">String parameter to be used in the callback.</param>
			public CallbackData(string _callbackName, float _timeTotriggerInSec, string _stringParamValue)
			{
				TimeTotriggerInSec = _timeTotriggerInSec;
				CallbackName = _callbackName;
				FloatParamValue = -1;
				SetFloat = false;
				IntParamValue = -1;
				SetInt = false;
				StringParamValue = _stringParamValue;
				SetString = true;
			}
		}

		/// <summary>
		/// Animation events data bundle. Setup animation events.
		/// </summary>
		public struct AnimEventsData
		{
			/// <summary>
			/// Index of the animation to be configured.
			/// </summary>
			public int ClipIndex { get; private set; }

			/// <summary>
			/// Events to be set.
			/// </summary>
			public CallbackData[] eventsList { get; private set; }

			public AnimEventsData(int _clipIndex, params CallbackData[] _eventsList)
			{
				ClipIndex = _clipIndex;
				eventsList = _eventsList;
			}
		}

		/// <summary>
		/// Check if an animation event is already set.
		/// </summary>
		/// <param name="clip">The animation to be checked.</param>
		/// <param name="eventName">The event name.</param>
		/// <param name="time">(optional) Check a specific time position.</param>
		/// <returns></returns>
		public static bool IsEventAlreadySet(AnimationClip clip, string eventName, float time=-1)
		{
			// Check each event in clip.events.
			foreach (AnimationEvent evt in clip.events) {
				// Check event name.
				if (evt.functionName != eventName) continue;
				// Time must be checked?
				if (time >= 0) {
					// Check event time.
					if (evt.time == time) {
						return true;
					}
				} else {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Setup an animation with animation events.
		/// 
		/// Motivation: I created this class because I was working in a project in which the art designer send me Unity
		/// packages with character animations in it. Every time that I loaded these packages in my project, all the
		/// previously animation events set by me (by hand) were lost. So, instead of bothering the animator (and myself),
		/// this feature allows me to set animation events from code.
		/// </summary>
		/// <param name="anim">The animator that holds the animations.</param>
		/// <param name="animData">Data from the animation to be set.</param>
		public static void SetupAnimationEvents(Animator anim, AnimEventsData animData)
		{

			AnimationClip clip = anim.runtimeAnimatorController.animationClips[animData.ClipIndex];
			

			for (int i = 0; i < animData.eventsList.Length; i++) {
				CallbackData currCb = animData.eventsList[i];

				// Avoid setting the same event twice.
				if (IsEventAlreadySet(clip, currCb.CallbackName, currCb.TimeTotriggerInSec)) continue;

				// Setup current callback.
				AnimationEvent evt = new AnimationEvent();
				evt.time = currCb.TimeTotriggerInSec;
				evt.functionName = currCb.CallbackName;

				// Setup paramters.
				if (currCb.SetFloat) {
					evt.floatParameter = currCb.FloatParamValue;
				} else if (currCb.SetInt) {
					evt.intParameter = currCb.IntParamValue;
				} else if (currCb.SetString) {
					evt.stringParameter = currCb.StringParamValue;
				}

				// Add the new animation event.
				clip.AddEvent(evt);
			}
		}
	
		public static void SetupAnimationEvents(Animator anim, params AnimEventsData[] animData)
		{
			for (int i = 0; i < animData.Length; i++) {
				SetupAnimationEvents(anim, animData[i]);
			}
		}

		/// <summary>
		/// Enable a layer while disabling all other layers.
		/// </summary>
		public static void SetupAnimatorLayerWeight(Animator animator, int layerIdx)
		{
			for (int i = 0; i < animator.layerCount; i++) {
				animator.SetLayerWeight(i, 0);
			}

			animator.SetLayerWeight(layerIdx, 1);
		}
	}
} // namespace CSGameUtils
