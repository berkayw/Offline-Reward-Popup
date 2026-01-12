using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This script gives UI buttons a "pressed" feel by temporarily reducing their height.
/// It is designed for layout-independent buttons (i.e., the parent is NOT controlling size via Layout Groups).
/// 
/// Use case:
/// - Works with Sprite Swap (Normal/Pressed sprites).
/// - The pressed sprite may look visually shorter, and this script makes the actual button height match that feeling.
/// 
/// Important!
/// - The button's RectTransform must be bottom-aligned. This ensures that when the height is reduced, the button shrinks from the TOP,
/// preserving its bottom alignment and creating a natural "pressed down" effect.
/// </summary>

[RequireComponent(typeof(RectTransform))]
public class ButtonPressResize : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [Header("Press Settings")]
    [Tooltip("How many pixels to reduce the height when pressed.")]
    [SerializeField] private float pressedHeightDelta = 8f;

    private RectTransform _rect;
    private Vector2 _initialSize;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _initialSize = _rect.sizeDelta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetPressed(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetPressed(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetPressed(false);
    }

    private void SetPressed(bool pressed)
    {
        var size = _initialSize;

        if (pressed)
            size.y = Mathf.Max(0f, size.y - pressedHeightDelta);

        _rect.sizeDelta = size;
    }
}