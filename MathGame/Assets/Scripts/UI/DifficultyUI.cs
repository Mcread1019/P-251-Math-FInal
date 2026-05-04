using UnityEngine;
using UnityEngine.UI;

namespace LogicGame
{
    public class DifficultyUI : MonoBehaviour
    {
        [SerializeField] Button easyButton;
        [SerializeField] Button mediumButton;
        [SerializeField] Button hardButton;
        [SerializeField] Button backButton;

        void Awake()
        {
            easyButton.onClick.AddListener(SelectEasy);
            mediumButton.onClick.AddListener(SelectMedium);
            hardButton.onClick.AddListener(SelectHard);
            backButton.onClick.AddListener(Back);
        }

        public void SelectEasy()   => GameManager.Instance.OnDifficultySelected(0);
        public void SelectMedium() => GameManager.Instance.OnDifficultySelected(1);
        public void SelectHard()   => GameManager.Instance.OnDifficultySelected(2);
        public void Back()         => GameManager.Instance.GoTo(GameState.MainMenu);
    }
}
