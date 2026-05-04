using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LogicGame
{
    // Handles gem sprite display and the polish animation sequence:
    //   1. Circular cloth wipe over gem  →  2. Linear cloth sweep away  →  3. Shine rays
    public class GemDisplayController : MonoBehaviour
    {
        [Header("Gem")]
        [SerializeField] Image gemImage;
        [SerializeField] Sprite[] gemSprites;   // drag gem sprites in here after setup

        [Header("Cloth (assign your cloth Material to this Image in the Inspector)")]
        [SerializeField] Image clothOverlay;

        [Header("Shine rays (8 thin rectangles, auto-created by setup)")]
        [SerializeField] RectTransform[] shineRays;

        Coroutine activeAnim;

        void Awake() => ApplyRoundedCorners();

        // Generates a rounded-rectangle sprite and applies it to the cloth as a 9-sliced Image,
        // so the corners stay round at any stretched size.
        void ApplyRoundedCorners()
        {
            if (clothOverlay == null) return;

            const int W = 64, H = 40, R = 12;
            var tex = new Texture2D(W, H, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            var pixels = new Color32[W * H];
            var white  = new Color32(255, 255, 255, 255);
            var clear  = new Color32(0,   0,   0,   0);
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                    pixels[y * W + x] = RoundedRectContains(x, y, W, H, R) ? white : clear;
            tex.SetPixels32(pixels);
            tex.Apply();

            // Border tells Unity which pixels are the corners (for 9-slicing)
            var sprite = Sprite.Create(tex, new Rect(0, 0, W, H), new Vector2(0.5f, 0.5f),
                                       100f, 0, SpriteMeshType.FullRect,
                                       new Vector4(R, R, R, R));
            clothOverlay.sprite = sprite;
            clothOverlay.type   = Image.Type.Sliced;
        }

        static bool RoundedRectContains(int px, int py, int w, int h, int r)
        {
            if (px >= r && px < w - r) return true;
            if (py >= r && py < h - r) return true;
            int cx = px < r ? r : w - 1 - r;
            int cy = py < r ? r : h - 1 - r;
            int dx = px - cx, dy = py - cy;
            return dx * dx + dy * dy <= r * r;
        }

        // Called by PolishPhaseUI when a new question loads — just shows gem, no animation.
        public void ShowRandomGem()
        {
            if (gemSprites != null && gemSprites.Length > 0)
                gemImage.sprite = gemSprites[UnityEngine.Random.Range(0, gemSprites.Length)];

            if (clothOverlay != null) clothOverlay.gameObject.SetActive(false);
            HideRays();
        }

        // Called by PolishPhaseUI after the player submits answers.
        // score: 0..1  |  onComplete fires after the full animation so the UI can reveal Continue.
        public void PlayPolishAnimation(float score, Action onComplete)
        {
            if (activeAnim != null) StopCoroutine(activeAnim);
            activeAnim = StartCoroutine(PolishSequence(score, onComplete));
        }

        // ── Full animation sequence ───────────────────────────────────────────────

        IEnumerator PolishSequence(float score, Action onComplete)
        {
            if (clothOverlay != null)
            {
                var rt = clothOverlay.rectTransform;

                // Phase 1 — circular polishing motion
                clothOverlay.gameObject.SetActive(true);
                float circDuration  = 1.4f;
                float radius        = 60f;
                float revolutions   = 3f;
                float elapsed       = 0f;
                // Start at angle 0 so the cloth appears at the right of the gem
                rt.anchoredPosition = new Vector2(radius, 0f);

                while (elapsed < circDuration)
                {
                    elapsed += Time.deltaTime;
                    float t     = elapsed / circDuration;
                    float angle = t * revolutions * Mathf.PI * 2f;
                    rt.anchoredPosition = new Vector2(Mathf.Cos(angle) * radius,
                                                      Mathf.Sin(angle) * radius);
                    yield return null;
                }

                // Phase 2 — wipe away (left → right)
                float wipeDuration = 0.6f;
                elapsed = 0f;
                float halfW  = rt.sizeDelta.x * 0.5f;
                float startX = -halfW - 80f;
                float endX   =  halfW + 80f;
                rt.anchoredPosition = new Vector2(startX, rt.anchoredPosition.y);

                while (elapsed < wipeDuration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.SmoothStep(0f, 1f, elapsed / wipeDuration);
                    rt.anchoredPosition = new Vector2(Mathf.Lerp(startX, endX, t),
                                                      rt.anchoredPosition.y);
                    yield return null;
                }

                clothOverlay.gameObject.SetActive(false);
            }

            // Phase 3 — shine effect (overlaps right after wipe)
            if (score > 0.01f)
                yield return ShineAnim(score);

            onComplete?.Invoke();
        }

        // ── Shine effect ──────────────────────────────────────────────────────────

        IEnumerator ShineAnim(float score)
        {
            if (shineRays == null || shineRays.Length == 0) yield break;

            int   rayCount = Mathf.Max(1, Mathf.RoundToInt(score * shineRays.Length));
            float duration = 0.5f + score * 1.0f;
            Color rayColor = Color.Lerp(new Color(1f, 0.8f, 0.1f, 1f),
                                        new Color(1f, 1f,   0.8f, 1f), score);

            for (int i = 0; i < shineRays.Length; i++)
            {
                bool active = i < rayCount;
                shineRays[i].gameObject.SetActive(active);
                if (active)
                {
                    shineRays[i].localScale = new Vector3(1f, 0f, 1f);
                    var img = shineRays[i].GetComponent<Image>();
                    if (img != null) img.color = rayColor;
                }
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t      = elapsed / duration;
                float scaleY = Mathf.Sin(t * Mathf.PI);
                float alpha  = 1f - Mathf.Pow(t, 2f);

                for (int i = 0; i < rayCount && i < shineRays.Length; i++)
                {
                    shineRays[i].localScale = new Vector3(1f, scaleY, 1f);
                    var img = shineRays[i].GetComponent<Image>();
                    if (img != null)
                    {
                        var c = img.color;
                        c.a = alpha;
                        img.color = c;
                    }
                }
                yield return null;
            }

            HideRays();
        }

        void HideRays()
        {
            if (shineRays == null) return;
            foreach (var r in shineRays)
                if (r != null) r.gameObject.SetActive(false);
        }
    }
}
