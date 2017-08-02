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
	/// Adds axis movement to 2D platforms.
	/// 
	/// You may want to add a trigger collider in order to the following to work:
	/// 
	/// It add passengers to a list and auto-update its position according to platform movement. This is necessary
	/// because Unity physics alone inserts a bounce effect while the player is being held up by the platform.
	/// </summary>
	public class AxisPlatform2D : MonoBehaviour
	{
		/// <summary>
		/// Platform speed.
		/// </summary>
		[SerializeField]
		float speed = 1;

		/// <summary>
		/// Delay when it arrives at a destination.
		/// </summary>
		[SerializeField]
		float delayInSec = 1;

		/// <summary>
		/// Use vertical movement instead of horizontal?
		/// </summary>
		[SerializeField]
		bool isVertical;

		/// <summary>
		/// Starting destn.
		/// </summary>
		[SerializeField]
		Transform pointA;

		/// <summary>
		/// Last destn.
		/// </summary>
		[SerializeField]
		Transform pointB;

		/// <summary>
		/// Current destination
		/// </summary>
		Vector3 currDestn;

		/// <summary>
		/// Wait some time after arriving at a destination.
		/// </summary>
		Cooldown waitSomeTimeDelay;

		/// <summary>
		/// Any transform that is being carried by this platform.
		/// </summary>
		List<Transform> passengers;

		// Use this for initialization
		void Start()
		{
			currDestn = pointA.position;
			waitSomeTimeDelay = new Cooldown(delayInSec);

			passengers = new List<Transform> { transform };
		}

		// Update is called once per frame
		void Update()
		{
			if (waitSomeTimeDelay.IsWaiting) return;

			// Check if arrived at the current destination.
			if (IsArrivedAtDestnConditional.IsArrivedAtDestination(currDestn, transform.position, 0.1f, isVertical)) {
				// Set the next destination.
				currDestn = (currDestn == pointA.position) ? pointB.position : pointA.position;
				// Wait some time before moving to the next position.
				waitSomeTimeDelay.Start(this);
				return;
			}

			// Calculate the movement to be added.
			Vector2 mvmtToAdd;
			if (isVertical) {
				mvmtToAdd = new Vector2(0, speed * Time.fixedDeltaTime * ((currDestn.y > transform.position.y) ? 1 : -1));
			} else {
				mvmtToAdd = new Vector2(speed * Time.fixedDeltaTime * ((currDestn.x > transform.position.x) ? 1 : -1), 0);
			}

			UpdatePassengersPos(mvmtToAdd);
		}

		/// <summary>
		/// Update the position of all passengers in the platform.
		/// </summary>
		/// <param name="mvmtToAdd">The movement to be added to platform positions.</param>
		void UpdatePassengersPos(Vector2 mvmtToAdd)
		{
			foreach (Transform pass in passengers) {
				Vector3 newPos = pass.localPosition;
				newPos.x += mvmtToAdd.x;
				newPos.y += mvmtToAdd.y;
				pass.localPosition = newPos;
			}
		}

		/// <summary>
		/// Check if the platform has arrived the current destination.
		/// </summary>
		/// <returns></returns>
		bool CheckArrived()
		{
			if (isVertical) {
				return (transform.position.y >= currDestn.y) || (transform.position.y <= currDestn.y);
			} else {
				return (transform.position.x >= currDestn.x) || (transform.position.x <= currDestn.x);
			}
		}


		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.isTrigger) return;

			passengers.Add(collision.transform);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (!collision.isTrigger) return;

			passengers.Remove(collision.transform);
		}
	}
}