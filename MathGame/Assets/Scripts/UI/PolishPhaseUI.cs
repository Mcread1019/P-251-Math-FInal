using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicGame
{
    // Manages the fill-in-blank "Polish" phase.
    // Word bank buttons let the player fill blank slots; Submit checks answers.
    public class PolishPhaseUI : MonoBehaviour
    {
        [Header("Question display")]
        [SerializeField] TextMeshProUGUI questionText;
        [SerializeField] TextMeshProUGUI feedbackText;
        [SerializeField] TextMeshProUGUI explanationText;

        [Header("Blank slots (create as many as max blanks needed = 5)")]
        [SerializeField] Button[] blankSlotButtons;     // 5 buttons showing placed word or "___"
        [SerializeField] TextMeshProUGUI[] blankSlotLabels;  // text on each slot button

        [Header("Word bank (create 8 buttons)")]
        [SerializeField] Button[] wordBankButtons;      // up to 8 word-bank option buttons
        [SerializeField] TextMeshProUGUI[] wordBankLabels;

        [Header("Controls")]
        [SerializeField] Button submitButton;
        [SerializeField] Button continueButton;
        [SerializeField] TextMeshProUGUI gemValueText;

        [Header("Gem display")]
        [SerializeField] GemDisplayController gemDisplay;

        // Runtime state
        FillBlankQuestion currentQuestion;
        string[] slotContents;          // what's in each blank slot (null = empty)
        bool[] slotActive;              // which slots exist for this question
        List<string> availableWords;    // words still in the bank
        int selectedSlot = -1;          // which slot is highlighted, -1 = none
        bool submitted;

        const string EMPTY_SLOT = "___";
        readonly Color normalColor   = new Color(0.2f, 0.2f, 0.2f);
        readonly Color selectedColor = new Color(0.1f, 0.5f, 0.8f);
        readonly Color correctColor  = new Color(0.1f, 0.7f, 0.2f);
        readonly Color wrongColor    = new Color(0.8f, 0.1f, 0.1f);

        void Awake()
        {
            submitButton.onClick.AddListener(OnSubmit);
            continueButton.onClick.AddListener(OnContinue);
            for (int i = 0; i < blankSlotButtons.Length; i++)
            {
                int idx = i;
                if (blankSlotButtons[idx] != null)
                    blankSlotButtons[idx].onClick.AddListener(() => OnSlotClicked(idx));
            }
            for (int i = 0; i < wordBankButtons.Length; i++)
            {
                int idx = i;
                if (wordBankButtons[idx] != null)
                    wordBankButtons[idx].onClick.AddListener(() => OnWordClicked(idx));
            }
        }

        void OnEnable() => LoadQuestion();

        void LoadQuestion()
        {
            submitted = false;
            selectedSlot = -1;
            currentQuestion = QuestionBank.GetRandomSoundness(GameManager.Instance.CurrentDifficulty);
            gemDisplay?.ShowRandomGem();

            if (currentQuestion == null)
            {
                questionText.text = "No questions available for this difficulty.";
                return;
            }

            // Build display: replace [_N] markers with numbered labels
            string display = currentQuestion.questionText;
            int blankCount = currentQuestion.correctAnswers.Length;
            for (int i = 0; i < blankCount; i++)
                display = display.Replace($"[_{i}]", $"<b>[{i + 1}]</b>");
            questionText.text = display;

            // Init slot state
            slotContents = new string[blankSlotButtons.Length];
            slotActive   = new bool[blankSlotButtons.Length];
            for (int i = 0; i < blankSlotButtons.Length; i++)
            {
                bool active = i < blankCount;
                slotActive[i] = active;
                slotContents[i] = null;
                blankSlotButtons[i].gameObject.SetActive(active);
                if (active) SetSlotLabel(i, EMPTY_SLOT);
                SetSlotColor(i, normalColor);
            }

            // Shuffle word bank
            availableWords = new List<string>(currentQuestion.wordBank);
            ShuffleList(availableWords);
            RefreshWordBank();

            feedbackText.text = "";
            explanationText.text = "";
            explanationText.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
            gemValueText.text = "";
        }

        // ── Slot interaction ────────────────────────────────────────────────────

        public void OnSlotClicked(int slotIndex)
        {
            if (submitted || !slotActive[slotIndex]) return;

            if (slotContents[slotIndex] != null)
            {
                // Return the word to the bank
                availableWords.Add(slotContents[slotIndex]);
                slotContents[slotIndex] = null;
                SetSlotLabel(slotIndex, EMPTY_SLOT);
                SetSlotColor(slotIndex, normalColor);
                RefreshWordBank();
                if (selectedSlot == slotIndex) selectedSlot = -1;
            }
            else
            {
                // Select this slot to receive the next word-bank click
                selectedSlot = slotIndex;
                for (int i = 0; i < blankSlotButtons.Length; i++)
                    SetSlotColor(i, i == slotIndex ? selectedColor : normalColor);
            }
        }

        // ── Word bank interaction ────────────────────────────────────────────────

        public void OnWordClicked(int wordIndex)
        {
            if (submitted || wordIndex >= availableWords.Count) return;

            // Auto-select first empty slot if none selected
            if (selectedSlot == -1)
            {
                for (int i = 0; i < slotActive.Length; i++)
                {
                    if (slotActive[i] && slotContents[i] == null)
                    { selectedSlot = i; break; }
                }
            }
            if (selectedSlot == -1) return;

            string word = availableWords[wordIndex];
            availableWords.RemoveAt(wordIndex);
            slotContents[selectedSlot] = word;
            SetSlotLabel(selectedSlot, word);
            SetSlotColor(selectedSlot, normalColor);
            selectedSlot = -1;
            RefreshWordBank();
        }

        // ── Submit ───────────────────────────────────────────────────────────────

        public void OnSubmit()
        {
            if (submitted) return;
            submitted = true;

            int correct = 0;
            int total = currentQuestion.correctAnswers.Length;

            for (int i = 0; i < total; i++)
            {
                bool right = slotContents[i] == currentQuestion.correctAnswers[i];
                if (right) correct++;
                SetSlotColor(i, right ? correctColor : wrongColor);
            }

            float base_    = ScoreManager.BaseGemValue(GameManager.Instance.CurrentDifficulty);
            float gemValue = base_ * (correct / (float)total);
            ScoreManager.Instance.SetGemValue(gemValue);

            // Show result text immediately; Continue button waits for gem animation
            feedbackText.text    = $"Polish result: {correct}/{total} correct — Gem value: {Mathf.RoundToInt(gemValue)} coins";
            explanationText.text = currentQuestion.explanation;
            explanationText.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
            foreach (var b in wordBankButtons) b.interactable = false;

            float score = correct / (float)total;
            if (gemDisplay != null)
            {
                gemDisplay.PlayPolishAnimation(score, () =>
                {
                    gemValueText.text = $"Gem value: {Mathf.RoundToInt(gemValue)}";
                    continueButton.gameObject.SetActive(true);
                });
            }
            else
            {
                gemValueText.text = $"Gem value: {Mathf.RoundToInt(gemValue)}";
                continueButton.gameObject.SetActive(true);
            }
        }

        public void OnContinue() => GameManager.Instance.OnPolishComplete();

        // ── Helpers ─────────────────────────────────────────────────────────────

        void RefreshWordBank()
        {
            for (int i = 0; i < wordBankButtons.Length; i++)
            {
                bool show = i < availableWords.Count;
                wordBankButtons[i].gameObject.SetActive(show);
                if (show) wordBankLabels[i].text = availableWords[i];
                wordBankButtons[i].interactable = true;
            }
        }

        void SetSlotLabel(int i, string text) => blankSlotLabels[i].text = text;

        void SetSlotColor(int i, Color c)
        {
            var colors = blankSlotButtons[i].colors;
            colors.normalColor = c;
            blankSlotButtons[i].colors = colors;
        }

        static void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
