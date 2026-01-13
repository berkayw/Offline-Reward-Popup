using TMPro;
using UnityEngine;

/// <summary>
/// Minimal wallet for the case. Stores resources and allows adding amounts.
/// </summary>
public class Wallet : MonoBehaviour
{
    public int Coins { get; private set; }
    public int Hammers { get; private set; }

    #region Public API

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coins += amount;
    }

    public void AddHammers(int amount)
    {
        if (amount <= 0) return;
        Hammers += amount;
    }

    #endregion
    
}
