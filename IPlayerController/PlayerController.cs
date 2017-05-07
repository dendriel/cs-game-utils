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

namespace CSGameUtils {

/// <summary>
/// Provides functionality to control an entity (character, interface, etc).
/// </summary>
public class PlayerController : MonoBehaviour
{
    IPlayerController movementCtrl;
    IPlayerController actionCtrl;
    
	// Use this for initialization
    void Start()
    {
        movementCtrl = new KeyboardController();
        actionCtrl = new MouseController();
    }

    // Update is called once per frame
    void Update()
    {
		// WARNING: don't forget to call Update() from controllers' instances!
        movementCtrl.Update();
        actionCtrl.Update();

        HandleAttack();
		HandleActions();
        HandleOptions();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handle the options commands.
    /// </summary>
    void HandleOptions()
    {
		if (movementCtrl.StartDown()) {
			Debug.Log("Game paused!");
		}
    }

	/// <summary>
	/// Handle attack commands.
	/// </summary>
	void HandleAttack()
	{
		if (actionCtrl.AttackAReleased()) {
			Debug.Log("Attack A!!");
		} else if (actionCtrl.AttackBReleased()) {
			Debug.Log("Attack B!!");
		}
	}

	/// <summary>
	/// Handle actions commands.
	/// </summary>
	void HandleActions()
	{
		if (movementCtrl.JumpDown()) {
			Debug.Log("Jumping!");
		} else if (movementCtrl.DodgeDown()) {
			Debug.Log("Dodging!");
		} else if (actionCtrl.ActionPressed()) {
			Debug.Log("Action pressed!!");
		}
	}

    /// <summary>
    /// Handle the player actions (commands to the character).
    /// </summary>
    void HandleMovement()
    {
        // Horizontal movement.
        if (movementCtrl.LeftPressed()) {
			Debug.Log("Move to the left.");
        } else if (movementCtrl.RightPressed()) {
			Debug.Log("Move to the right.");
        }

        // Vertical movement.
        if (movementCtrl.TopPressed()) {
			Debug.Log("Move to the top.");
        } else if (movementCtrl.DownPressed()) {
			Debug.Log("Move to the bottom.");
		}
    }
}
} // namespace CSGameUtils