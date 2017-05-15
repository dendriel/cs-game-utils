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
using BehaviorLibrary.Components.Composites;
using BehaviorLibrary.Components.Conditionals;
using BehaviorLibrary.Components.Actions;
using BehaviorLibrary;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Side Scrolling Patrol behavior.
	/// </summary>
	[RequireComponent(typeof(ICharacterDriver))]
	public class SDPatrolBehavior : WaspBehavior
	{
		/// <summary>
		/// Character driver (onwer of the action).
		/// </summary>
		ICharacterDriver charDriver;

		/// <summary>
		/// Target layer.
		/// </summary>
		[SerializeField]
		LayerMask targetLayer;

		/// <summary>
		/// Range to check. X is the horizontal range. Y is the vertical interval.
		/// Size of the collision box casted in front of the character to check if
		/// an enemy has entered its range.
		/// </summary>
		[SerializeField]
		Vector2 range;

		/// <summary>
		/// The points to be patrolled by this character.
		/// </summary>
		[SerializeField]
		Transform[] pointsToPatrol;
		public Transform[] GetPointsToPatrol() { return pointsToPatrol; }

		/// <summary>
		/// The starting patrol point.
		/// </summary>
		[SerializeField]
		int startingPointToPatrolIdx;

		/// <summary>
		/// If the character movement type is to run.
		/// </summary>
		[SerializeField]
		bool run;

		/// <summary>
		/// If the character can move vertically.
		/// </summary>
		[SerializeField]
		bool axisMvmt;

		/// <summary>
		/// Minimun cooldown time before moving to the next patrol point.
		/// </summary>
		[SerializeField]
		float minTimeToWaitBetweenMvmt;

		/// <summary>
		/// Maximum cooldown time before moving to the next patrol point.
		/// </summary>
		[SerializeField]
		float maxTimeToWaitBetweenMvmt;

		/// <summary>
		/// The current destination index
		/// </summary>
		int currDestnIndex = 0;
		public int GetCurrDestnIndex() { return currDestnIndex; }

		/// <summary>
		/// The current destination.
		/// </summary>
		// To be used in the components.
		public Vector3 GetCurrDestn() {return pointsToPatrol[currDestnIndex].position; }
		public void SetCurrDestnIndex(int index) { currDestnIndex = index; }

		/// <summary>
		/// Cooldown to handle pauses when reaching the current destination.
		/// </summary>
		Cooldown waitSomeTimeCooldown;

		// Use this for initialization
		protected override void Start ()
		{
			charDriver = GetComponent<ICharacterDriver>();
			// Check if range was initialized.
			Assert.IsTrue(range != Vector2.zero, "Range parameter was not initialized.");

			// Check if there at least 2 points to be patrolled.
			Assert.IsNotNull<Transform[]>(pointsToPatrol);
			Assert.IsTrue(pointsToPatrol.Length >= 2);
			// Check if current destn index is valid.
			Assert.IsTrue((startingPointToPatrolIdx >= 0) && (startingPointToPatrolIdx < pointsToPatrol.Length));

			base.Start();
		}
	
		public override void Sanitize()
		{
			waitSomeTimeCooldown = new Cooldown(minTimeToWaitBetweenMvmt, maxTimeToWaitBetweenMvmt);
			base.Sanitize();
		}

		protected override void BuildBehavior()
		{
			Conditional isAttacking = new IsAttackingConditional(charDriver);

			Conditional isTargetInRange = new IsTargetInRangeConditional(charDriver, targetLayer, range);
			BehaviorAction attack = new AttackAAction(charDriver);
			Sequence checkTargetSeq = new Sequence(isTargetInRange, attack);

			Conditional isWaiting = new IsWaitingConditional(waitSomeTimeCooldown);

			Conditional arrivedAtDestn = new IsArrivedAtDestnConditional(charDriver, GetCurrDestn);
			BehaviorAction stopMvmt = new StopMovementAction(charDriver, true);
			BehaviorAction pickNextDestn = new PickNextDestnAction<Transform>(GetPointsToPatrol, GetCurrDestnIndex, SetCurrDestnIndex);
			BehaviorAction waitSomeTime = new WaitSomeTimeAction(charDriver, waitSomeTimeCooldown);
			Sequence checkArrivedSeq = new Sequence(arrivedAtDestn, pickNextDestn, stopMvmt, waitSomeTime);

			BehaviorAction moveToDestn = new MoveToDestnAction(charDriver, GetCurrDestn, run, axisMvmt);
			Selector checkMovementSel = new Selector(checkArrivedSeq, moveToDestn);

			Selector patrol = new Selector(isAttacking, checkTargetSeq, isWaiting, checkMovementSel);
			
			behavior = new Behavior(new Sequence(patrol));
		}
        
		/// <summary>
		/// Display the attack area.
		/// </summary>
		void OnDrawGizmosSelected()
		{
			ICharacterDriver driver = GetComponent<ICharacterDriver>();
			RaycastOrigins raycastOrigins = driver.GetRaycastOrigins();
			bool isFacingRight = driver.IsFacingRight();

			Vector2 center = ((isFacingRight) ? raycastOrigins.rightCenter : raycastOrigins.leftCenter);

			// Centralize the central point.
			center.x += (range.x / 2) * ((isFacingRight) ? 1 : -1);
			Gizmos.DrawWireCube(center, new Vector3(range.x, range.y, 0));
		}
	}
} // namespace CSGameUtils
