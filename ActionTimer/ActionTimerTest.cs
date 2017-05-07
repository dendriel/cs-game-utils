/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Action Timer.
 *
 *	Action Timer is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Action Timer is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Action Timer. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;

namespace CSGameUtils
{
	/// <summary>
	/// Testing class for action timer.
	/// </summary>
	class ActionTimerTest : MonoBehaviour
	{
		/// <summary>
		/// Testing struct to be passed as parameter inside ActionTimer.
		/// </summary>
		public struct MyData
		{
			public int a;
			public string b;
			public float c;
			public char d;

			public MyData(int _a, string _b, float _c, char _d)
			{
				a = _a;
				b = _b;
				c = _c;
				d = _d;
			}
		}

		ActionTimer<MyData> timerMyData;
		ActionTimer<float> timer;

		void Start()
		{
			float timeToWaitInSec = 2;
			float valueToBePrinted = 123456;

			// Create an ActionTimer with a primitive type.
			timer = new ActionTimer<float>(timeToWaitInSec, Test, valueToBePrinted);
			timer.Start(this);

			timerMyData = new ActionTimer<MyData>(3, 6, PrintMyData, new MyData(10, "teste", 654321, 'V'));
		}

		public void Test(float value)
		{
			Debug.Log("Callback called for primitive type! Value: " + value);
			Debug.Log("Triggering the next callback...");
			timerMyData.Start(this);
		}

		public static void PrintMyData(MyData data)
		{
			Debug.Log("Printing MyData: " + data.a + " - " + data.b + " - " + data.c + " - " + data.d);
		}
	}
} // namespace CSGameUtils
