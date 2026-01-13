using UnityEngine;

/// <summary>
/// Development-only timer that simulates "offline duration".
/// Starts from a fixed value (default: 15 minutes) and increases every second.
/// </summary>
public class DevelopmentTimer : Timer
{
    [Header("Simulation")]
    [Tooltip("Start duration in seconds. Default is 15 minutes (900 seconds).")]
    [Min(0)] public int startSeconds = 15 * 60;
    
    [Tooltip("Time speed multiplier. 1 = real-time, 10 = 10x faster for testing.")]
    [Min(0.1f)] public float speedMultiplier = 1f;
    
    public override double Seconds { get; protected set; }
    
    private void Awake()
    {
        Seconds = startSeconds;
    }

    private void Update()
    {
        Seconds += Time.deltaTime * speedMultiplier;
        if (Seconds < 0) Seconds = 0;
    }

    #region Public API

    public override  void ResetToZero()
    {
        Seconds = 0;
    }

    #endregion
}
