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

public class KeyboardController : IPlayerController
{
	KeyCode StartKey = KeyCode.Return;
	KeyCode LeftKey  = KeyCode.A;
	KeyCode RightKey = KeyCode.D;
	KeyCode TopKey   = KeyCode.W;
	KeyCode DownKey  = KeyCode.S;
	KeyCode AttackAKey = KeyCode.J;
	KeyCode AttackBKey = KeyCode.K;
	KeyCode BlockKey = KeyCode.L;
	KeyCode DodgeKey = KeyCode.H;
	KeyCode JumpKey    = KeyCode.Space;

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

	public bool StartDown()
	{
		return Input.GetKeyDown (StartKey);
	}
	
	public bool StartPressed()
	{
		return Input.GetKey (StartKey);
	}

	public bool LeftPressed()
	{
		return Input.GetKey (LeftKey);
	}

	public bool LeftReleased()
	{
		return Input.GetKeyUp (LeftKey);
	}

	public bool RightPressed()
	{
		return Input.GetKey (RightKey);
	}

	public bool RightReleased()
	{
		return Input.GetKeyUp (RightKey);
	}

	public bool TopPressed()
	{
		return Input.GetKey (TopKey);
	}

	public bool DownPressed()
	{
		return Input.GetKey (DownKey);
	}

	public bool AttackAPressed()
	{
		return Input.GetKey (AttackAKey);
	}

	public bool AttackADown()
	{
		return Input.GetKeyDown (AttackAKey);
    }

    public bool AttackAReleased()
    {
        return Input.GetKeyUp(AttackAKey);
    }

    public bool AttackBPressed()
	{
		return Input.GetKey (AttackBKey);
	}

    public bool AttackBDown()
	{
		return Input.GetKeyDown (AttackBKey);
    }

    public bool AttackBReleased()
    {
        return Input.GetKeyUp(AttackBKey);
    }

    public bool DodgePressed ()
	{
		return Input.GetKey(DodgeKey);
	}

	public bool DodgeDown ()
	{
		return Input.GetKeyDown(DodgeKey);
	}

	public bool JumpDown()
	{
		return Input.GetKeyDown (JumpKey);
	}

	public bool JumpPressed()
	{
		return Input.GetKey (JumpKey);
	}

	public bool BlockPressed()
	{
		return Input.GetKey(BlockKey);
	}

	public bool BlockDown()
	{
		return Input.GetKeyDown(BlockKey);
	}

	public bool BlockReleased ()
	{
		return Input.GetKeyUp(BlockKey);
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
