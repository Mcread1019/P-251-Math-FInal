using UnityEngine;

namespace LogicGame
{
    public enum GameState { MainMenu, DifficultySelect, PolishPhase, SellPhase, Results }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Panels — assign in Inspector")]
        [SerializeField] GameObject mainMenuPanel;
        [SerializeField] GameObject difficultyPanel;
        [SerializeField] GameObject polishPanel;
        [SerializeField] GameObject sellPanel;
        [SerializeField] GameObject resultsPanel;

        public Difficulty CurrentDifficulty { get; private set; }
        public GameState State { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start() => GoTo(GameState.MainMenu);

        // ── Navigation ──────────────────────────────────────────────────────────

        public void GoTo(GameState next)
        {
            State = next;
            mainMenuPanel.SetActive(next == GameState.MainMenu);
            difficultyPanel.SetActive(next == GameState.DifficultySelect);
            polishPanel.SetActive(next == GameState.PolishPhase);
            sellPanel.SetActive(next == GameState.SellPhase);
            resultsPanel.SetActive(next == GameState.Results);
        }

        // ── Called by UI buttons ─────────────────────────────────────────────────

        public void OnPlayPressed() => GoTo(GameState.DifficultySelect);

        public void OnDifficultySelected(int d)
        {
            CurrentDifficulty = (Difficulty)d;
            GoTo(GameState.PolishPhase);
        }

        public void OnPolishComplete() => GoTo(GameState.SellPhase);

        public void OnSellComplete() => GoTo(GameState.Results);

        public void OnPlayAgain() => GoTo(GameState.DifficultySelect);

        public void OnReturnToMenu() => GoTo(GameState.MainMenu);
    }
}
