using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Simple Development Debug Panel:
/// - Displays Wallet totals (coins/hammers) and Speed multiplier
/// - Allows speeding up / slowing down DevTimer with +/- keys
/// </summary>
public class DevDebugUI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private DevelopmentTimer devTimer;
    [SerializeField] private Wallet wallet;
    [SerializeField] private RewardService rewardService;
    [SerializeField] private GameObject debugPanel;
    
    [Header("UI")]
    [SerializeField] private TMP_Text walletText;
    [SerializeField] private TMP_Text speedText;

    [Tooltip("How much to change speed per key press.")]
    [Min(1f)]
    [SerializeField] private float speedStep = 2f;

    [Tooltip("Minimum allowed speed multiplier.")]
    [Min(1f)]
    [SerializeField] private float minSpeed = 1f;

    [Tooltip("Maximum allowed speed multiplier.")]
    [Min(1f)]
    [SerializeField] private float maxSpeed = 32f;

    private float _refreshTimer;

    
    private void Start()
    {
        RefreshUI(0,0); //placeholder parameters
    }

    private void OnEnable()
    {
        rewardService.ResourcesCollected.AddListener(RefreshUI);
    }
    private void OnDisable()
    {
        rewardService.ResourcesCollected.AddListener(RefreshUI);
    }
    
    #region Button Callbacks

    public void IncreaseSpeed()
    {
        SetSpeed(devTimer.speedMultiplier * speedStep);
    }
    
    public void DecreaseSpeed()
    {
        SetSpeed(devTimer.speedMultiplier / speedStep);
    }
    
    public void DebugPanelStateHandler()
    {
        if (!debugPanel.activeSelf)
        {
            debugPanel.SetActive(true);
        }
        else
        {
            debugPanel.SetActive(false);
        }
    }

    #endregion
    

    #region Helpers

    private void SetSpeed(float value)
    {
        devTimer.speedMultiplier = Mathf.Clamp(value, minSpeed, maxSpeed);
        RefreshUI(wallet.Coins, wallet.Hammers); 
    }
    
    private void RefreshUI(int coinAmount, int hammerAmount) //placeholder parameters
    {
        walletText.text = wallet.Coins + " coins" + " / " + wallet.Hammers + " hammers";
        speedText.text = $"{devTimer.speedMultiplier:0.##}x";
    }

    #endregion
}
