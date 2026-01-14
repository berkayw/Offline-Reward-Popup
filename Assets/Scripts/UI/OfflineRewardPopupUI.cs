using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI-only script.
/// Listens to RewardService state updates and refreshes visuals.
/// No game logic or reward calculations should live here.
/// </summary>
public class OfflineRewardPopupUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text offlineDurationText;
    [SerializeField] private TMP_Text coinAmountText;
    [SerializeField] private TMP_Text hammerAmountText;
    [SerializeField] private TMP_Text coinRatioText;
    [SerializeField] private TMP_Text hammerRatioText;
    
    [Header("Bars (Fill RectTransforms)")]
    [Tooltip("Fill rect for the coin bar. Width is set based on progress (0..1).")]
    [SerializeField] private RectTransform coinFill;
    
    [Tooltip("Fill rect for the hammer bar. Width is set based on progress (0..1).")]
    [SerializeField] private RectTransform hammerFill;

    [Header("Buttons")]
    [SerializeField] private Button collectButton;
    [SerializeField] private Button lootButton;
    
    //buttons' background image to change color when disabled/enabled.
    private Image collectButtonImage;
    private Image lootButtonImage;  
    
    [Header("Button Colors")]
    [SerializeField] private Color defaultDisabledColor = Color.white; //inspector
    private Color collectDefaultColor;
    private Color lootDefaultColor;
    
    private void Awake()
    {
        collectButtonImage = collectButton.GetComponent<Image>();
        lootButtonImage = lootButton.GetComponent<Image>();

        collectDefaultColor = collectButtonImage.color;
        lootDefaultColor = lootButtonImage.color;
    }

    #region Public API

    public void SetCollectInteractable(bool value)
    {
        collectButton.interactable = value;
        collectButtonImage.color = value ? collectDefaultColor : defaultDisabledColor;
    }

    public void SetLootInteractable(bool value)
    {
        lootButton.interactable = value;
        lootButtonImage.color = value ? lootDefaultColor : defaultDisabledColor;
    }

    public void RenderRatios(int coinRatio, int hammerRatio)
    {
        coinRatioText.text = coinRatio + "/m";
        hammerRatioText.text = hammerRatio + "/m";
    }
    
    public void RenderDuration(double totalSeconds)
    {
        offlineDurationText.text = FormatDuration(totalSeconds);
    }

    public void RenderAmounts(int coins, int hammers)
    {
        coinAmountText.text = coins.ToString();
        hammerAmountText.text = hammers.ToString();
    }

    public void RenderBars(float coinProgress01, float hammerProgress01, float minWidth, float maxWidth)
    {
        SetFillWidth(coinFill, coinProgress01, minWidth, maxWidth);
        SetFillWidth(hammerFill, hammerProgress01, minWidth, maxWidth);
    }

    #endregion

    #region Helpers

    private static void SetFillWidth(RectTransform fill, float progress01, float minWidth, float maxWidth)
    {
        progress01 = Mathf.Clamp01(progress01);

        // Map 0..1 -> minWidth..maxWidth
        float width = Mathf.Lerp(minWidth, maxWidth, progress01);

        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private static string FormatDuration(double totalSeconds)
    {
        if (totalSeconds < 0) totalSeconds = 0;

        int seconds = Mathf.FloorToInt((float)totalSeconds);
        int minutes = seconds / 60;
        int sec = seconds % 60;

        int hours = minutes / 60;
        int min = minutes % 60;

        if (hours > 0) return hours + "h " + min + "m " + sec + "s";
        if (minutes > 0) return min + "m " + sec + "s";
        return sec + "s";
    }

    #endregion
}

