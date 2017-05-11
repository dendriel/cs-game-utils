/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Spriter Animator.
 *
 *	Spriter Animator is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Spriter Animator is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Spriter Animator. If not, see<http://www.gnu.org/licenses/>.
 */

namespace CSGameUtils
{
	/// <summary>
	/// Dummy UnityAnimator. Download SpriterDotNet resource to use with GenericAnimator.
	/// </summary>
	public class UnityAnimator
	{
		public struct CurrentAnimationSt
		{
			public string Name;
		}

		public CurrentAnimationSt CurrentAnimation;

		public int Progress;

		public float Speed;

		public void Play(string param)
		{
		}
	}
} // namespace CSGameUtils
