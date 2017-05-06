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
using System;

public class USBAndKeyboardController : IPlayerController
{
	USBController usbCtrl;
	KeyboardController keyCtrl;
	
	
	public string Type()
	{
		return this.GetType ().Name;
	}

	public uint ID()
	{
		return usbCtrl.ID();
	}

	public USBAndKeyboardController (uint id)
	{
		usbCtrl = new USBController(id);
		keyCtrl = new KeyboardController();
	}

	public void Update()
	{
		usbCtrl.Update();
		keyCtrl.Update();
	}

	public bool StartDown()
	{
		return (usbCtrl.StartDown() || keyCtrl.StartDown());
	}
	
	public bool StartPressed()
	{
		return (usbCtrl.StartPressed() || keyCtrl.StartPressed());
	}
	
	public bool LeftPressed()
	{
		return (usbCtrl.LeftPressed() || keyCtrl.LeftPressed());
	}
	
	public bool LeftReleased()
	{
		return (usbCtrl.LeftReleased() || keyCtrl.LeftReleased());
	}
	
	public bool RightPressed()
	{
		return (usbCtrl.RightPressed() || keyCtrl.RightPressed());
	}
	
	public bool RightReleased()
	{
		return (usbCtrl.RightReleased() || keyCtrl.RightReleased());
	}
	
	public bool TopPressed()
	{
		return (usbCtrl.TopPressed() || keyCtrl.TopPressed());
	}
	
	public bool DownPressed()
	{
		return (usbCtrl.DownPressed() || keyCtrl.DownPressed());
	}
	
	public bool AttackAPressed()
	{
		return (usbCtrl.AttackAPressed() || keyCtrl.AttackAPressed());
	}
	
	public bool AttackADown()
	{
		return (usbCtrl.AttackADown() || keyCtrl.AttackADown());
	}

	public bool AttackAReleased()
	{
		throw new NotImplementedException();
	}

	public bool AttackBPressed()
	{
		return (usbCtrl.AttackBPressed() || keyCtrl.AttackBPressed());
	}
	
	public bool AttackBDown()
	{
		return (usbCtrl.AttackBDown() || keyCtrl.AttackBDown());
	}

	public bool AttackBReleased()
	{
		throw new NotImplementedException();
	}

	public bool DodgePressed ()
	{
		return (usbCtrl.DodgePressed() || keyCtrl.DodgePressed());
	}

	public bool DodgeDown ()
	{
		return (usbCtrl.DodgeDown() || keyCtrl.DodgeDown());
	}

	public bool JumpDown()
	{
		return (usbCtrl.JumpDown() || keyCtrl.JumpDown());
	}
	
	public bool JumpPressed()
	{
		return (usbCtrl.JumpPressed() || keyCtrl.JumpPressed());
	}

	public bool BlockDown ()
	{
		return (usbCtrl.BlockDown() || keyCtrl.BlockDown());
	}

    public bool BlockPressed ()
	{
		return (usbCtrl.BlockPressed() || keyCtrl.BlockPressed());
	}

	public bool BlockReleased ()
	{
		return (usbCtrl.BlockReleased() || keyCtrl.BlockReleased());
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
