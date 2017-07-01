/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Wasp Behavior.
 *
 *	Wasp Behavior is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Wasp Behavior is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Wasp Behavior. If not, see<http://www.gnu.org/licenses/>.
 */
using BehaviorLibrary.Components.Conditionals;
using System;
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Conditional to check if a point is inside the given (box) area.
	/// </summary>
	public class IsPointInBoxAreaConditional : Conditional
	{
		/// <summary>
		/// The target must be inside this area to be assigned.
		/// </summary>
		Vector3 minArea;

		/// <summary>
		/// The target must be inside this area to be assigned.
		/// </summary>
		Vector3 maxArea;

		/// <summary>
		/// A Func to retrieve the point.
		/// </summary>
		Func<Vector3> GetPoint;

		/// <summary>
		/// Create a new IsPointInBoxAreaConditional.
		/// </summary>
		/// <param name="_GetPoint">A Func to retrieve the current point to be tested</param>
		/// <param name="_minArea">Minimum limits in which the target must be to be assigned.</param>
		/// <param name="_maxArea">Maximum limits in which the target must be to be assigned.</param>
		public IsPointInBoxAreaConditional(Func<Vector3> _GetPoint, Vector3 _minArea, Vector3 _maxArea)
		{
			GetPoint = _GetPoint;
			minArea = _minArea;
			maxArea = _maxArea;

			_Bool = IsPointInBoxAreaTest;
		}

		bool IsPointInBoxAreaTest()
		{
			return PointInArea(GetPoint(), minArea, maxArea);
		}

		/// <summary>
		/// Checks if the given point is inside the given area (box).
		/// </summary>
		/// <param name="point">The point to be checked.</param>
		/// <param name="minArea">Min area bounds.</param>
		/// <param name="maxArea">Max area bounds.</param>
		/// <returns>true if the point is inside the area; false otherwise.</returns>
		public static bool PointInArea(Vector3 point, Vector3 minArea, Vector2 maxArea)
		{
			return ((point.x >= minArea.x) && (point.y >= minArea.y) &&
					(point.x <= maxArea.x) && (point.y <= maxArea.y));
		}
	}
} // namespace CSGameUtils