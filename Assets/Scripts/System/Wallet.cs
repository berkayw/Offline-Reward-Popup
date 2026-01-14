using UnityEngine;

/// <summary>
/// Minimal wallet for the case. Stores resources and allows adding amounts.
/// </summary>
public class Wallet : MonoBehaviour
{
    public int Coins { get; private set; }
    public int Hammers { get; private set; }

    public bool isDevelopmentVersion; //dont use save-load for development version

    private void Awake()
    {
        if(isDevelopmentVersion) return;

        SaveLoadManager.LoadOrInitialize();
        InitializeWalletFromSave();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;

        if(isDevelopmentVersion) return;

        SaveLoadManager.Data.coins = Coins;
        SaveLoadManager.Save();
        
    }

    public void AddHammers(int amount)
    {
        if (amount <= 0) return;
        Hammers += amount;

        if(isDevelopmentVersion) return;

        SaveLoadManager.Data.hammers = Hammers;
        SaveLoadManager.Save();
        
    }

    public void InitializeWalletFromSave()
    {
        Coins = SaveLoadManager.Data.coins;
        Hammers = SaveLoadManager.Data.hammers;
    }
}