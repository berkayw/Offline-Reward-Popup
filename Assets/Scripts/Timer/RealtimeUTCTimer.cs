using System;
using UnityEngine;

/// <summary>
/// Realtime UTC-based timer for offline duration.
/// - Stores last collect UTC timestamp in a JSON file (persistentDataPath)
/// - Updates Seconds as state (same pattern as DevTimer)
/// </summary>
public class RealtimeUTCTimer : Timer
{
    public override double Seconds { get; protected set; }

    private void Awake()
    {
        SaveLoadManager.LoadOrInitialize();
        RecalculateSeconds();
    }

    private void Update()
    {
        RecalculateSeconds();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            RecalculateSeconds();
    }

    private void OnApplicationPause(bool paused)
    {
        if (!paused)
            RecalculateSeconds();
    }

    public override void ResetToZero()
    {
        SaveLoadManager.Data.lastCollectUtcSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        SaveLoadManager.Save();

        Seconds = 0;
    }

    private void RecalculateSeconds()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long diff = now - SaveLoadManager.Data.lastCollectUtcSeconds;

        if (diff < 0)
            diff = 0;

        Seconds = diff;
    }
    

}
