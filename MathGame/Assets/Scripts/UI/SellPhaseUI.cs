using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicGame
{
    // Manages the multiple-choice "Sell" phase (5 questions per round).
    public class SellPhaseUI : MonoBehaviour
    {
        const int QuestionsPerRound = 5;

        [Header("Question display")]
        [SerializeField] TextMeshProUGUI headerText;
        [SerializeField] TextMeshProUGUI questionText;
        [SerializeField] TextMeshProUGUI feedbackText;
        [SerializeField] TextMeshProUGUI explanationText;

        [Header("Choice buttons (4)")]
        [SerializeField] Button[] choiceButtons;
        [SerializeField] TextMeshProUGUI[] choiceLabels;

        [Header("Controls")]
        [SerializeField] Button nextButton;
        [SerializeField] TextMeshProUGUI scoreText;  // live correct count

        readonly Color normalColor  = new Color(0.2f, 0.2f, 0.2f);
        readonly Color correctColor = new Color(0.1f, 0.7f, 0.2f);
        readonly Color wrongColor   = new Color(0.8f, 0.1f, 0.1f);

        List<MultipleChoiceQuestion> questions;
        int currentIndex;
        int correctCount;
        bool answeredCurrent;

        void Awake()
        {
            nextButton.onClick.AddListener(OnNext);
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                int idx = i;
                if (choiceButtons[idx] != null)
                    choiceButtons[idx].onClick.AddListener(() => OnChoiceSelected(idx));
            }
        }

        void OnEnable() => StartSellPhase();

        void StartSellPhase()
        {
            questions = QuestionBank.GetRandomMCQ(GameManager.Instance.CurrentDifficulty, QuestionsPerRound);
            currentIndex = 0;
            correctCount = 0;
            LoadCurrentQuestion();
        }

        void LoadCurrentQuestion()
        {
            answeredCurrent = false;
            var q = questions[currentIndex];

            headerText.text   = $"Sell Phase  —  Question {currentIndex + 1} / {questions.Count}";
            questionText.text = q.questionText;
            feedbackText.text    = "";
            explanationText.text = "";
            explanationText.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            scoreText.text = $"Correct: {correctCount} / {currentIndex}";

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                bool hasChoice = i < q.choices.Length;
                choiceButtons[i].gameObject.SetActive(hasChoice);
                if (hasChoice)
                {
                    choiceLabels[i].text = $"{(char)('A' + i)})  {q.choices[i]}";
                    SetChoiceColor(i, normalColor);
                    choiceButtons[i].interactable = true;
                }
            }
        }

        public void OnChoiceSelected(int index)
        {
            if (answeredCurrent) return;
            answeredCurrent = true;

            var q = questions[currentIndex];
            bool correct = index == q.correctIndex;
            if (correct) correctCount++;

            SetChoiceColor(index, correct ? correctColor : wrongColor);
            if (!correct) SetChoiceColor(q.correctIndex, correctColor);

            feedbackText.text = correct ? "Correct!" : $"Incorrect. The right answer was {(char)('A' + q.correctIndex)}.";
            explanationText.text = q.explanation;
            explanationText.gameObject.SetActive(true);
            scoreText.text = $"Correct: {correctCount} / {currentIndex + 1}";

            foreach (var b in choiceButtons) b.interactable = false;
            nextButton.gameObject.SetActive(true);
        }

        public void OnNext()
        {
            currentIndex++;
            if (currentIndex < questions.Count)
            {
                LoadCurrentQuestion();
            }
            else
            {
                float multiplier = ScoreManager.SellMultiplier(correctCount, questions.Count);
                ScoreManager.Instance.CommitRound(multiplier);
                GameManager.Instance.OnSellComplete();
            }
        }

        void SetChoiceColor(int i, Color c)
        {
            var colors = choiceButtons[i].colors;
            colors.normalColor = c;
            choiceButtons[i].colors = colors;
        }
    }
}
