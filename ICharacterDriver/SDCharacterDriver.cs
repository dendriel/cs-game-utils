/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Character Driver.
 *
 *	Character Driver is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Character Driver is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Character Driver. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Default Side Scrolling driver for characters. Contains basic actions.
	/// 
	/// The suitable development partner is the following:
	/// 
	/// - Create "default" characters. The main character, enemies and npcs. Don't mix controllers with the driver;
	/// - Create controllers and attach to the character as needed. You may use a "PlayerController", for controlling
	/// the main character, and Behaviors for controlling the enemies and NPCs.
	/// - Attach the needed controllers to the characters when spawming then.
	/// 
	/// WARNING: must have a circle collider! There is no room in the RequireComponent statement.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
	public class SDCharacterDriver : MonoBehaviour, ICharacterDriver
	{
		/// <summary>
		/// Character's rigidy body.
		/// </summary>
		protected Rigidbody2D rb2D;

		/// <summary>
		/// Character's animator.
		/// </summary>
		protected Animator anim;

		/// <summary>
		/// Character's box collider (used to check if the chracter is grounded).
		/// </summary>
		protected BoxCollider2D coll2D;

		/// <summary>
		/// Character's ground collider (affected by friction).
		/// 
		/// WARNING: It's necessary to setup a Physics 2D Material for this collider (because it uses friction to keep
		/// the character from moving when standing on an inclination).
		/// </summary>
		protected Collider2D groundColl2D;

		/// <summary>
		/// Character sprite is facing right? Set this parameter when creating the character prefab.
		/// </summary>
		[SerializeField]
		bool facingRight;
		public bool FacingRight { get { return facingRight; } }
	
		/// <summary>
		/// Returns the current character velocity.
		/// </summary>
		public Vector2 Velocity { get { return rb2D.velocity; } }

		/// <summary>
		/// Returns the character run speed.
		/// </summary>
		public Vector2 RunSpeed { get { return runSpeed * GravityFactor; } }

		/// <summary>
		/// Returns the character walk speed.
		/// </summary>
		public Vector2 WalkSpeed { get { return walkSpeed * GravityFactor; } }

		/// <summary>
		/// Returns the character jump force.
		/// </summary>
		public float JumpForce { get { return jumpForce * GravityFactor; } }

		/// <summary>
		/// Flag to tell if the character is inside a body of water.
		/// </summary>
		public bool IsInsideWater { get; set; }

		/// <summary>
		/// Get and set the character gravity factor (0 - no factor; 1 - maximum).
		/// The get function will return a value to be multiplied by a movement value (walk, run, jump, etc).
		/// This means that the value won't be the same defined by the Set function.
		/// </summary>
		public float GravityFactor {
			get { return (1 - gravityFactor); }
			set { gravityFactor = Mathf.Clamp(value, 0, 1); }
		}

		/// <summary>
		/// Walk speed.
		/// </summary>
		[SerializeField]
		Vector2 walkSpeed;

		/// <summary>
		/// Run speed.
		/// </summary>
		[SerializeField]
		Vector2 runSpeed;

		/// <summary>
		/// Jump force.
		/// </summary>
		[SerializeField]
		[Tooltip("May be limited due to MaxAscendingSpeed parameter.")]
		protected float jumpForce;

		/// <summary>
		/// Maximum speed to reach while ascending (jumping). Used to avoid bugs from physics, that may "launch"
		/// the character in the air due to a certain collision situation.
		/// </summary>
		[SerializeField]
		protected float MaxAscendingSpeed;

		/// <summary>
		/// Maximum speed to reach while falling.
		/// </summary>
		[SerializeField]
		[Tooltip("Use a negative value for falling speed.")]
		protected float MaxFallingSpeed;

		/// <summary>
		/// Affects the character movement (used to simulate different gravity forces, e.g.: inside the water).
		/// </summary>
		[Tooltip("Affects the character movement while not attached. (0 - no factor; 1 - maximum). Use to create different graivities.")]
		[SerializeField]
		protected float gravityFactor;
	
		/// <summary>
		/// Reduces the vertical movement while the character is in the air (and not pressing the movement keys).
		/// </summary>
		[Tooltip("Affects the character movement speed while in the air. Use it to easier the air control. (e.g: .95")]
		[SerializeField]
		protected float airMvmtFactor;

		/// <summary>
		/// The player can change direction while in the air? (useful for flying characters. e.g.: a bird).
		/// </summary>
		[Tooltip("Allow character to control movement while in the air.")]
		[SerializeField]
		protected bool airControl;

		/// <summary>
		/// Default ground collider friction (the character's collider that touches the ground).
		/// This property must be set manually. Getting it via code from the shared material doesn't work well
		/// because changing it while playing also change its original value.
		/// </summary>
		[SerializeField]
		protected float defaultFriction;

		/// <summary>
		/// Allow the character to move vertically (enables the walk/run Up and Down functions).
		/// </summary>
		[Tooltip("Allow character to move up and down.")]
		[SerializeField]
		protected bool axisMovement;

		/// <summary>
		/// The character may die by falling.
		/// </summary>
		[SerializeField]
		[Tooltip("True for the player's character; False for NPCs.")]
		protected bool MayDie;
    
		/// <summary>
		/// Ground Layer for this character.
		/// </summary>
		[SerializeField]
		protected LayerMask whatIsGround;
		public LayerMask WhatIsGround() { return whatIsGround; }

		/// <summary>
		/// The grounded check function always return true. Useful for controlling characters that may fly.
		/// </summary>
		[Tooltip("If this option is set, whatIsGround variable won't be used.")]
		[SerializeField]
		protected bool alwaysGrounded;
	
		/// <summary>
		/// Default character gravity.
		/// </summary>
		public float DefaultGravity { get; private set; }

		// Animator definitions.
		// Bools and Triggers.
		protected const string walkBool = "Walk";
		protected const string runBool = "Run";
		protected const string attackATrigger = "AttackA";
		protected const string attackBTrigger = "AttackB";
		protected const string jumpTrigger = "Jump";
		protected const string hurtTrigger = "Hurt";
		protected const string fallBool = "Fall";
		protected const string dieTrigger = "Die";
		// States.
		protected string idleStateTag = "Idle";
		protected string walkStartStateTag = "WalkStart";
		protected string walkStateTag = "Walk";
		protected string walkEndStateTag = "WalkEnd";
		protected string runStartStateTag = "RunStart";
		protected string runStateTag = "Run";
		protected string runEndStateTag = "RunEnd";
		protected string attackStateTag = "Attack";
		protected string jumpStateTag = "Jump";
		protected string fallStateTag = "Fall";
		protected string landStateTag = "Land";
		protected string hurtStateTag = "Hurt";
		protected string idleAttachStartStateTag = "IdleAttachStart";
		protected string idleAttachStateTag = "IdleAttach";
		protected string idleAttachEndStateTag = "IdleAttachEnd";
		protected string walkAttachStartStateTag = "WalkAttachStart";    
		protected string walkAttachStateTag = "WalkAttach";
		protected string walkAttachEndStateTag = "WalkAttachEnd";
		protected string deadStateTag = "Dead";
		// Layers.
		protected const int groundLayerIdx = 0;

		// Use this for initialization
		protected virtual void Start ()
		{
			rb2D = GetComponent<Rigidbody2D>();
			coll2D = GetComponent<BoxCollider2D>();
			anim = GetComponent<Animator>();

			Collider2D[] collsList = GetComponents<Collider2D>();

			// Find the ground collider (the one that is not a trigger.
			foreach (Collider2D coll in collsList) {
				if (!coll.isTrigger) {
					groundColl2D = coll;
				}
			}

			DefaultGravity = rb2D.gravityScale;

			CalculateRaySpacing();
		}

		protected virtual void Update()
		{
			if (IsFalling() && (IsBeingHurt() == false) && (IsDead() == false)) {
				SetFalling();
			} else {
				UnsetFalling();
			}

			if (IsLanding() || IsAttacking() || IsDead()) {
				EnableFriction();
			}

			// Out of scenario death condition.
			if (MayDie && IsOutOfScenarioBounds()) {
				DeathByOutOfScenario();
			}
		}

		// Fixes some movement limits.
		protected virtual void FixedUpdate()
		{
			// Fix speed limits while in the air.
			if (IsGrounded() != true) {
				// Fix (limit) horizontal and vertical speeds.
				rb2D.velocity = new Vector2(rb2D.velocity.x * airMvmtFactor, Mathf.Clamp(rb2D.velocity.y, MaxFallingSpeed, MaxAscendingSpeed));
        
			// Check if the character hit the ground at high speed.
			// If the character IS grounded and it have a vertical speed, it just landed. If the landing vertical speed is above a limit
			// the character hit the ground very hard.. :D
			} else if (MayDie && IsHittingTheGroundHard()) {
				DeathByFalling();
			}
		}

		/// <summary>
		/// The character started to fall.
		/// </summary>
		protected virtual void SetFalling()
		{
			anim.SetBool(fallBool, true);
		}

		/// <summary>
		/// The character stopped to fall.
		/// </summary>
		protected virtual void UnsetFalling()
		{
			anim.SetBool(fallBool, false);
		}

		/// <summary>
		/// Trigger death by out of scenario.
		/// </summary>
		protected virtual void DeathByOutOfScenario()
		{
			throw new UnityException("DeathByOutOfScenario() must be overrided!");
			// What happens when the character dies?
		}

		/// <summary>
		/// Checks if the character is out of the scenario  bottom bound.
		/// </summary>
		/// <returns>true if out of the scenario bottom bound; false otherwise.</returns>
		protected virtual bool IsOutOfScenarioBounds()
		{
			// Example using the maximum camera bounds.
			Transform bottomLimit = Camera.main.GetComponent<Camera2DFollowMany>().BottomLimit;
			Assert.IsNotNull<Transform>(bottomLimit, "There is no camera bottom limit set.");

			if (transform.position.y < bottomLimit.position.y) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Trigger death by falling.
		/// </summary>
		protected virtual void DeathByFalling()
		{
			throw new UnityException("DeathByFalling() must be overrided!");
			// What happens when the character dies?
		}

		/// <summary>
		/// Checks if the character's landing speed is above the limit.
		/// </summary>
		/// <returns>true if the character landing speed is above the limit; false otherwise.</returns>
		protected virtual bool IsHittingTheGroundHard()
		{
			if (rb2D.velocity.y <= MaxFallingSpeed) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Move the character. Will automatically flip to the walking direction.
		/// </summary>
		public virtual void WalkRight()
		{
			Move(Vector2.right, WalkSpeed);
		}

		/// <summary>
		/// Move the character. Will automatically flip to the walking direction.
		/// </summary>
		public virtual void WalkLeft()
		{        
			Move(Vector2.left, WalkSpeed);
		}

		/// <summary>
		/// Makes the character walk up.
		/// </summary>
		public virtual void WalkUp()
		{
			if (axisMovement == false) return;
			Move(Vector2.up, WalkSpeed);
		}

		/// <summary>
		/// Makes the character walk down.
		/// </summary>
		public virtual void WalkDown()
		{
			if (axisMovement == false) return;
			Move(Vector2.down, WalkSpeed);
		}

		/// <summary>
		/// Makes the character run to the right.
		/// </summary>
		public virtual void RunRight()
		{
			Move(Vector2.right, RunSpeed);
		}

		/// <summary>
		/// Makes the character run to the left.
		/// </summary>
		public virtual void RunLeft()
		{
			Move(Vector2.left, RunSpeed);
		}

		/// <summary>
		/// Makes the character run up.
		/// </summary>
		public virtual void RunUp()
		{
			if (axisMovement == false) return;
			Move(Vector2.up, RunSpeed);
		}

		/// <summary>
		/// Makes the character run down.
		/// </summary>
		public virtual void RunDown()
		{
			if (axisMovement == false) return;
			Move(Vector2.down, RunSpeed);
		}

		public virtual void Move(Vector2 dir, Vector2 speed)
		{
			if (!CanMove()) return;

			bool grounded = IsGrounded();
        
			if ((speed.x == WalkSpeed.x) && IsRunning()) {
				anim.SetBool(runBool, false);
			} else if (speed.x == RunSpeed.x) {
				anim.SetBool(runBool, grounded);
			}								

			// If the character is in the air, moving will just set the direction, but won't add any force.
			if (grounded == false) {
				if (airControl &&
					 (((dir == Vector2.right) && (rb2D.velocity.x < 0)) ||
					  ((dir == Vector2.left) && (rb2D.velocity.x > 0)))) {
					rb2D.velocity = new Vector2(rb2D.velocity.x * -1, rb2D.velocity.y);
				}
				//return; // uncomment this line to disallow adding force in the air.
			}

			// If grounded, may flip. If not grounded but airControl, may flip.
			if (grounded || airControl) FlipTo(dir);
			
			if (dir.x != 0) {
				rb2D.velocity = new Vector2(speed.x * dir.x, rb2D.velocity.y);
			} else {
				rb2D.velocity = new Vector2(rb2D.velocity.x, speed.y * dir.y);
			}

			anim.SetBool(walkBool, grounded);
			DisableFriction();
		}

		/// <summary>
		/// Check if the character can move.
		/// </summary>
		/// <returns>true if can move; false otherwise</returns>
		public virtual bool CanMove()
		{
			return !(IsLanding() || IsAttacking() || IsBeingHurt() || IsDead());
		}

		/// <summary>
		/// Display movement dust.
		/// </summary>
		protected virtual void DisplaMovementyDust(float param)
		{
			// Example gratia:
			//
			// if (IsInsideWater) return;
			// DustManager.Inst.DisplayDust((facingRight) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
		}

		/// <summary>
		/// Display landing dust.
		/// </summary>
		protected virtual void DisplayLandingDust(float param)
		{
			// Example gratia:
			//
			// if (IsInsideWater) return;		
			// DustManager.Inst.DisplayLandingDust(new Vector3(transform.position.x, raycastOrigins.bottomCenter.y), 0.15f, "Player", 1);
		}

		/// <summary>
		/// Makes the character perform an attack.
		/// </summary>
		public virtual void AttackA()
		{
			// Not grounded, landing or running can't perform attacks or is already attacking.
			if (IsIdle() || IsWalking() || IsRunning()) {
				StopMoving();
				anim.SetTrigger(attackATrigger);
			}
		}

		/// <summary>
		/// Makes the character perform an attack.
		/// </summary>
		public virtual void AttackB()
		{
			// Not grounded, landing or running can't perform attacks or is already attacking.
			if (IsIdle() || IsWalking() || IsRunning()) {
				StopMoving();
				anim.SetTrigger(attackBTrigger);
			}
		}

		/// <summary>
		/// Perform an action.
		/// </summary>
		protected virtual void Action(float param)
		{
		}

		/// <summary>
		/// Makes the character jump.
		/// </summary>
		public virtual void Jump()
		{
			// Can't jump if not grounded or landing.
			if ((IsGrounded() == false) || IsJumping() || IsLanding() || IsBeingHurt() || IsDead() || IsInsideWater) return;
		
			anim.SetTrigger(jumpTrigger);
			EnableFriction();

			// TODO: REVIEW THIS. the animator is still adding force.
			// I believe that because the characte is still in other animation than "Jumping", the method returns
			// without adding the jump force. So, keep calling the AddJumpForceCb() from the animation. It works and
			// we can set the right key frame to start adding impulse.
			//AddJumpForceCb();
		}

		/// <summary>
		/// Display the hurt animation from the character.
		/// </summary>
		public virtual bool TakeHit<T>(T hitProperty)
		{
			if (IsBeingHurt() || IsDead()) return false;

			Debug.Log("Being hit!!");
			anim.SetTrigger(hurtTrigger);
			//EnableFriction();

			return true;
		}

		/// <summary>
		/// Add the jump force. To be called from the right frame from the jump animation.
		/// </summary>
		protected virtual void AddJumpForceCb()
		{
			// May not be jumping anymore. (maybe started falling when preparing to jump).
			// This condition may cause problems if the animations have some transition duration set.
			if (!CanJump()) return;

			// WARNING: clearing the velocity won't work sometimes.
			// Set vertical speed to zero or it will increase the jump length.
			rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
			// Add jump force.
			rb2D.AddForce(new Vector2(0, JumpForce));
		}

		/// <summary>
		/// Checks if the character is able to jump.
		/// </summary>
		/// <returns>true if it can jump; false otherwise.</returns>
		protected virtual bool CanJump()
		{
			return IsJumping();
		}

		/// <summary>
		/// Add an impulse in the character towards its facing direction.
		/// </summary>
		/// <param name="value">The impulse value.</param>
		/// <param name="reverse">The impulse should be reversed? (towards the character backing direction?)</param>
		protected virtual void AddImpulse(Vector2 value, bool reverseX = false)
		{
			// Add impulse towards facing direction.
			value.x *= ((FacingRight) ? 1 : -1);
			// Reverse if needed.
			value.x *= ((reverseX) ? -1 : 1);

			// Add jump force.
			rb2D.AddForce(value, ForceMode2D.Impulse);
		}

		/// <summary>
		/// Wrapper callback. Needed because animations doesn't support callbacks with more than one parameter.
		/// </summary>
		/// <param name="value"></param>
		protected virtual void AddImpulseCb(float value)
		{
			AddImpulse(new Vector2(value, 0));
		}
		protected virtual void AddImpulseReversedCb(float value)
		{
			AddImpulse(new Vector2(value, 0), true);
		}

		/// <summary>
		/// Stop moving animations.
		/// </summary>
		/// <param name="zeroVelocity">Set the velocity vector to zero. Useful for flying characters.</param>
		public virtual void StopMoving(bool zeroVelocity = false)
		{
			anim.SetBool(walkBool, false);
			anim.SetBool(runBool, false);
			EnableFriction();

			if (zeroVelocity) {
				rb2D.velocity = Vector2.zero;
				// Maybe it's also necessary to stop rotation.
			}
		}

		/// <summary>
		/// Flip the character.
		/// </summary>
		public virtual void Flip()
		{
			facingRight = !facingRight;
			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}

		/// <summary>
		/// Flip the character to the given direction (if necessary).
		/// </summary>
		public virtual void FlipTo(Vector2 dir)
		{
			float mvmtDir = dir.x;

			if ((facingRight && (mvmtDir < 0)) ||
				(!facingRight && (mvmtDir > 0))) {
				Flip();
			}
		}

		/// <summary>
		/// Get the characters position.
		/// </summary>
		/// <returns>The character position.</returns>
		public Vector3 GetPosition()
		{
			return transform.position;
		}

		/// <summary>
		/// Get a MonoBehavior from the character. Useful for starting coroutines in AI behaviors.
		/// </summary>
		/// <returns>A MonoBehavior reference.</returns>
		public MonoBehaviour GetMonoBehavior()
		{
			return this;
		}

		/// <summary>
		/// Set default character friction with the ground.
		/// </summary>
		public virtual void EnableFriction()
		{
			SetFriction(defaultFriction);
		}

		/// <summary>
		/// Disables the character friction.
		/// </summary>
		public virtual void DisableFriction()
		{
			SetFriction(0);
		}

		/// <summary>
		/// Change the character friction (update the ground collider).
		/// </summary>
		public virtual void SetFriction(float newFriction)
		{
			groundColl2D.sharedMaterial.friction = newFriction;
			groundColl2D.enabled = false;
			groundColl2D.enabled = true;
		}
    
		/// <summary>
		/// Checks if the character is touching the ground.
		/// </summary>
		/// <returns>true if he is grounded; false otherwise.</returns>
		public virtual bool IsGrounded()
		{
			if (alwaysGrounded) return true;

			UpdateRaycastOrigins();
			DisplayGizmos();
		
			for (int i = 0; i < verticalRayCount; i++) {
				RaycastHit2D[] hitList = Physics2D.RaycastAll(raycastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.down, hitDist.y, whatIsGround);
				for (int j = 0; j < hitList.Length; j++) {
					RaycastHit2D hit = hitList[j];
					if ((hit.collider != null) && (hit.collider.isTrigger == false)) {
						if ((hit.distance <= groundMinDist) && (rb2D.velocity.y <= 0.1f)) {
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// The character is facing to the right?
		/// </summary>
		/// <returns></returns>
		public bool IsFacingRight()
		{
			return FacingRight;
		}

		/// <summary>
		/// Check if the character is in the idle state.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		public virtual bool IsIdle() { return IsCurrentAnimationName(idleStateTag); }

		/// <summary>
		/// Check if the character is in the walking state.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		public virtual bool IsWalking() { return IsCurrentAnimationName(walkStateTag, walkStartStateTag, walkEndStateTag); }

		/// <summary>
		/// Check if the character is in the running state.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		public virtual bool IsRunning() { return IsCurrentAnimationName(runStateTag, runStartStateTag, runEndStateTag); }

		/// <summary>
		/// Check if the character is in the attacking state.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		public virtual bool IsAttacking() { return IsCurrentAnimationName(attackStateTag); }
    
		/// <summary>
		/// Check if the character is in the jumping state.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		public virtual bool IsJumping() { return IsCurrentAnimationName(jumpStateTag); }

		/// <summary>
		/// Check if the character is falling.
		/// </summary>
		/// <returns>true if he is falling; false otherwise.</returns>
		public virtual bool IsFalling() { return (IsGrounded()) ? false : (rb2D.velocity.y < 0); }

		/// <summary>
		/// Check if the character is in the landing state.
		/// </summary>
		/// <returns>true if it is; false otherwise.</returns>
		public virtual bool IsLanding() { return IsCurrentAnimationName(landStateTag); }

		/// <summary>
		/// Check if the character is being hurt.
		/// </summary>
		/// <returns>true if he is being hurt; false otherwise.</returns>
		public virtual bool IsBeingHurt() { return IsCurrentAnimationName(hurtStateTag); }

		/// <summary>
		/// Check if the character is dead.
		/// </summary>
		/// <returns>true if the character is dead; false otherwise.</returns>
		public virtual bool IsDead() { return IsCurrentAnimationName(deadStateTag); }


		/// <summary>
		/// Check if the current animation is called "name" for animator layer 0.
		/// </summary>
		/// <param name="statesList">The name of the animation. May be a list of states.</param>
		/// <returns>true if the running animation is one of the names in "statesList"; false otherwise.</returns>
		protected bool IsCurrentAnimationName(params string[] statesList)
		{
			return IsCurrentAnimationName(0, statesList);
		}

		/// <summary>
		/// Check if the current animation is called "name" for the given animator layer.
		/// </summary>
		/// <param name="statesList">The name of the animation. May be a list of states.</param>
		/// <param name="layer">The layer of the animator to check.</param>
		/// <returns>true if the running animation is one of the names in "statesList"; false otherwise.</returns>
		protected bool IsCurrentAnimationName(int layer, params string[] statesList)
		{
			for (int i = 0; i < statesList.Length; i++) {
				if (anim.GetCurrentAnimatorStateInfo(layer).IsName(statesList[i])) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Check if a trigger is set.
		/// </summary>
		/// <param name="trigger">The trigger to be checked.</param>
		/// <returns>true if the trigger is set; false otherwise.</returns>
		protected bool IsTriggerSet(string trigger)
		{
			return anim.GetBool(trigger);
		}

		/**
		 * Code to enable us to check if the character is grounded. It's based on some Yahoo! answers, but i don't remember which one.
		 */
		protected float skinWidth = .015f;
		int horizontalRayCount = 4;
		int verticalRayCount = 4;
		/// <summary>
		/// The X dimension is used to check objects ahead. The Y dimension is to check ground.
		/// </summary>
		Vector2 hitDist = new Vector2(0.25f, 0.4f);
		// Minimum distance from the ground to be considered grounded.
		float groundMinDist = 0.4f;
		float verticalRaySpacing;

		/// <summary>
		/// Call UpdateRaycastOrigins method before checking this data.
		/// 
		/// RaycastOrigins is defined at ICharacterDriver.
		/// </summary>
		public RaycastOrigins raycastOrigins;
		
		void DisplayGizmos()
		{
			//return;
			// Bottom gizmos.
			for (int i = 0; i < verticalRayCount; i++) {
				Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, new Vector2(0, hitDist.y * -1), Color.red);
			}

			if (FacingRight) {
				Debug.DrawRay(raycastOrigins.rightCenter, new Vector2(hitDist.x, 0), Color.green);
			} else {
				Debug.DrawRay(raycastOrigins.leftCenter, new Vector2(hitDist.x * -1, 0), Color.green);
			}
		}

		public void UpdateRaycastOrigins()
		{
			// Useful while in editor mode.
			if (coll2D == null) {
				coll2D = GetComponent<BoxCollider2D>();
			}

			Bounds bounds = coll2D.bounds;
			bounds.Expand(skinWidth * -2);
        
			raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
			raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
			raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

			float centerY = ((bounds.max.y - bounds.min.y) / 2) + bounds.min.y;
			float centerX = ((bounds.max.x - bounds.min.x) / 2) + bounds.min.x;
			raycastOrigins.leftCenter = new Vector2(bounds.min.x, centerY);
			raycastOrigins.rightCenter = new Vector2(bounds.max.x, centerY);
			raycastOrigins.bottomCenter = new Vector2(centerX, bounds.min.y);
			raycastOrigins.topCenter = new Vector2(centerX, bounds.max.y);
		}

		/// <summary>
		/// Get the character's raycast origins.
		/// </summary>
		/// <returns>The updated raycast origins from this character.</returns>
		public RaycastOrigins GetRaycastOrigins()
		{
			UpdateRaycastOrigins();
			return raycastOrigins;
		}

		/// <summary>
		/// Calculate ray spacing based on box collider sizes.
		/// </summary>
		void CalculateRaySpacing()
		{
			Bounds bounds = coll2D.bounds;
			bounds.Expand(skinWidth * -2);

			horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
			verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
        
			verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}
	}
} // namespace CSGameUtils