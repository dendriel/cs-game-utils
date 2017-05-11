/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Spriter Animator.
 *
 *	Spriter Animator is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Spriter Animator is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Spriter Animator. If not, see<http://www.gnu.org/licenses/>.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * WARNING:  Download SpriterDotNet resource to use with GenericAnimator.
 */
//using SpriterDotNetUnity;

namespace CSGameUtils
{
	/// <summary>
	/// This class is used to encapsulate the animator functionality. The class may handle
	/// Unity Animator or Spriter Animator. This allows the character driver to use one or
	/// another.
	/// 
	/// We could had created an interface to create a IUnityAnimator and ISpriterAnimator and
	/// create a generic variable here, but i believe that will be harder to use.
	/// </summary>
	public class SpriterAnimator
	{
		/// <summary>
		/// Unity animator.
		/// </summary>
		Animator unityAnimator;

		/// <summary>
		/// Spriter animator (despiste the name..)
		/// </summary>
		UnityAnimator spriterAnimator;

		/// <summary>
		/// Using the unity animator.
		/// </summary>
		bool useUnityAnim;

		/// <summary>
		/// Tells if the animator was setup and is ready to be used.
		/// </summary>
		public bool IsReady { get; private set; }
    
		/// <summary>
		/// Starts the Spriter Animator using the unity animator.
		/// </summary>
		/// <param name="animator">The animator to be handled.</param>
		public SpriterAnimator(Animator animator)
		{
			unityAnimator = animator;
			useUnityAnim = true;
			IsReady = true;
		}

		/// <summary>
		/// Starts the Spriter Animator using the spriter animator. Set the animator and layers later.
		/// DON'T forget to call Update() every frame to handle automatic transitions between states.
		/// </summary>
		/// <param name="animator">The animator to be handled.</param>
		/// <param name="layerAmount">The number of layers in this animator. (Default is 1. The layer number "0")</param>
		public SpriterAnimator(UnityAnimator animator, int layerAmount = 1)
		{
			spriterAnimator = animator;
			spriterLayerAmount = layerAmount;
			useUnityAnim = false;
			IsReady = false;
		}

		/// <summary>
		/// Starts the Spriter Animator using the spriter animator.
		/// DON'T forget to call Update() every frame to handle automatic transitions between states.
		/// </summary>
		public SpriterAnimator()
		{
			useUnityAnim = false;
			IsReady = false;
		}

		/// <summary>
		/// Setup the spriter animator. Set the animator and layers later.
		/// </summary>
		/// <param name="animator">The animator to be handled.</param>
		/// <param name="layerAmount">The number of layers in this animator. (Default is 1. The layer number "0")</param>
		public void SetupSpriterAnimator(UnityAnimator animator, int layerAmount = 1)
		{
			spriterAnimator = animator;
			spriterLayerAmount = layerAmount;
		}

		/// <summary>
		/// Set a bool in the animator.
		/// </summary>
		/// <param name="param">The param to set.</param>
		/// <param name="value">The value to set.</param>
		public void SetBool(string param, bool value)
		{
			if (useUnityAnim) {
				unityAnimator.SetBool(param, value);
			} else {
				SpriterAnimatorSetBool(param, value);
			}
		}

		/// <summary>
		/// Set a trigger in the animator.
		/// </summary>
		/// <param name="param">The trigger to set.</param>
		public void SetTrigger(string param)
		{
			if (useUnityAnim) {
				unityAnimator.SetTrigger(param);
			} else {
				SpriterAnimatorSetTrigger(param);
			}
		}

		/// <summary>
		/// Set the weight of a layer.
		/// </summary>
		/// <param name="layer">The layer index.</param>
		/// <param name="weight">The layer weight.</param>
		/// <param name="customEntryState">Override the entry state of the new layer. Works for Spriter Animator only.</param>
		public void SetLayerWeight(int layer, int weight, string customEntryState = "")
		{
			if (useUnityAnim) {
				unityAnimator.SetLayerWeight(layer, weight);
			} else {
				SpriterAnimatorSetLayerWeight(layer, weight, customEntryState);
			}
		}

