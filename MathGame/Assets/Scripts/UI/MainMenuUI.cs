using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicGame
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI totalScoreText;
        [SerializeField] Button playButton;

        void Awake()
        {
            playButton.onClick.AddListener(() => GameManager.Instance.OnPlayPressed());
        }

        void OnEnable()
        {
            if (ScoreManager.Instance != null)
                totalScoreText.text = $"Total Coins: {ScoreManager.Instance.TotalCoins}";
        }
    }
}
