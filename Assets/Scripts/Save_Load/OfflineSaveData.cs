using System;

/// <summary>
/// Serializable data container.
/// Stores last collect time and wallet values.
/// </summary>

[Serializable]
public class OfflineSaveData
{
    public long lastCollectUtcSeconds;
    
    public int coins;
    public int hammers;
}