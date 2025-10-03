using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestPaperSystem
{
    /// <summary>
    /// Represents grading results for a test.
    /// </summary>
    public class GradingResult
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalPointsEarned { get; set; }
        public int TotalPointsPossible { get; set; }
        public double Percentage { get; set; }
        public List<QuestionResult> Details { get; set; }

        public GradingResult()
        {
            Details = new List<QuestionResult>();
        }
    }

    /// <summary>
    /// Represents the result for a single question.
    /// </summary>
    public class QuestionResult
    {
        public int QuestionNumber { get; set; }
        public string StudentAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public char CorrectLetter { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public int PointsPossible { get; set; }
    }

    /// <summary>
    /// Represents a multiple choice test paper that teachers can create.
    /// </summary>
    public class TestPaper
    {
        public string Title { get; set; }
        public string Subject { get; set; }
        public string TeacherName { get; set; }
        public string Instructions { get; set; }
        public List<Question> Questions { get; private set; }
        public int TotalPoints => Questions.Sum(q => q.Points);

        /// <summary>
        /// Initializes a new instance of the TestPaper class.
        /// </summary>
        /// <param name="title">Title of the test</param>
        /// <param name="subject">Subject name (optional)</param>
        /// <param name="teacherName">Name of the teacher creating the test (optional)</param>
        /// <param name="instructions">Special instructions for students (optional)</param>
        public TestPaper(string title, string subject = null, string teacherName = null, string instructions = null)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Subject = subject;
            TeacherName = teacherName;
            Instructions = instructions;
            Questions = new List<Question>();
        }

        /// <summary>
        /// Adds a multiple choice question to the test paper.
        /// </summary>
        /// <param name="questionText">The question text</param>
        /// <param name="options">List of answer options</param>
        /// <param name="correctAnswer">The correct answer</param>
        /// <param name="points">Points for this question (default: 1)</param>
        /// <returns>The created question object</returns>
        public Question AddQuestion(string questionText, List<string> options, string correctAnswer, int points = 1)
        {
            var question = new Question(questionText, options, correctAnswer, points);
            Questions.Add(question);
            return question;
        }

        /// <summary>
        /// Adds a pre-created Question object to the test paper.
        /// </summary>
        /// <param name="question">The question object to add</param>
        public void AddQuestion(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            Questions.Add(question);
        }

        /// <summary>
        /// Removes a question by its index.
        /// </summary>
        /// <param name="index">Index of the question to remove (0-based)</param>
        /// <returns>The removed question object</returns>
        public Question RemoveQuestion(int index)
        {
            if (index < 0 || index >= Questions.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Question index out of range");

            var removedQuestion = Questions[index];
            Questions.RemoveAt(index);
            return removedQuestion;
        }

        /// <summary>
        /// Gets the total number of questions in the test.
        /// </summary>
        /// <returns>The number of questions</returns>
        public int GetQuestionCount()
        {
            return Questions.Count;
        }

        /// <summary>
        /// Displays the formatted test paper.
        /// </summary>
        /// <param name="showAnswers">Whether to show correct answers (for teacher's copy)</param>
        /// <returns>Formatted test paper string</returns>
        public string DisplayTest(bool showAnswers = false)
        {
            var sb = new StringBuilder();
            
            // Header
            sb.AppendLine(new string('=', 60));
            sb.AppendLine($"TEST: {Title}");
            
            if (!string.IsNullOrWhiteSpace(Subject))
                sb.AppendLine($"Subject: {Subject}");
            
            if (!string.IsNullOrWhiteSpace(TeacherName))
                sb.AppendLine($"Teacher: {TeacherName}");
            
            sb.AppendLine($"Total Questions: {GetQuestionCount()}");
            sb.AppendLine($"Total Points: {TotalPoints}");
            sb.AppendLine(new string('=', 60));

            // Instructions
            if (!string.IsNullOrWhiteSpace(Instructions))
            {
                sb.AppendLine();
                sb.AppendLine("INSTRUCTIONS:");
                sb.AppendLine(Instructions);
                sb.AppendLine();
            }

            // Questions
            sb.AppendLine();
            sb.AppendLine("QUESTIONS:");
            sb.AppendLine(new string('-', 40));

            for (int i = 0; i < Questions.Count; i++)
            {
                sb.AppendLine();
                sb.Append(Questions[i].ToString(i + 1, showAnswers));
            }

            // Answer key (if showing answers)
            if (showAnswers)
            {
                sb.AppendLine();
                sb.AppendLine(new string('=', 60));
                sb.AppendLine("ANSWER KEY:");
                sb.AppendLine(new string('-', 20));
                
                for (int i = 0; i < Questions.Count; i++)
                {
                    var question = Questions[i];
                    char correctLetter = question.GetCorrectAnswerLetter();
                    sb.AppendLine($"{i + 1}. {correctLetter}) {question.CorrectAnswer}");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Saves the test paper to a file.
        /// </summary>
        /// <param name="filename">Name of the file to save to</param>
        /// <param name="showAnswers">Whether to include answers in the saved file</param>
        public void SaveToFile(string filename, bool showAnswers = false)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename cannot be null or empty", nameof(filename));

            File.WriteAllText(filename, DisplayTest(showAnswers));
        }

        /// <summary>
        /// Grades a student's test answers.
        /// </summary>
        /// <param name="studentAnswers">List of student's answers in order</param>
        /// <returns>Grading results with score, percentage, and details</returns>
        public GradingResult GradeTest(List<string> studentAnswers)
        {
            if (studentAnswers == null)
                throw new ArgumentNullException(nameof(studentAnswers));

            if (studentAnswers.Count != Questions.Count)
                throw new ArgumentException("Number of answers must match number of questions");

            var result = new GradingResult
            {
                TotalQuestions = Questions.Count,
                TotalPointsPossible = TotalPoints
            };

            for (int i = 0; i < Questions.Count; i++)
            {
                var question = Questions[i];
                var studentAnswer = studentAnswers[i];
                var isCorrect = question.CheckAnswer(studentAnswer);
                var pointsEarned = isCorrect ? question.Points : 0;

                if (isCorrect)
                {
                    result.CorrectAnswers++;
                    result.TotalPointsEarned += question.Points;
                }

                var questionResult = new QuestionResult
                {
                    QuestionNumber = i + 1,
                    StudentAnswer = studentAnswer,
                    CorrectAnswer = question.CorrectAnswer,
                    CorrectLetter = question.GetCorrectAnswerLetter(),
                    IsCorrect = isCorrect,
                    PointsEarned = pointsEarned,
                    PointsPossible = question.Points
                };

                result.Details.Add(questionResult);
            }

            result.Percentage = result.TotalPointsPossible > 0 
                ? Math.Round((double)result.TotalPointsEarned / result.TotalPointsPossible * 100, 2) 
                : 0;

            return result;
        }

        /// <summary>
        /// Creates a copy of the test paper.
        /// </summary>
        /// <returns>A new TestPaper instance with the same questions</returns>
        public TestPaper Clone()
        {
            var clone = new TestPaper(Title, Subject, TeacherName, Instructions);
            
            foreach (var question in Questions)
            {
                clone.AddQuestion(question.QuestionText, 
                    new List<string>(question.Options), 
                    question.CorrectAnswer, 
                    question.Points);
            }

            return clone;
        }

        /// <summary>
        /// Shuffles the order of questions in the test.
        /// </summary>
        public void ShuffleQuestions()
        {
            var random = new Random();
            for (int i = Questions.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = Questions[i];
                Questions[i] = Questions[j];
                Questions[j] = temp;
            }
        }

        /// <summary>
        /// Shuffles the options within each question.
        /// </summary>
        public void ShuffleOptions()
        {
            var random = new Random();
            
            foreach (var question in Questions)
            {
                var shuffledOptions = question.Options.OrderBy(x => random.Next()).ToList();
                question.Options.Clear();
                question.Options.AddRange(shuffledOptions);
            }
        }
    }
}