		/// <summary>
		/// Check if the current animation is called "name" for animator layer 0.
		/// </summary>
		/// <param name="statesList">The name of the animation. May be a list of states.</param>
		/// <returns>true if the running animation is one of the names in "statesList"; false otherwise.</returns>
		public bool IsCurrentAnimationName(params string[] statesList)
		{
			return IsCurrentAnimationName(0, statesList);
		}

		/// <summary>
		/// Check if the current animation is called "name" for the given animator layer.
		/// </summary>
		/// <param name="statesList">The name of the animation. May be a list of states.</param>
		/// <param name="layer">The layer of the animator to check.</param>
		/// <returns>true if the running animation is one of the names in "statesList"; false otherwise.</returns>
		public bool IsCurrentAnimationName(int layer, params string[] statesList)
		{
			for (int i = 0; i < statesList.Length; i++) {
				if (useUnityAnim) {
					if (unityAnimator.GetCurrentAnimatorStateInfo(layer).IsName(statesList[i])) return true;
				} else {
					if (spriterAnimator.CurrentAnimation.Name.Equals(statesList[i])) return true;
				}
			}

			return false;
		}

	//************************************************************************************************/
		/**
		 * Spriter Animator setup methods and values.
		 */

		/// <summary>
		/// Hold all values and transitions related to a state.
		/// </summary>
		public class AnimState
		{
			/// <summary>
			/// Holds data for a bool parameter.
			/// </summary>
			public struct BoolParam
			{
				/// <summary>
				/// The parameter name.
				/// </summary>
				public string Name;

				/// <summary>
				/// The parameter value.
				/// </summary>
				public bool Value;

				/// <summary>
				/// The next state to go.
				/// </summary>
				public string NextState;
			}

			/// <summary>
			/// Holds data for a Trigger parameter.
			/// </summary>
			public struct TriggerParam
			{
				/// <summary>
				/// The parameter name.
				/// </summary>
				public string Name;

				/// <summary>
				/// The next state to go.
				/// </summary>
				public string NextState;
			}

			/// <summary>
			/// Bool parameters List.
			/// </summary>
			public List<BoolParam> BoolParamsList;

			/// <summary>
			/// Trigger parameters List.
			/// </summary>
			public List<TriggerParam> TriggerParamsList;

			/// <summary>
			/// This is the entry state?
			/// </summary>
			public bool IsEntry { get; set; }

			/// <summary>
			/// The name of the state.
			/// </summary>
			public string Name { get; private set; }

			/// <summary>
			/// The name of the animation related to this state. Some states may be used in multiple layers but using
			/// different animations.
			/// </summary>
			public string AnimName { get; private set; }

			/// <summary>
			/// Atuomatically switch to next state.
			/// </summary>
			public bool AutoTransition { get; private set; }

			/// <summary>
			/// The next state from the auto-transition (if set).
			/// </summary>
			public string NextState { get; private set; }

			/// <summary>
			/// The speed of the animation. Default is 1.
			/// </summary>
			public float PlaybackSpeed { get; private set; }

			/// <summary>
			/// Create a new animation state.
			/// </summary>
			/// <param name="name">The name of the state and its animation.</param>
			/// <param name="isEntry">Is the entry state?</param>
			public AnimState(string name, string animName, bool isEntry = false)
			{
				Name = name;
				AnimName = animName;
				IsEntry = isEntry;

				PlaybackSpeed = 1;

				BoolParamsList = new List<BoolParam>();
				TriggerParamsList = new List<TriggerParam>();
			}

			/// <summary>
			/// Automatically switch to next state at the end of animation.
			/// </summary>
			public void SetAutomaticTransition(string nextState)
			{
				NextState = nextState;
				AutoTransition = true;
			}

			/// <summary>
			/// Set the animation's playback speed.
			/// </summary>
			/// <param name="speed">The speed to be used. Default is 1.</param>
			public void SetPlaybackSpeed(float speed)
			{
				PlaybackSpeed = speed;
			}
		}

