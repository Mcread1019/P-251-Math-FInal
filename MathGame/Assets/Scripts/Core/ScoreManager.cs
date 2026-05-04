using UnityEngine;

namespace LogicGame
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        public int TotalCoins { get; private set; }
        public int LastRoundCoins { get; private set; }
        public float LastGemValue { get; private set; }
        public float LastSellMultiplier { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        // Call after polish: gemValue = baseDifficulty * (correctBlanks / totalBlanks)
        public void SetGemValue(float value) => LastGemValue = value;

        // Call after sell: multiplier in range [0.5, 2.0]
        public void CommitRound(float sellMultiplier)
        {
            LastSellMultiplier = sellMultiplier;
            LastRoundCoins = Mathf.RoundToInt(LastGemValue * sellMultiplier);
            TotalCoins += LastRoundCoins;
        }

        public void ResetSession() { TotalCoins = 0; LastRoundCoins = 0; }

        // Base gem value per difficulty tier
        public static float BaseGemValue(Difficulty d) => d switch
        {
            Difficulty.Easy   => 10f,
            Difficulty.Medium => 25f,
            Difficulty.Hard   => 50f,
            _                 => 10f
        };

        // Sell multiplier: 0.5 at 0 correct, 2.0 at all correct, linear between
        public static float SellMultiplier(int correct, int total)
        {
            if (total <= 0) return 0.5f;
            return 0.5f + (correct / (float)total) * 1.5f;
        }
    }
}
