/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	Camera Parallax is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Camera Parallax is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Camera Parallax. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;
using System;

namespace CSGameUtils
{
	/// <summary>
	/// Provides the parallax (or "moving backgrounds") feature.
	/// </summary>
	public class CameraParallax : MonoBehaviour
	{
		/// <summary>
		/// Enable/disable the parallax.
		/// </summary>
		[SerializeField]
		public bool Enabled = true;

		[Serializable]
		public class ParallaxElement
		{
			public Transform Trans;
			public Vector2 OffsetSpeed;
			public bool reverseX;
			public bool reverseY;
		}

		/// <summary>
		/// Elements to be affected by the parallax.
		/// </summary>
		[SerializeField]
		ParallaxElement[] elements;

		Vector3 campPrevPos;
	
		// Use this for initialization
		void Start ()
		{
			if (Enabled == false) return;
		
			campPrevPos = transform.position;

			for (int i = 0; i < elements.Length; i++) {
				SetStartingElementPos(elements[i]);
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Enabled == false) return;
        
			for (int i = 0; i < elements.Length; i++) {
				UpdateElementPos(elements[i]);
			}

			campPrevPos = transform.position;
		}

		/// <summary>
		/// Update the element position acording to Parallax.
		/// </summary>
		/// <param name="elem"></param>
		void UpdateElementPos(ParallaxElement elem)
		{
			if (elem.OffsetSpeed == Vector2.zero) return;

			Vector3 mvmDist = transform.position - campPrevPos;

			if (!elem.reverseX) {
				mvmDist.x *= -1;
			}
			if (!elem.reverseY) {
				mvmDist.y *= -1;
			}

			Vector3 newElemPos = elem.Trans.position + (new Vector3(mvmDist.x * elem.OffsetSpeed.x, mvmDist.y * elem.OffsetSpeed.y, elem.Trans.position.z));

			float journeyLength = Vector3.Distance(elem.Trans.position, newElemPos);
			float distanceCovered = elem.OffsetSpeed.x; // speed
			float fracJourney = distanceCovered / journeyLength;
			elem.Trans.position = Vector3.Lerp(elem.Trans.position, newElemPos, fracJourney);
		}

		/// <summary>
		/// Set the initial element position acording to Parallax.
		/// </summary>
		/// <param name="elem">The element to be set.</param>
		void SetStartingElementPos(ParallaxElement elem)
		{
			// TODO: this inst working as planned. The offset is being inversed somehow..
			Vector3 mvmDist = GameObject.FindGameObjectWithTag("PlayerStartingPos").transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;

			if (!elem.reverseX) {
				mvmDist.x *= -1;
			}
			if (!elem.reverseY) {
				mvmDist.y *= -1;
			}

			Vector3 newElemPos = elem.Trans.position + (new Vector3(mvmDist.x * elem.OffsetSpeed.x, mvmDist.y * elem.OffsetSpeed.y, elem.Trans.position.z));        
			elem.Trans.position = newElemPos;
		}
	}
} // namespace CSGameUtils
