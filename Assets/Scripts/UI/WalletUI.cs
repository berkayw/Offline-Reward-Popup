using TMPro;
using UnityEngine;

/// <summary>
/// Wallet Panel:
/// - Displays Wallet totals (coins/hammers)
/// </summary>
public class WalletUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Wallet wallet;
    [SerializeField] private RewardService rewardService;
    [SerializeField] private GameObject walletPanel;
    
    [Header("UI")]
    [SerializeField] private TMP_Text walletText;

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

    public void WalletPanelStateHandler()
    {
        if (!walletPanel.activeSelf)
        {
            walletPanel.SetActive(true);
        }
        else
        {
            walletPanel.SetActive(false);
        }
    }

    #endregion
    

    #region Helpers

    private void RefreshUI(int coinAmount, int hammerAmount) //placeholder parameters
    {
        walletText.text = wallet.Coins + " coins" + " / " + wallet.Hammers + " hammers";
    }

    #endregion
}
