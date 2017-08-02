/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Platform 2D.
 *
 *	Platform 2D is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Platform 2D is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Platform 2D. If not, see<http://www.gnu.org/licenses/>.
 */

using UnityEngine;
using System.Collections.Generic;

namespace CSGameUtils
{
	/// <summary>
	/// Adds circular movement to 2D platforms.
	/// 
	/// You may want to add a trigger collider in order to the following to work:
	/// 
	/// It add passengers to a list and auto-update its position according to platform movement. This is necessary
	/// because Unity physics alone inserts a bounce effect while the player is being held up by the platform.
	/// </summary>
	public class CircularPlatform2D : MonoBehaviour
	{
		/// <summary>
		/// Platform speed.
		/// 
		/// This parameter defines how much the platform will move per second. If it is set 360, the platform
		/// will make a full tour in just one second.
		/// </summary>
		[SerializeField]
		float degreesPerSecond = 65.0f;

		/// <summary>
		/// Use vertical movement instead of horizontal?
		/// </summary>
		[SerializeField]
		bool isClockwise;

		/// <summary>
		/// Central point of the circular trajectory.
		/// </summary>
		[SerializeField]
		Transform center;

		/// <summary>
		/// Current destination
		/// </summary>
		Vector3 currDestn;

		/// <summary>
		/// Any transform that is being carried by this platform.
		/// </summary>
		List<Transform> passengers;

		// Use this for initialization
		protected virtual void Start()
		{

			passengers = new List<Transform> { };
			currDestn = transform.position - center.position;
		}

		// Update is called once per frame
		protected virtual void Update()
		{
			// From robertbu: http://answers.unity3d.com/questions/686785/multiple-gameobjects-moving-in-circular-path.html
			currDestn = Quaternion.AngleAxis((degreesPerSecond * ((isClockwise) ? -1 : 1)) * Time.deltaTime, Vector3.forward) * currDestn;
			Vector3 prevPos = transform.position;
			transform.position = center.position + currDestn;

			// Update passengers positions.
			Vector3 offset = transform.position - prevPos;
			foreach (Transform pass in passengers) {
				pass.position += offset;
			}
		}

		protected virtual void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.isTrigger) return;

			passengers.Add(collision.transform);
		}

		protected virtual void OnTriggerExit2D(Collider2D collision)
		{
			if (!collision.isTrigger) return;

			passengers.Remove(collision.transform);
		}
	}
}
