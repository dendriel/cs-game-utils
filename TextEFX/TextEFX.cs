/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	Text Effect is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Text Effect is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Text Effect. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Displays a text effect over a character on enable.
	/// You may use TextManager to manage TextEFX. Create a GameObject and assign TextManager script to it. Then
	/// create child game objects and setup the TextEFX in then (you may configure TextEFX from inspector).
	/// </summary>
	[RequireComponent(typeof(TextMesh))]
	public class TextEFX : MonoBehaviour
	{
		/// <summary>
		/// Starting effect color.
		/// </summary>
		public Color StartColor = Color.red;

		/// <summary>
		/// Offset from the middle of the character (so the text appear above the character).
		/// </summary>
		public Vector3 TextOffset = new Vector3(0, 1f);

		/// <summary>
		/// Distance to cover before desappearing.
		/// </summary>
		public Vector3 DistanceToCover = new Vector3(0, 0.5f);
		

		/// <summary>
		/// Speed of the text animation (Lerp fraction jorney).
		/// </summary>
		public float TextSpeed = 0.05f;

		/// <summary>
		/// Target position (character).
		/// </summary>
		[HideInInspector]
		public Vector3 TargetPos;

		/// <summary>
		/// How much time the text should be displayed.
		/// </summary>
		public float DecayTime = 1.5f;

		/// <summary>
		/// Text to be displayed.
		/// </summary>
		public string DisplayText;
		
		/// <summary>
		/// Text Sorting layer.
		/// </summary>
		public string TextSortingLayer;
		
		/// <summary>
		/// Text Layer order.
		/// </summary>
		public int TextOrderInLayer;

		// Use this for initialization
		void Start()
		{
			// Setup mesh renderer sorting layer and order.
			MeshRenderer renderer = GetComponent<MeshRenderer>();
			renderer.sortingLayerName = TextSortingLayer;
			renderer.sortingOrder = TextOrderInLayer;
		}
		/// <summary>
		/// Text renderer.
		/// </summary>
		TextMesh text;

		/// <summary>
		/// Step for changing color from red to yellow.
		/// </summary>
		float totalDist;

		/// <summary>
		/// Final text position.
		/// </summary>
		Vector3 finalPos;

		ActionTimer<bool> DisableTextAction;

		void OnEnable()
		{
			if (text == null) {
				text = GetComponent<TextMesh>();
			}

			// First setup when enable. So the text doesn't come flying from "nowhere" to the character.        
			transform.position = TargetPos + TextOffset;
			finalPos = TargetPos + TextOffset + DistanceToCover;

			text.text = DisplayText;
			text.color = StartColor;

			totalDist = Vector3.Distance(transform.position, finalPos);

			// Setup disable action.
			DisableTextAction = new ActionTimer<bool>(DecayTime, DecayTime, gameObject.SetActive, false);
			DisableTextAction.Start(this);
		}

		void FixedUpdate()
		{
			// Update position.
			float journeyLength = Vector3.Distance(transform.position, finalPos);
			float distanceCovered = TextSpeed; // speed
			float fracJourney = distanceCovered / journeyLength;
        
			transform.position = Vector3.Lerp(transform.position, finalPos, fracJourney);

			UpdateTextColors();

			// Check if finished. (because we set a destination above, we may check the y axis.
			/*if (transform.position.y >= finalPos.y) {
				enabled = false;
				gameObject.SetActive(false);
			}*/
		}
       
		void UpdateTextColors()
		{
			float currDist = Vector3.Distance(transform.position, finalPos);
			float newColor = 1 - (currDist / totalDist);
        
			Color c = text.color;

			if (updateRedColor) {
				c.r = ((incRedColor) ? newColor  : 1 - newColor);
			}

			if (updateGreenColor) {
				if ((incGreenColor && (text.color.g < greenTargetValue) ) ||
				   (!incGreenColor && (text.color.g > greenTargetValue) )) {
					c.g = ((incGreenColor) ? newColor : 1 - newColor);
				}
			}

			if (updateBlueColor) {
				c.b = ((incBlueColor) ? newColor : 1 - newColor);
			}

			text.color = c;
		}

		bool updateRedColor = false;
		bool updateGreenColor = false;
		bool updateBlueColor = false;

		bool incRedColor = false;
		bool incGreenColor = false;
		bool incBlueColor = false;
    
		float greenTargetValue = 0f;

		/// <summary>
		/// Setup text transition colors.
		/// </summary>
		public void SetupPickupItemColorTrans()
		{
			StartColor = Color.green;

			// None.
			updateRedColor = false;
			updateGreenColor = false;
			updateBlueColor = false;
		}

		/// <summary>
		/// Setup text transition colors.
		/// </summary>
		public void SetupPickupGoldColorTrans()
		{
			StartColor = Color.yellow;

			// None.
			updateRedColor = false;
			updateGreenColor = false;
			updateBlueColor = false;
		}

		/// <summary>
		/// Setup text transition colors.
		/// </summary>
		public void SetupTakeHitColorTrans()
		{
			StartColor = Color.red;

			// Red to yellow.
			updateRedColor = false;
			updateGreenColor = true;
			updateBlueColor = false;

			incGreenColor = true;
			greenTargetValue = 1f;
		}

		/// <summary>
		/// Setup text transition colors.
		/// </summary>
		public void SetupReceiveExpColorTrans()
		{
			StartColor = Color.white;

			// White to yellow
			updateRedColor = false;
			updateGreenColor = false;
			updateBlueColor = true;

			incBlueColor = false;
		}

		/// <summary>
		/// Setup text transition colors.
		/// </summary>
		public void SetupEmotionTextColorTrans()
		{
			StartColor = Color.red;
			
			updateRedColor = false;
			updateGreenColor = false;
			updateBlueColor = false;
		}
	}
} // namespace CSGameUtils
