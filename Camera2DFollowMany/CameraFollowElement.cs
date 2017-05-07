/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Camera 2D Follow Many.
 *
 *	Camera 2D Follow Many is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Camera 2D Follow Many is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Camera 2D Follow Many. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;


namespace CSGameUtils {

/// <summary>
/// Add this component to any GO that must be follow by the camera.
/// 
/// It will register himself in the Camera2DFollowMany.
/// </summary>
public class CameraFollowElement : MonoBehaviour
{
	Camera2DFollowMany cameraFollow;

	void OnEnable()
	{
		cameraFollow = Camera.main.GetComponent<Camera2DFollowMany>();
		cameraFollow.AddPlayer(this.gameObject);
	}

	void OnDisable()
	{
		// Won't be followed if disabled.
		cameraFollow.RemovePlayer(this.gameObject);
	}
}
} // namespace CSGameUtils
