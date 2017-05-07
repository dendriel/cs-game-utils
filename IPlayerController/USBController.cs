/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Player Controller Interface.
 *
 *	Player Controller Interface is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Player Controller Interface is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Player Controller Interface. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;
using System;


namespace CSGameUtils {

/// <summary>
/// Create an USB Controller.
/// 
/// WARNING: it's necessary to setup the controller keys in Unity ("Edit->Settings->Input") so the USB controller
/// can work properly.
/// </summary>
public class USBController : IPlayerController
{	
	/// <summary>
	/// Gets the controller ID.
	/// </summary>
	/// <value>The ID.</value>
	uint _ID;

	string startButton;
	string horizontalButton;
	string verticalButton;
	string attackAButton;
	string attackBButton;
	string jumpButton;
	string dodgeButton;
	string blockButton;

	/// <summary>
	/// The button name prefix.
	/// </summary>
	const string buttonNamePrefix = "";

	bool leftPressed = false;
	bool rightPressed = false;
	bool leftReleased = false;
	bool rightReleased = false;

	public string Type()
	{
		return this.GetType ().Name;
	}

	public uint ID()
	{
		return _ID;
	}

	/// <summary>
	/// Create an USB controller mapping.
	/// 
	/// Use "id = 0" to listen to all USB controllers.
	/// </summary>
	/// <param name="id"></param>
	public USBController(uint id)
	{
		_ID = id;

		string idStr = (id != 0)? id.ToString() : "";

		// When using multiple controllers, you may prefix the input keys as "Joystick" + "controller ID" + "key name".
		// For example: Joystick0Submit, Joystick1Submit, Joystick2Submit, etc.
		// Also, 0 is a key ID and should be used when no ID is necessary.
		startButton		  = buttonNamePrefix + idStr + "Submit";
		horizontalButton  = buttonNamePrefix + idStr + "Horizontal";
		verticalButton    = buttonNamePrefix + idStr + "Vertical";
		attackAButton     = buttonNamePrefix + idStr + "Fire4";
		attackBButton     = buttonNamePrefix + idStr + "Fire1";
		dodgeButton       = buttonNamePrefix + idStr + "Fire2";
		jumpButton        = buttonNamePrefix + idStr + "Fire3";
		// To use a "block button" it's necessary to create this input ("Edit->Settings->Input").
		//blockButton       = buttonNamePrefix + idStr + "Block";
	}

	/// <summary>
	/// Update this instance. Needed in order to use "Released" functions.
	/// </summary>
	public void Update()
	{
		if (LeftPressed()) {
			leftPressed = true;
			leftReleased = false;

		} else {
			if (leftPressed == true) {
				leftPressed = false;
				leftReleased = true;

			} else {
				leftReleased = false;
			}
		}

		// If right is pressed in this frame.
		if (RightPressed()) {
			rightPressed = true;
			rightReleased = false;

		} else {
			// If right was pressed previously.
			if (rightPressed) {
				rightPressed = false;
				rightReleased = true;

			} else {
				rightReleased = false;
			}
		}
	}

	public bool StartDown()
	{
		return Input.GetButtonDown(startButton);
	}
	
	public bool StartPressed()
	{
		return Input.GetButton(startButton);
	}

	public bool LeftPressed()
	{
		return (Input.GetAxis(horizontalButton) < 0);
	}

	public bool LeftReleased()
	{
		return leftReleased;
	}

	public bool RightPressed()
	{
		return (Input.GetAxis(horizontalButton) > 0);	
	}

	public bool RightReleased()
	{
		return rightReleased;
	}

	public bool TopPressed()
	{
		return (Input.GetAxis(verticalButton) > 0);
	}

	public bool DownPressed()
	{
		return (Input.GetAxis(verticalButton) < 0);
	}

	public bool AttackAPressed()
	{
		return Input.GetButton(attackAButton);
	}

	public bool AttackADown()
	{
		return Input.GetButtonDown(attackAButton);
	}

	public bool AttackAReleased()
	{
		throw new NotImplementedException();
	}

	public bool AttackBPressed()
	{
		return Input.GetButton(attackBButton);	
	}
	
	public bool AttackBDown()
	{
		return Input.GetButtonDown(attackBButton);
	}

	public bool AttackBReleased()
	{
		throw new NotImplementedException();
	}

	public bool DodgePressed ()
	{
		return Input.GetButton(dodgeButton);
	}

	public bool DodgeDown ()
	{
		return Input.GetButtonDown(dodgeButton);
	}

	public bool JumpDown()
	{
		return Input.GetButtonDown(jumpButton);
	}

	public bool JumpPressed()
	{
		return Input.GetButton(jumpButton);
	}

	public bool BlockDown ()
	{
		throw new UnityException("Block button must be set before being used!");
		//return Input.GetButtonDown(blockButton);
	}
	
    public bool BlockPressed ()
	{
		throw new UnityException("Block button must be set before being used!");
		//return Input.GetButton(blockButton);
	}

	public bool BlockReleased ()
	{
		throw new UnityException("Block button must be set before being used!");
		//return Input.GetButtonUp(blockButton);
	}

	public bool ActionDown ()
	{
		return JumpDown() || StartDown();
	}

	public bool ActionPressed ()
	{
		return JumpPressed() || StartPressed();
	}
}
} // namespace CSGameUtils
