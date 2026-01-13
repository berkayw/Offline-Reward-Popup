using UnityEngine;

/// <summary>
/// Configuration values for the Offline Reward popup (Development / UTC scenes).
/// Keeps editor-tweakable values in one place.
/// </summary>
[CreateAssetMenu(fileName = "OfflineRewardConfig", menuName = "Offline Reward/Config")]
public class OfflineRewardConfig : ScriptableObject
{
    #region Rates

    [Header("Rates (per minute)")]
    [Tooltip("Coins generated per minute of offline time.")]
    [Min(0)] public int coinPerMinute = 10;

    [Tooltip("Hammers generated per minute of offline time.")]
    [Min(0)] public int hammerPerMinute = 1;

    #endregion

    #region UI

    [Header("UI")]
    [Tooltip("Max width (in pixels) for the sliced fill bar when progress is 100%.")]
    [Min(1f)] public float barMaxWidth = 200f;

    [Tooltip("Min width (in pixels) for the fill bars when progress is 0%.")]
    [Min(0f)] public float barMinWidth = 40f;
    
    [Tooltip("Collect button becomes interactable only if earned minutes >= this value.")]
    [Min(0)] public int minCollectMinutes = 1;

    #endregion
}
