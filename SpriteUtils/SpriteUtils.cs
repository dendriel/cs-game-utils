/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	Sprite Utilities is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Sprite Utilities is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Sprite Utilities. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Sprite utilities.
	/// </summary>
	public static class SpriteUtils
	{
		/// <summary>
		/// Default color step for fading routines.
		/// </summary>
		public static Color DefaultColorStep { get { return new Color(0.05f, 0.05f, 0.05f); } }

		/// <summary>
		/// Default step for fading alpha routines.
		/// </summary>
		//public static float DefaultAlphaStep { get { return DefaultAlphaStepConst; } }
		public const float DefaultAlphaStep = 0.05f;

		/// <summary>
		/// Default step for fading transform routines.
		/// </summary>
		public const float DefaultSizeStep = 0.1f;

		/// <summary>
		/// Load a sprite from Resources.
		/// </summary>
		/// <param name="path">Path to the resource starting from after "Resources/".</param>
		/// <returns>The loaded resource.</returns>
		public static Sprite LoadSprite(string path)
		{
			Sprite loadedRes = Resources.Load<Sprite>(path);
			Assert.IsNotNull<Sprite>(loadedRes, "Couldn't find the resource to load: " + path);

			return loadedRes;
		}

		/// <summary>
		/// Load a sprite array from Resources.
		/// </summary>
		/// <param name="path">Path to the resource starting from after Resources/</param>
		/// <returns>The loaded resources.</returns>
		public static Sprite[] LoadSprites(string path)
		{
			Sprite[] loadedRes = Resources.LoadAll<Sprite>(path);
			Assert.IsNotNull<Sprite[]>(loadedRes, "Couldn't find the resources to load: " + path);

			return loadedRes;
		}

		/// <summary>
		/// Load a texture2D from Resources.
		/// </summary>
		/// <param name="path">Path to the resource starting from after Resources/</param>
		/// <returns>The loaded resource.</returns>
		public static Texture2D LoadTexture2D(string path)
		{
			Texture2D loadedRes = Resources.Load<Texture2D>(path);
			Assert.IsNotNull<Texture2D>(loadedRes, "Couldn't find the resource to load: " + path);

			return loadedRes;
		}

		/// <summary>
		/// Sets the image color alpha.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="a">The alpha component.</param>
		public static void SetImageColorAlpha(Image target, float newAlpha)
		{
			Color c = target.color;
			c.a = newAlpha;
			target.color = c;
		}

		/// <summary>
		/// Sets the image color alpha to max.
		/// </summary>
		/// <param name="target">Target.</param>
		public static void SetImageColorAlphaToMax(Image target)
		{
			Color c = target.color;
			c.a = 1f;
			target.color = c;
		}

		/// <summary>
		/// Sets the image color alpha to zero.
		/// </summary>
		/// <param name="target">Target.</param>
		public static void SetImageColorAlphaToZero(Image target)
		{
			Color c = target.color;
			c.a = 0f;
			target.color = c;
		}

		/// <summary>
		/// Sets the sprite renderer original color (1f, 1f, 1f, 1f).
		/// </summary>
		/// <param name="render"></param>
		public static void SetRendererOriginalColor(SpriteRenderer render)
		{
			render.color = new Color(1f, 1f, 1f);
		}

		/// <summary>
		/// Sets the renderer color alpha to zero.
		/// </summary>
		/// <param name="target">Target renderer.</param>
		public static void SetRendererColorAlphaToZero(SpriteRenderer render)
		{
			Color c = render.color;
			c.a = 0f;
			render.color = c;
		}
    
		/// <summary>
		/// Sets the renderer color alpha to maximum (1f).
		/// </summary>
		/// <param name="target">Target renderer.</param>
		public static void SetRendererColorAlphaToMax(SpriteRenderer render)
		{
			Color c = render.color;
			c.a = 1f;
			render.color = c;
		}

		/// <summary>
		/// Set the original scale of a transform (1,1,1).
		/// </summary>
		/// <param name="trans">The transform to be set.</param>
		public static void SetTransformOriginalScale(Transform trans)
		{
			trans.localScale = new Vector3(1, 1, 1);
		}

		/// <summary>
		/// Scale in the transform (increase the image scale).
		/// </summary>
		/// <param name="rect">The rect transform to scale.</param>
		/// <param name="step">The scale step. (DefaultSizeStep is cool)</param>
		/// <param name="size">The maximum size. (1f to fade until default size)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns></returns>
		public static IEnumerator FadeInScale(Transform rect, float step, float size, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				rect.localScale = new Vector3(Mathf.Min(size, rect.localScale.x + (step * Time.deltaTime)),
												Mathf.Min(size, rect.localScale.y + (step * Time.deltaTime)),
												rect.localScale.z);
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ((rect != null) && ((rect.localScale.x < size) || (rect.localScale.y < size)));

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Scale out the transform (decrease the image scale).
		/// </summary>
		/// <param name="rect">The rect transform to scale.</param>
		/// <param name="step">The scale step. (DefaultSizeStep is cool)</param>
		/// <param name="size">The minimum size. (0f to fade until the image is invisible)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns></returns>
		public static IEnumerator FadeOutScale(Transform rect, float step, float size, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				rect.localScale = new Vector3(Mathf.Max(size, rect.localScale.x - (step * Time.deltaTime)),
												Mathf.Max(size, rect.localScale.y - (step * Time.deltaTime)),
												rect.localScale.z);
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ((rect.localScale.x > size) || (rect.localScale.y > size));

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Scale in vertically the transform (increase the image scale).
		/// </summary>
		/// <param name="rect">The rect transform to scale.</param>
		/// <param name="step">The scale step. (DefaultSizeStep is cool)</param>
		/// <param name="size">The maximum size. (1f to fade until default size)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns></returns>
		public static IEnumerator FadeInScaleY(Transform rect, float step, float size, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				rect.localScale = new Vector3(rect.localScale.x,
					Mathf.Min(size, rect.localScale.y + (step * Time.deltaTime)),
					rect.localScale.z);
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ((rect != null) && (rect.localScale.y < size));

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Scale out vertically the transform (decrease the image scale).
		/// </summary>
		/// <param name="rect">The rect transform to scale.</param>
		/// <param name="step">The scale step. (DefaultSizeStep is cool)</param>
		/// <param name="size">The minimum size. (0f to fade until the image is invisible)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns></returns>
		public static IEnumerator FadeOutScaleY(Transform rect, float step, float size, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				rect.localScale = new Vector3(rect.localScale.x,
					Mathf.Max(size, rect.localScale.y - (step * Time.deltaTime)),
					rect.localScale.z);
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ((rect != null) && (rect.localScale.y > size));

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Fade the sprite renderer in. Increase its alpha until it is fully visible.
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. (DefaulAlphatStep is cool)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeInAlpha(SpriteRenderer render, float step, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = render.color;
				c.a = Mathf.Min(1, c.a + (step * Time.deltaTime));
				render.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while (render.color.a < 1f);

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Fade the sprite renderer in. Increase its alpha until it is fully visible.
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. (DefaulAlphatStep is cool)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeInAlpha(Image image, float step, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = image.color;
				c.a = Mathf.Min(1, c.a + (step * Time.deltaTime));
				image.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while (image.color.a < 1f);
        
			if (callback != null) {
				callback(value);
			}
		}
		/// <summary>
		/// Fade the sprite renderer out. Decrease its alpha until it is invisible.
		/// 
		/// Will stop if the image reference is set to null (may happen when the scene is reloaded).
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. (DefaulAlphatStep is cool)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeOutAlpha(SpriteRenderer render, float step, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = render.color;
				c.a = Mathf.Max(0, c.a - (step * Time.deltaTime));
				render.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while (render.color.a > 0f);

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Fade out the image alpha. Decrease its alpha until it is invisible.
		/// 
		/// WARNING: don't forget to execute within StartCoroutine().
		/// 
		/// Will stop if the image reference is set to null (may happen when the scene is reloaded).
		/// </summary>
		/// <param name="image">The image to fade.</param>
		/// <param name="step">The scale step. (DefaulAlphatStep is cool)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeOutAlpha(Image image, float step, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = image.color;
				c.a = Mathf.Max(0, c.a - (step * Time.deltaTime));
				image.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ((image != null) && (image.color.a > 0f));
        
			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Fade in the image color.
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. (DefaultColorStep is cool)</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeInImageColor(Image img, Color step, Action<float> callback = null, float value = 0)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = img.color;
				c.r = Mathf.Min(1, c.r + (step.r * Time.deltaTime));
				c.g = Mathf.Min(1, c.g + (step.g * Time.deltaTime));
				c.b = Mathf.Min(1, c.b + (step.b * Time.deltaTime));
				img.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ( // Take in account only the colors that are affected.
					((img.color.r < 1f) && (step.r != 0f)) ||
					((img.color.g < 1f) && (step.g != 0f)) ||
					((img.color.b < 1f) && (step.b != 0f)));

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Fade out the image color.
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. Use "DefaultColorStep" property if unsure (it's a good value).</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeOutImageColor(Image img, Color step)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = img.color;
				c.r = Mathf.Max(0, c.r - (step.r * Time.deltaTime));
				c.g = Mathf.Max(0, c.g - (step.g * Time.deltaTime));
				c.b = Mathf.Max(0, c.b - (step.b * Time.deltaTime));
				img.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ( // Take in account only the colors that are affected.
					((img.color.r > 0f) && (step.r != 0f)) ||
					((img.color.g > 0f) && (step.g != 0f)) ||
					((img.color.b > 0f) && (step.b != 0f)));
		}

		/// <summary>
		/// Fade in the SpriteRenderer color.
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. (DefaultColorStep is cool)</param>
		/// <param name="targetColor">Target color.</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeInSpriteRendererColor(SpriteRenderer render, Color step, Color? targetColor = null, Action<float> callback = null, float value = 0)
		{
			Color newC = targetColor ?? Color.white;

			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = render.color;
				if ((render.color.r < newC.r) && (step.r != 0f)) {
					c.r = Mathf.Min(1, c.r + (step.r * Time.deltaTime));
				}
				if ((render.color.g < newC.g) && (step.g != 0f)) {
					c.g = Mathf.Min(1, c.g + (step.g * Time.deltaTime));
				}
				if ((render.color.b < newC.b) && (step.b != 0f)) {
					c.b = Mathf.Min(1, c.b + (step.b * Time.deltaTime));
				}
				render.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ( // Take in account only the colors that are affected.
					((render.color.r < newC.r) && (step.r != 0f)) ||
					((render.color.g < newC.g) && (step.g != 0f)) ||
					((render.color.b < newC.b) && (step.b != 0f)));

			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Fade out the image color.
		/// </summary>
		/// <param name="render">The renderer to fade.</param>
		/// <param name="step">The scale step. Use "DefaultColorStep" property if unsure (it's a good value).</param>
		/// <param name="targetColor">Target color.</param>
		/// <param name="callback">Execute this callback (if any) when finished.</param>
		/// <param name="value">Value to be passed to the callback.</param>
		/// <returns>unused.</returns>
		public static IEnumerator FadeOutSpriteRendererColor(SpriteRenderer render, Color step, Color? targetColor = null, Action<float> callback = null, float value = 0)
		{
			Color newC = targetColor ?? Color.white;

			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Color c = render.color;

				if ((render.color.r > newC.r) && (step.r != 0f)) {
					c.r = Mathf.Max(0, c.r - (step.r * Time.deltaTime));
				}
				if ((render.color.g > newC.g) && (step.g != 0f)) {
					c.g = Mathf.Max(0, c.g - (step.g * Time.deltaTime));
				}
				if ((render.color.b > newC.b) && (step.b != 0f)) {
					c.b = Mathf.Max(0, c.b - (step.b * Time.deltaTime));
				}

				render.color = c;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while ( // Take in account only the colors that are affected.
					((render.color.r > newC.r) && (step.r != 0f)) ||
					((render.color.g > newC.g) && (step.g != 0f)) ||
					((render.color.b > newC.b) && (step.b != 0f)));
        
			if (callback != null) {
				callback(value);
			}
		}

		/// <summary>
		/// Moves a transform horizontally to the left.
		/// </summary>
		/// <param name="trans">The transform to move.</param>
		/// <param name="step">Step determines the speed. Try DefaultSizeStep.</param>
		/// <param name="destn">Y position in which the transform must get to.</param>
		/// <returns>unused.</returns>
		public static IEnumerator MoveHorTransformNegativeDestn(Transform trans, float step, float destn)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Vector3 curr = trans.position;
				curr.x = Mathf.Max(destn, curr.x - (step * Time.deltaTime));
				trans.position = curr;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while (trans.position.x > destn);
		}

		/// <summary>
		/// Moves a transform horizontally to the right.
		/// </summary>
		/// <param name="trans">The transform to move.</param>
		/// <param name="step">Step determines the speed. Try DefaultSizeStep.</param>
		/// <param name="destn">Y position in which the transform must get to.</param>
		/// <returns>unused.</returns>
		public static IEnumerator MoveHorTransformPositiveDestn(Transform trans, float step, float destn)
		{
			// smooth delay between steps.
			const float delay = 0.1f;
			do {
				Vector3 curr = trans.position;
				curr.x = Mathf.Min(destn, curr.x + (step * Time.deltaTime));
				trans.position = curr;
				yield return new WaitForSeconds(delay * Time.deltaTime);
			} while (trans.position.x < destn);
		}
	}
} // namespace CSGameUtils