		/// <summary>
		/// The number of layers the animator can handle.
		/// </summary>
		int spriterLayerAmount;

		/// <summary>
		/// Handle the "layers" and animations of the animator. Will be initialized when filling in the animations.
		/// </summary>
		List<AnimState>[] animStateList;

		/// <summary>
		/// Default layer index (or base layer index).
		/// </summary>
		const int defaultLayerIdx = 0;

		/// <summary>
		/// Defines the current layer being used. May be changed through SetLayerWeight.
		/// </summary>
		int currLayerIdx = defaultLayerIdx;

		/// <summary>
		/// Any state label.
		/// </summary>
		public const string AnyStateLabel = "AnyState";

		/// <summary>
		/// Access the any state data.
		/// </summary>
		AnimState anyState { get { return animStateList[currLayerIdx].Find(x => x.Name == AnyStateLabel); } }

		/// <summary>
		/// Access the current state date.
		/// </summary>
		AnimState currState { get { return animStateList[currLayerIdx].Find(x => x.AnimName == spriterAnimator.CurrentAnimation.Name); } }


		/// <summary>
		/// This method is used to handle automatic transitions from states. Call it every frame from a Monobehaviour.Update().
		/// </summary>
		public void Update()
		{
			// Do for the running state.
			if (spriterAnimator.Progress >= 1) {
				AnimState currAnimState = currState;
				Assert.IsNotNull<AnimState>(currAnimState, "Something went wrong. The current animation wasn't found in the current layer!" + spriterAnimator.CurrentAnimation.Name);
				if (currAnimState.AutoTransition) {
					SpriterAnimatorPlay(currAnimState.NextState);
				}
			}
		}

		/// <summary>
		/// Create a layer in the animator. This method will leave the IsReady flag true.
		/// 
		/// The first state in the list will be the entry state. It is set when the layer gain priority.
		/// </summary>
		/// <param name="layer">The layer to setup.</param>
		/// <param name="statesList">The states to fill the layer. Each entry must have State's Name and Animation's name [name, animName]</param>
		public void SpriterAnimatorSetupLayer(int layer, params string[][] statesList)
		{
			Assert.IsTrue(layer >= 0 && layer < spriterLayerAmount, "Invalid layer ID received: " + layer);
			Assert.IsTrue(statesList.Length > 0, "Must be at least one state in the layer.");

			// First setup. Create the array.
			if (animStateList == null) {
				// Create array.
				animStateList = new List<AnimState>[spriterLayerAmount];
				// Create lists.
				for (int i = 0; i < animStateList.Length; i++) {
					animStateList[i] = new List<AnimState>();
				}
			}

			// Create the states in the layer.
			for (int i = 0; i < statesList.Length; i++) {
				AnimState newState = new AnimState(statesList[i][0], statesList[i][1]);
				animStateList[layer].Add(newState);
				//Debug.Log("Created a new state. Name: " + newState.Name + " - Anim: " + newState.AnimName);
			}

			// Sets the first animation as the entry animation.
			animStateList[layer][0].IsEntry = true;

			// Automatically creates the "AnyState" state.
			animStateList[layer].Add(new AnimState(AnyStateLabel, ""));

			IsReady = true;
		}

		/// <summary>
		/// Dump the states inside animation's list. Testing purpose only.
		/// </summary>
		void DumpAnimStateList()
		{
			for (int i = 0; i < animStateList.Length; i++) {
				Debug.Log("Display entries for layer: " + i);
				for (int j = 0; j < animStateList[i].Count; j++) {
					Debug.Log("Layer " + i + " - entry: " + animStateList[i][j].Name + " anim name: " + animStateList[i][j].Name);
				}
			}
		}

