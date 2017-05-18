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
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Handles in game texts.
	/// 
	/// This is kind of a "text factory". Create a GameObject and assign TextManager script to it. Then
	/// create child game objects and setup the TextEFX in then (you may configure TextEFX from inspector).
	/// </summary>
	public class TextManager : MonoBehaviour
	{
		/// <summary>
		/// Use this as an "unique" object identifier, as won't be allowed another ObjectManager
		/// instance in the game.
		/// </summary>
		public static TextManager Inst = null;

		void Awake()
		{
			// If first awake.
			if (Inst == null) {
				Inst = this;
			} else if (Inst != this) { // Subsequent awakes. Object already exist.
				Destroy(gameObject);
			}

			// Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
		}

//*************************************************************************************************
	
		/// <summary>
		/// Available texts list.
		/// </summary>
		GameObject[] textsList;
	

		// Use this for initialization
		void Start()
		{
			// Store in-game texts reference. Do this only once in all the running time (because these
			// are children and doesn't get lost when loading a new scene).
			textsList = new GameObject[transform.childCount];
			for (int i = 0; i < transform.childCount; i++) {
				textsList[i] = transform.GetChild(i).gameObject;
			}
		}

		/// <summary>
		/// Find an inactive text.
		/// </summary>
		/// <returns>The inactive text. Null if there is no inactive text.</returns>
		GameObject GetText()
		{
			for (int i = 0; i < textsList.Length; i++) {
				if (!textsList[i].gameObject.activeSelf) {
					return textsList[i];
				}
			}
			Debug.Log("WARNING: There is no enough text to be displayed in the game.");
			return null;
		}

		/// <summary>
		/// Displays a experience text over the target character.
		/// </summary>
		/// <param name="target">The transform of the character.</param>
		/// <param name="damageValue">The damage value to be displayed.</param>
		public void DisplayReceiveExpText(Transform target, int exp)
		{
			GameObject textGO = GetText();
			if (textGO == null) return;

			TextEFX textEFX = textGO.GetComponent<TextEFX>();
			Assert.IsNotNull<TextEFX>(textEFX, "Could not find the TextEFX component in the text GO.");

			textEFX.TargetPos = target.position;
			textEFX.DisplayText = "+" + exp + " Exp" ;
			textEFX.SetupReceiveExpColorTrans();
			textEFX.enabled = true;
			textGO.SetActive(true);
		}

		/// <summary>
		/// Displays a hit text over the target character.
		/// </summary>
		/// <param name="target">The transform of the character.</param>
		/// <param name="damageValue">The damage value to be displayed.</param>
		public void DisplayHitText(Transform target, int damageValue)
		{
			GameObject textGO = GetText();
			if (textGO == null) return;

			TextEFX textEFX = textGO.GetComponent<TextEFX>();
			Assert.IsNotNull<TextEFX>(textEFX, "Could not find the TextEFX component in the text GO.");

			textEFX.TargetPos = target.position;

			if (damageValue > 0) {
				textEFX.DisplayText = damageValue.ToString ();
			} else {
				textEFX.DisplayText = "Block!";
			}
			textEFX.SetupTakeHitColorTrans();
			textEFX.enabled = true;
			textGO.SetActive(true);
		}

		/// <summary>
		/// Displays a hit text over the target character.
		/// </summary>
		/// <param name="target">The transform of the character.</param>
		/// <param name="name">Name of the item picked.</param>
		/// <param name="amount">Amount of gold picked.</param>
		/// <param name="isValuable">Is picking a valuable?</param>
		/// <param name="infinite">The picked item is infinite?</param>
		public void DisplayPickupText(Transform target, string name, int amount = 0, bool isValuable = false, bool infinite = false)
		{
			GameObject textGO = GetText();
			if (textGO == null) return;

			TextEFX textEFX = textGO.GetComponent<TextEFX>();
			Assert.IsNotNull<TextEFX>(textEFX, "Could not find the TextEFX component in the text GO.");

			textEFX.TargetPos = target.position;
			textEFX.DisplayText = "+";
			textEFX.DisplayText += (infinite)? "" : amount.ToString() + " ";
			textEFX.DisplayText += name + ((amount > 1)? "s" : "");

			if (isValuable) {
				textEFX.SetupPickupGoldColorTrans();
			} else {
				textEFX.SetupPickupItemColorTrans();
			}

			textEFX.enabled = true;
			textGO.SetActive(true);
		}
	}
} // namespace CSGameUtils
