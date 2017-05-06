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

/// <summary>
/// The mouse controller is meant to be used with other controllers.
/// </summary>
public class MouseController : IPlayerController
{
    // From Unity docs: button values are 0 for left button, 1 for right button, 2 for the middle button.
	int AttackAKey = 0;
	int AttackBKey = 1;
    int ActionKey = 2;

	public string Type()
	{
		return this.GetType ().Name;
	}

	public uint ID()
	{
		return 0;
	}

	public void Update()
	{
		// skip.
	}

    // Unused.
	public bool StartDown()	{ return false;	}	
	public bool StartPressed()	{ return false;	}
	public bool LeftPressed()	{ return false;	}
	public bool LeftReleased()	{ return false; }
	public bool RightPressed()	{ return false; }
	public bool RightReleased()	{ return false; }
	public bool TopPressed()	{ return false; }
	public bool DownPressed()	{ return false; }
    public bool DodgePressed() { return false; }
    public bool DodgeDown() { return false; }
    public bool JumpDown() { return false; }
    public bool JumpPressed() { return false; }
    public bool BlockPressed() { return false; }
    public bool BlockDown() { return false; }
    public bool BlockReleased() { return false; }

    public bool AttackAPressed()
	{
		return Input.GetMouseButton(AttackAKey);
	}

	public bool AttackADown()
	{
		return Input.GetMouseButtonDown(AttackAKey);
    }

    public bool AttackAReleased()
    {
        return Input.GetMouseButtonUp(AttackAKey);
    }

    public bool AttackBPressed()
    {
        return Input.GetMouseButton(AttackBKey);
    }

    public bool AttackBDown()
    {
        return Input.GetMouseButtonDown(AttackBKey);
    }

    public bool AttackBReleased()
    {
        return Input.GetMouseButtonUp(AttackBKey);
    }

	public bool ActionDown ()
    {
        return Input.GetMouseButton(ActionKey);
    }

	public bool ActionPressed ()
    {
        return Input.GetMouseButtonDown(ActionKey);
    }
}
