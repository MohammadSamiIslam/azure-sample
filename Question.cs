using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPaperSystem
{
    /// <summary>
    /// Represents a multiple choice question.
    /// </summary>
    public class Question
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public int Points { get; set; }

        /// <summary>
        /// Initializes a new instance of the Question class.
        /// </summary>
        /// <param name="questionText">The question text</param>
        /// <param name="options">List of answer options</param>
        /// <param name="correctAnswer">The correct answer (must match one of the options)</param>
        /// <param name="points">Points for this question (default: 1)</param>
        public Question(string questionText, List<string> options, string correctAnswer, int points = 1)
        {
            if (options == null || options.Count < 2)
            {
                throw new ArgumentException("A question must have at least 2 options");
            }

            if (string.IsNullOrWhiteSpace(correctAnswer) || !options.Contains(correctAnswer))
            {
                throw new ArgumentException("Correct answer must be one of the provided options");
            }

            if (points <= 0)
            {
                throw new ArgumentException("Points must be a positive number");
            }

            QuestionText = questionText ?? throw new ArgumentNullException(nameof(questionText));
            Options = new List<string>(options);
            CorrectAnswer = correctAnswer;
            Points = points;
        }

        /// <summary>
        /// Returns a formatted string representation of the question.
        /// </summary>
        /// <param name="questionNumber">The question number (optional)</param>
        /// <param name="showCorrectAnswer">Whether to highlight the correct answer</param>
        /// <returns>Formatted question string</returns>
        public string ToString(int? questionNumber = null, bool showCorrectAnswer = false)
        {
            var sb = new StringBuilder();
            
            if (questionNumber.HasValue)
            {
                string pointText = Points == 1 ? "point" : "points";
                sb.AppendLine($"{questionNumber}. {QuestionText} ({Points} {pointText})");
            }
            else
            {
                sb.AppendLine(QuestionText);
            }

            for (int i = 0; i < Options.Count; i++)
            {
                char letter = (char)('A' + i);
                string marker = showCorrectAnswer && Options[i] == CorrectAnswer ? "→ " : "   ";
                sb.AppendLine($"{marker}{letter}) {Options[i]}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks if the provided answer is correct.
        /// </summary>
        /// <param name="answer">The answer to check</param>
        /// <returns>True if the answer is correct, false otherwise</returns>
        public bool CheckAnswer(string answer)
        {
            return string.Equals(answer, CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the letter corresponding to the correct answer (A, B, C, etc.)
        /// </summary>
        /// <returns>The letter of the correct answer</returns>
        public char GetCorrectAnswerLetter()
        {
            int index = Options.IndexOf(CorrectAnswer);
            return (char)('A' + index);
        }

        /// <summary>
        /// Gets the letter corresponding to a given answer option.
        /// </summary>
        /// <param name="answer">The answer option</param>
        /// <returns>The letter of the answer, or null if not found</returns>
        public char? GetAnswerLetter(string answer)
        {
            int index = Options.IndexOf(answer);
            return index >= 0 ? (char)('A' + index) : null;
        }
    }
}