		/// <summary>
		/// Setup a bool for a state.
		/// </summary>
		/// <param name="layer">The animator layer.</param>
		/// <param name="state">The state to setup.</param>
		/// <param name="param">The parameter name.</param>
		/// <param name="value">The parameter value.</param>
		/// <param name="nextState">The next state to go.</param>
		public void SpriterAnimatorSetupStateBool(int layer, string state, string param, bool value, string nextState)
		{
			Assert.IsTrue(layer >= 0 && layer < spriterLayerAmount, "Invalid layer ID received: " + layer);
			AnimState animState = animStateList[layer].Find(x => x.Name == state);
			Assert.IsNotNull<AnimState>(animState, "The state wasn't found in the given layer!");
			Assert.IsTrue(param.Length > 0, "Invalid bool parameter name: \"" + param + "\"");
			Assert.IsTrue(nextState.Length > 0, "Invalid next state name: \"" + param + "\"");
			Assert.IsTrue(animStateList[layer].Exists(x => x.Name == nextState), "The next state doesn't exist in the given layer. Layer: " + layer + " - State: " + nextState);
        
			AnimState.BoolParam newParam = new AnimState.BoolParam();
			newParam.Name = param;
			newParam.Value = value;
			newParam.NextState = nextState;
			animState.BoolParamsList.Add(newParam);
		}

		/// <summary>
		/// Setup a trigger for a state.
		/// </summary>
		/// <param name="layer">The animator layer.</param>
		/// <param name="state">The state to setup.</param>
		/// <param name="param">The parameter name.</param>
		/// <param name="nextState">The next state to go.</param>
		public void SpriterAnimatorSetupStateTrigger(int layer, string state, string param, string nextState)
		{
			Assert.IsTrue(layer >= 0 && layer < spriterLayerAmount, "Invalid layer ID received: " + layer);
			AnimState animState = animStateList[layer].Find(x => x.Name == state);
			Assert.IsNotNull<AnimState>(animState, "The state wasn't found in the given layer!");
			Assert.IsTrue(param.Length > 0, "Invalid trigger parameter name: \"" + param + "\"");
			Assert.IsTrue(nextState.Length > 0, "Invalid next state name: \"" + param + "\"");
			Assert.IsTrue(animStateList[layer].Exists(x => x.Name == nextState), "The next state doesn't exist in the given layer. Layer: " + layer + " - State: " + nextState);

			AnimState.TriggerParam newParam = new AnimState.TriggerParam();
			newParam.Name = param;
			newParam.NextState = nextState;
			animState.TriggerParamsList.Add(newParam);
		}

		/// <summary>
		/// Setup a custom playback speed for the animation. Default is 1.
		/// </summary>
		/// <param name="layer">The animator layer.</param>
		/// <param name="state">The state to setup.</param>
		/// <param name="speed">The new playback speed.</param>
		public void SpriterAnimatorSetupStateSpeed(int layer, string state, float speed)
		{
			Assert.IsTrue(layer >= 0 && layer < spriterLayerAmount, "Invalid layer ID received: " + layer);
			AnimState animState = animStateList[layer].Find(x => x.Name == state);
			Assert.IsNotNull<AnimState>(animState, "The state wasn't found in the given layer!");

			animState.SetPlaybackSpeed(speed);
		}

		/// <summary>
		/// Setup an automatic transition from an state to another.
		/// </summary>
		/// <param name="layer">The animator layer.</param>
		/// <param name="state">The state to setup.</param>
		/// <param name="nextState">The next state from the transition.</param>
		public void SpriterAnimatorSetupAutomaticTransition(int layer, string state, string nextState)
		{
			Assert.IsTrue(layer >= 0 && layer < spriterLayerAmount, "Invalid layer ID received: " + layer);
			AnimState animState = animStateList[layer].Find(x => x.Name == state);
			Assert.IsNotNull<AnimState>(animState, "The state wasn't found in the given layer!");
			Assert.IsTrue(animStateList[layer].Exists(x => x.Name == nextState), "The next state doesn't exist in the given layer. Layer: " + layer + " - State: " + nextState);

			animState.SetAutomaticTransition(nextState);
		}

