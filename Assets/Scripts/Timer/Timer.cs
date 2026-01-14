using UnityEngine;

/// <summary>
/// Base class for "offline duration" timers.
/// RewardService reads Seconds from here, without caring if it's Dev or Realtime.
/// </summary>
public abstract class Timer : MonoBehaviour
{
    /// <summary> Current offline duration in seconds. </summary>
    public abstract double Seconds { get; protected set;}

    /// <summary> Resets the tracked duration to zero (used after Collect). </summary>
    public abstract void ConsumeMinutes(int minutes);
}