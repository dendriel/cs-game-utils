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

/// <summary>
/// Interface for player controllers.
/// 
/// Useful for allowing the player to use different controllers.
/// </summary>
public interface IPlayerController
{
	// Controller type (Class name).
	string Type();
	// The ID is useful when setting up USB controllers. It is used to define the controller keys.
	uint ID();
	void Update();
	bool LeftPressed();
	bool LeftReleased();
	bool RightPressed();
	bool RightReleased();
	bool TopPressed();
	bool DownPressed();
	bool AttackAPressed();
	bool AttackADown();
    bool AttackAReleased();
    bool AttackBPressed();
	bool AttackBDown();
    bool AttackBReleased();
    bool JumpPressed ();
	bool JumpDown();
	bool DodgePressed();
	bool DodgeDown();
    bool StartDown();
	bool StartPressed();
	bool BlockDown();
	bool BlockPressed();
	bool BlockReleased ();

	bool ActionDown();
	bool ActionPressed();
}
