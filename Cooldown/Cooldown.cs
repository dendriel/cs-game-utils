/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 *
 *	Cooldown is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Cooldown is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Cooldown. If not, see<http://www.gnu.org/licenses/>.
 */
using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Handles cooldown data. This is an independent timer that uses "WaitForSeconds" for creating a cooldown.
///
/// The easiest way to use this is to instantiate a new Cooldown setting the time to wait in seconds "foo = new Cooldown(1)"
/// and triggering the cooldown passing a reference to a monobehavior "foo.Start(this)". Then wait it to end by checking
/// "foo.IsWaiting".
/// </summary>
[Serializable]
public class Cooldown
{
	/// <summary>
	/// The minimum time of cooldown.
	/// </summary>
	[SerializeField]
	float minimum = 1f;
	public float Minimum { get { return minimum; } }
	
	/// <summary>
	/// The maximum time of cooldown.
	/// </summary>
	[SerializeField]
	float maximum = 2f;
	public float Maximum { get { return maximum; } }

	/// <summary>
	/// Custom time for the cooldown;
	/// </summary>
	float customTimeToWait = 0f;

	/// <summary>
	/// Tells if cooldown is in progress.
	/// </summary>
	bool isWaiting;
	public bool IsWaiting { get { return isWaiting; } }

    /// <summary>
    /// Tells if the cool has ended.
    /// </summary>
    public bool HasEnded { get; private set; }

    /// <summary>
    /// The coroutine that runs the cooldown.
    /// </summary>
    Coroutine cooldownCoroutine;

	/// <summary>
	/// Initializes a new instance of the <see cref="Cooldown"/> class. Setting a "maximum" time to wait will
	/// random the time to wait between min and max.
	/// </summary>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public Cooldown(float min, float max)
	{
		DefaultConstructor(min, max);
    }

	/// <summary>
	/// Initializes a new instance of the <see cref="Cooldown"/> class.
	/// </summary>
	/// <param name="delay">The time to wait.</param>
	public Cooldown (float delay)
	{
		DefaultConstructor(delay, delay);
	}

	void DefaultConstructor(float min, float max)
	{
		minimum = min;
		maximum = max;
		isWaiting = false;
		HasEnded = false;
	}

	/// <summary>
	/// Calculates the cooldown value.
	/// </summary>
	/// <returns>The cooldown value.</returns>
	float TimeToWait()
	{
		if (customTimeToWait != 0) {
			float temp = customTimeToWait;
            customTimeToWait = 0f;
			return temp;
        }

		if (minimum == maximum) {
			return maximum;
		} else {
			return UnityEngine.Random.Range(minimum, maximum);
		}
	}

	/// <summary>
	/// Starts the cooldown. Will restart if already running.
	/// </summary>
	/// <param name="behavior">Behavior.</param>
	public void Start(MonoBehaviour behavior)
	{
        if (cooldownCoroutine != null) behavior.StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = behavior.StartCoroutine(WaitCooldown());
        HasEnded = false;
	}

	/// <summary>
	/// Wait a different amount of time once.
	/// </summary>
	/// <param name="behavior">Behavior</param>
	/// <param name="customTime">The custom time to wait (this value is used only once).</param>
	public void Start(MonoBehaviour behavior, float customTime)
	{
		customTimeToWait = customTime;
		Start(behavior);
    }

    /// <summary>
    /// Interrupts the cooldown.
    /// </summary>
    public void Stop(MonoBehaviour behavior)
    {
        if (cooldownCoroutine != null) behavior.StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = null;
        isWaiting = false;
        HasEnded = false;
    }

	/// <summary>
	/// Waits the cooldown.
	/// </summary>
	/// <returns>The cooldown.</returns>
	IEnumerator WaitCooldown()
	{
		isWaiting = true;
		yield return new WaitForSeconds(TimeToWait());
		isWaiting = false;
        HasEnded = true;
    }
}