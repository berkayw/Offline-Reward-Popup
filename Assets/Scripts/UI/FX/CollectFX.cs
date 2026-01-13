using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

/// <summary>
/// Collect burst FX:
/// 1) Spawn around SpawnPoint (spread X/Y)
/// 2) Fall straight DOWN to Landing Y while scaling up to a random target scale
/// 3) After landing, do 2-3 bounces and spread horizontally:
///    - direction + amount depend on how far from center the icon spawned (edge = more spread)
///    - never bounces to the opposite side
/// </summary>
public class OfflineCollectBurstFX : MonoBehaviour
{
    #region Inspector - Tweak

    [Header("References")]
    [SerializeField] private RectTransform fxRoot;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private RectTransform landingPoint;
    [SerializeField] private RectTransform coinPrefab;
    [SerializeField] private RectTransform hammerPrefab;

    [Header("Counts")]
    [Tooltip("How many coin icons to spawn.")]
    [SerializeField] private int coinCount = 15;
    
    [Tooltip("How many hammer icons to spawn.")]
    [SerializeField] private int hammerCount = 15;

    [Header("Spawn Spread")]
    [Tooltip("Random spread around spawn point (pixels).")]
    [SerializeField] private Vector2 spawnSpread = new Vector2(120f, 60f);

    [Header("Fall")]
    [Tooltip("Fall duration range (seconds).")]
    [SerializeField] private Vector2 fallDurationRange = new Vector2(0.4f, 0.7f);
    
    [Header("Delay")]
    [Tooltip("Random delay before each icon starts (seconds).")]
    [SerializeField] private Vector2 delayRange = new Vector2(0f, 0.2f);

    [Header("Scale")]
    [Tooltip("Start scale at spawn.")]
    [SerializeField] private float startScale = 0f;
    
    [Tooltip("Random target scale range during fall.")]
    [SerializeField] private Vector2 targetScaleRange = new Vector2(0.5f, 1f);

    [Header("Bounce")]
    [Tooltip("Random bounce count.")]
    [SerializeField] private Vector2Int bounceCountRange = new Vector2Int(2, 3);
    
    [Tooltip("Vertical bounce amount (pixels).")]
    [SerializeField] private float bounceHeight = 24f;
    
    [Tooltip("Bounce duration (seconds).")]
    [SerializeField] private float bounceDuration = 0.3f;
    [Range(0f, 0.8f)][SerializeField] private float bounceDecay = 0.35f;

    [Tooltip("How wide the spawn area is considered for edge-strength calculation (pixels). spawnSpread.x / 2 is fine.")]
    [SerializeField] private float edgeReferenceWidth = 60f;

    [Tooltip("Horizontal spread per bounce at CENTER (pixels).")]
    [SerializeField] private float bounceXCenter = 2f;

    [Tooltip("Horizontal spread per bounce at EDGES (pixels).")]
    [SerializeField] private float bounceXEdge = 30f;

    [Header("End")]
    [Tooltip("How long icons stay after bounce (seconds).")]
    [SerializeField] private Vector2 stayRange = new Vector2(0.1f, 0.3f);

    #endregion
    
    #region Public API / Collect Button Callback
    
    public void Play()
    {
        SpawnBatch(coinPrefab, coinCount);
        SpawnBatch(hammerPrefab, hammerCount);
    }

    #endregion
    
    #region Internals
    
    private void SpawnBatch(RectTransform prefab, int count)
    {
        for (int i = 0; i < count; i++)
            SpawnOne(prefab);
    }

    private void SpawnOne(RectTransform prefab)
    {
        RectTransform rt = Instantiate(prefab, fxRoot);
        rt.gameObject.SetActive(true);

        // 1) Spawn
        Vector2 spawn = RandomAround(spawnPoint.anchoredPosition, spawnSpread);

        // 2) Fall straight down (same X), landing Y fixed
        float landingY = landingPoint.anchoredPosition.y;
        Vector2 land = new Vector2(spawn.x, landingY);

        rt.anchoredPosition = spawn;
        rt.localScale = Vector3.one * startScale;

        float delay = Random.Range(delayRange.x, delayRange.y);
        float fallDur = Random.Range(fallDurationRange.x, fallDurationRange.y);
        float stay = Random.Range(stayRange.x, stayRange.y);
        float targetScale = Random.Range(targetScaleRange.x, targetScaleRange.y);

        // Determine direction + strength from spawn X (relative to center)
        float centerX = spawnPoint.anchoredPosition.x;
        float distanceFromCenter = spawn.x - centerX;

        int direction = distanceFromCenter >= 0f ? 1 : -1; // right side -> right, left side -> left (never opposite)

        float edge01 = Mathf.Clamp01(Mathf.Abs(distanceFromCenter) / edgeReferenceWidth); // 0=center, 1=edge

        // Per-bounce horizontal step (edge -> bigger step)
        float stepX = Mathf.Lerp(bounceXCenter, bounceXEdge, edge01);

        int bounces = Random.Range(bounceCountRange.x, bounceCountRange.y + 1);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay);

        // Fall + scale up (straight down)
        seq.Join(rt.DOAnchorPos(land, fallDur).SetEase(Ease.InQuad));
        seq.Join(rt.DOScale(targetScale, fallDur).SetEase(Ease.OutSine));

        // 3) Bounce + spread (Y returns to landingY each time)
        Vector2 current = land;

        for (int i = 0; i < bounces; i++)
        {
            float h = Mathf.Max(6f, bounceHeight * (1f - bounceDecay * i));

            // Each bounce moves further to its natural side
            float nextX = current.x + direction * stepX;

            Vector2 up = new Vector2((current.x + nextX) * 0.5f, landingY + h);
            Vector2 down = new Vector2(nextX, landingY);

            seq.Append(rt.DOAnchorPos(up, bounceDuration * 0.45f).SetEase(Ease.OutQuad));
            seq.Append(rt.DOAnchorPos(down, bounceDuration * 0.55f).SetEase(Ease.InQuad));

            current = down;
        }

        seq.AppendInterval(stay);
        seq.OnComplete(() => Destroy(rt.gameObject));
    }

    private static Vector2 RandomAround(Vector2 center, Vector2 spread)
    {
        return new Vector2(
            center.x + Random.Range(-spread.x * 0.5f, spread.x * 0.5f),
            center.y + Random.Range(-spread.y * 0.5f, spread.y * 0.5f)
        );
    }
    
    #endregion
}
