using System;

/// <summary>
/// Realtime UTC-based timer for offline duration.
/// - Handles last collect UTC timestamp in the JSON file.
/// - Calculates offline duration using real UTC time.
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
    
    #region Public API
    
    public override void ResetToZero()
    {
        SaveLoadManager.Data.lastCollectUtcSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        SaveLoadManager.Save();

        Seconds = 0;
    }
    
    #endregion
    
    private void RecalculateSeconds()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long diff = now - SaveLoadManager.Data.lastCollectUtcSeconds;

        if (diff < 0)
            diff = 0;

        Seconds = diff;
    }
    

}
