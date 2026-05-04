using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicGame
{
    public class ResultsUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gemValueText;
        [SerializeField] TextMeshProUGUI multiplierText;
        [SerializeField] TextMeshProUGUI coinsEarnedText;
        [SerializeField] TextMeshProUGUI totalCoinsText;
        [SerializeField] Button playAgainButton;
        [SerializeField] Button mainMenuButton;

        void Awake()
        {
            playAgainButton.onClick.AddListener(OnPlayAgain);
            mainMenuButton.onClick.AddListener(OnMainMenu);
        }

        void OnEnable()
        {
            var sm = ScoreManager.Instance;
            gemValueText.text    = $"Gem Value:       {Mathf.RoundToInt(sm.LastGemValue)} coins";
            multiplierText.text  = $"Sell Multiplier:  x{sm.LastSellMultiplier:F2}";
            coinsEarnedText.text = $"Coins Earned:    {sm.LastRoundCoins} coins";
            totalCoinsText.text  = $"Total Coins:     {sm.TotalCoins} coins";
        }

        public void OnPlayAgain() => GameManager.Instance.OnPlayAgain();
        public void OnMainMenu()  => GameManager.Instance.OnReturnToMenu();
    }
}