		/// <summary>
		/// Implements the SetBool() functionality for SpriterAnimator.
		/// </summary>
		/// <param name="param">The param to set.</param>
		/// <param name="value">The value to set.</param>
		void SpriterAnimatorSetBool(string param, bool value)
		{
			if (IsReady != true) return;

			AnimState currAnimState = currState;
			Assert.IsNotNull<AnimState>(currAnimState, "Something went wrong. The current animation wasn't found in the current layer!");
        
			// We need the bool name and bool value (true or false).
			int paramIdx = currAnimState.BoolParamsList.FindIndex(x => (x.Name == param && x.Value == value));
			if (paramIdx < 0) {
				paramIdx = anyState.BoolParamsList.FindIndex(x => (x.Name == param && x.Value == value));
				if (paramIdx >= 0) {
					currAnimState = anyState;
				} else {
					return;
				}
			}
			//Debug.Log("currAnim B: " + currAnimState.Name + " - idx: " + paramIdx + " - boolParamCount: " + currAnimState.BoolParamsList.Count);
        
			AnimState.BoolParam boolParam = currAnimState.BoolParamsList[paramIdx];
			SpriterAnimatorPlay(boolParam.NextState);
		}

		/// <summary>
		/// Implements the SetTrigger() functionality for SpriterAnimator.
		/// </summary>
		/// <param name="param">The param to set.</param>
		/// <param name="value">The value to set.</param>
		void SpriterAnimatorSetTrigger(string param)
		{
			if (IsReady != true) return;

			AnimState currAnimState = currState;
			Assert.IsNotNull<AnimState>(currAnimState, "Something went wrong. The current animation wasn't found in the current layer!");

			// We need the bool name and bool value (true or false).
			int paramIdx = currAnimState.TriggerParamsList.FindIndex(x => x.Name == param);
			if (paramIdx < 0) {
				paramIdx = anyState.BoolParamsList.FindIndex(x => x.Name == param);
				if (paramIdx < 0) return;

			}

			AnimState.TriggerParam triggerParam = currAnimState.TriggerParamsList[paramIdx];
			SpriterAnimatorPlay(triggerParam.NextState);
		}

		/// <summary>
		/// Implements the SetLayerWeight() functionality for SpriterAnimator. If "weight > 0", the currently layer
		/// being used will be set to "layer". Otherwise, the base layer will be set as current.
		/// </summary>
		/// <param name="layer">The layer index.</param>
		/// <param name="weight">The layer weight.</param>
		/// <param name="overrideState">Override entry state of the new layer.</param>
		void SpriterAnimatorSetLayerWeight(int layer, int weight, string customEntryState)
		{
			if (IsReady != true) return;
        
			Assert.IsTrue(layer > 0 && layer < spriterLayerAmount, "Received invalid layer ID: " + layer);
			currLayerIdx = (weight > 0) ? layer : defaultLayerIdx;

			// Look for the layer's entry state.
			AnimState entryState;  
			if (customEntryState != "") {
				// Use custom entry state.
				entryState = animStateList[currLayerIdx].Find(x => x.Name == customEntryState);
				Assert.IsNotNull<AnimState>(entryState, "Something went wrong. The custom entry state for the current layer wasn't found: " + currLayerIdx + " - state: " + customEntryState);
			} else {
				// Use default entry state.
				entryState = animStateList[currLayerIdx].Find(x => x.IsEntry == true);
				Assert.IsNotNull<AnimState>(entryState, "Something went wrong. There is no entry state for the current layer: " + currLayerIdx);
			}

			SpriterAnimatorPlay(entryState.Name);

		}

		/// <summary>
		/// Plays an animation within spriter animator.
		/// </summary>
		/// <param name="state">The state to be executed.</param>
		/// <param name="speed">The playback speed.</param>
		void SpriterAnimatorPlay(string state)
		{
			if (IsReady != true) return;

			AnimState animState = animStateList[currLayerIdx].Find(x => x.Name == state);
			Assert.IsNotNull<AnimState>(animState, "The state wasn't found in the given layer! " + state + " (" + currLayerIdx + ")");

			spriterAnimator.Speed = animState.PlaybackSpeed;
			spriterAnimator.Play(animState.AnimName);
		}
	}
} // namespace CSGameUtils

