using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// DevelopmentScene controller:
/// - Reads simulated time from DevTimer
/// - Calculates rewards (floor by minute)
/// - Publishes calculated values via events.
/// - Handles Collect using cached values
/// 
/// Loot button is intentionally a no-op (placeholder).
/// </summary>
public class RewardService : MonoBehaviour
{
    public UnityEvent<int,int> ResourcesCollected;    // Fired when the player collects rewards.
    
    [Header("References")]
    [SerializeField] private OfflineRewardConfig config;
    [SerializeField] private Timer timer;
    [SerializeField] private OfflineRewardPopupUI rewardPopupUI;
    [SerializeField] private Wallet wallet;
    
    private int _lastRenderedSecond = -1;
    
    //Cached reward data
    private double _cachedSeconds;
    private int _cachedCoins;
    private int _cachedHammers;
    private bool _cachedCanCollect;

    private void Update()
    {
        //each 1 sec.
        int currentSecond = Mathf.FloorToInt((float)timer.Seconds);
        if (currentSecond == _lastRenderedSecond) return;

        _lastRenderedSecond = currentSecond;

        Calculate();
        HandleUIRefresh();
    }

    #region Calculation
    
    //Calculates reward values based on current dev timer and stores them in cache.
    private void Calculate()
    {
        _cachedSeconds = timer.Seconds;

        int earnedMinutes = Mathf.FloorToInt((float)(_cachedSeconds / 60.0));

        _cachedCoins = earnedMinutes * config.coinPerMinute;
        _cachedHammers = earnedMinutes * config.hammerPerMinute;

        bool hasAnyReward = _cachedCoins > 0 || _cachedHammers > 0;
        _cachedCanCollect = hasAnyReward && earnedMinutes >= config.minCollectMinutes;
    }

    #endregion
    
    #region UI
    
    // Refreshes popup UI using cached reward values.
    private void HandleUIRefresh()
    {
        float minuteProgress01 = (float)((_cachedSeconds % 60.0) / 60.0);

        rewardPopupUI.RenderRatios(config.coinPerMinute, config.hammerPerMinute);
        rewardPopupUI.RenderDuration(_cachedSeconds);
        rewardPopupUI.RenderAmounts(_cachedCoins, _cachedHammers);
        rewardPopupUI.RenderBars(
            minuteProgress01,
            minuteProgress01,
            config.barMinWidth,
            config.barMaxWidth
        );

        rewardPopupUI.SetCollectInteractable(_cachedCanCollect);
    }

    #endregion
    
    #region Button Callbacks

    /// <summary>
    /// Collect button: grants calculated resources and resets the dev timer (for quick retesting).
    /// </summary>
    public void Collect()
    {
        if (!_cachedCanCollect) return;
        
        wallet.AddCoins(_cachedCoins); 
        wallet.AddHammers(_cachedHammers);

        ResourcesCollected?.Invoke(_cachedCoins, _cachedHammers);

        ResetAfterCollect();
    }

    /// <summary>
    /// Loot button: placeholder in this case version.
    /// </summary>
    public void Loot()
    {
        Debug.Log("[OfflineReward] Loot is placeholder.");
    }

    #endregion

    #region Helpers
    
    private void ResetAfterCollect()
    {
        timer.ResetToZero();

        _cachedCoins = 0;
        _cachedHammers = 0;
        _cachedCanCollect = false;
        
        // Force refresh next frame.
        _lastRenderedSecond = -1;
    }

    #endregion
    
    
    
}

