using System;

namespace LogicGame
{
    public enum Difficulty { Easy, Medium, Hard }
    public enum QuestionType { TFL, FOL, Soundness }

    [Serializable]
    public class MultipleChoiceQuestion
    {
        public string questionText;
        public string[] choices;   // exactly 4
        public int correctIndex;
        public string explanation;
        public Difficulty difficulty;
        public QuestionType type;

        public MultipleChoiceQuestion(string q, string[] c, int correct, string exp, Difficulty d, QuestionType t)
        {
            questionText = q; choices = c; correctIndex = correct;
            explanation = exp; difficulty = d; type = t;
        }
    }

    // Fill-in-the-blank question. questionText uses [_0], [_1] … as blank markers.
    // correctAnswers[i] is the answer for blank i.
    // wordBank contains all options (correct + distractors).
    [Serializable]
    public class FillBlankQuestion
    {
        public string questionText;
        public string[] correctAnswers;
        public string[] wordBank;
        public string explanation;
        public Difficulty difficulty;

        public FillBlankQuestion(string q, string[] answers, string[] bank, string exp, Difficulty d)
        {
            questionText = q; correctAnswers = answers; wordBank = bank;
            explanation = exp; difficulty = d;
        }
    }
}
