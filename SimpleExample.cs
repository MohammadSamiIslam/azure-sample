using System;
using System.Collections.Generic;
using TestPaperSystem;

namespace TestPaperSystem
{
    /// <summary>
    /// Simple example showing basic usage of the TestPaper system.
    /// This demonstrates the core functionality without the interactive menu.
    /// </summary>
    public class SimpleExample
    {
        public static void RunExample()
        {
            Console.WriteLine("=== Simple TestPaper Example ===\n");

            // Create a new test paper
            var test = new TestPaper(
                title: "Basic Programming Quiz",
                subject: "Computer Science",
                teacherName: "Prof. Johnson",
                instructions: "Choose the best answer for each question."
            );

            Console.WriteLine("1. Creating a test paper...");
            Console.WriteLine($"   Title: {test.Title}");
            Console.WriteLine($"   Subject: {test.Subject}");
            Console.WriteLine($"   Teacher: {test.TeacherName}\n");

            // Add some questions
            Console.WriteLine("2. Adding questions...");

            test.AddQuestion(
                "What does 'OOP' stand for?",
                new List<string> { 
                    "Object-Oriented Programming", 
                    "Online Operating Platform", 
                    "Open Office Protocol", 
                    "Optimal Output Processing" 
                },
                "Object-Oriented Programming",
                points: 2
            );

            test.AddQuestion(
                "Which of these is a loop structure?",
                new List<string> { "if", "for", "class", "namespace" },
                "for",
                points: 1
            );

            test.AddQuestion(
                "What is the result of 5 + 3 * 2?",
                new List<string> { "11", "16", "10", "13" },
                "11",
                points: 1
            );

            Console.WriteLine($"   Added {test.GetQuestionCount()} questions");
            Console.WriteLine($"   Total points: {test.TotalPoints}\n");

            // Display the student version
            Console.WriteLine("3. Student version of the test:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(test.DisplayTest(showAnswers: false));

            // Display the teacher version
            Console.WriteLine("\n4. Teacher version (with answers):");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(test.DisplayTest(showAnswers: true));

            // Simulate grading
            Console.WriteLine("\n5. Grading simulation:");
            Console.WriteLine(new string('-', 60));

            // Student answers: 2 correct, 1 wrong
            var studentAnswers = new List<string> { 
                "Object-Oriented Programming",  // Correct
                "if",                          // Wrong (should be "for")
                "11"                           // Correct
            };

            Console.WriteLine($"Student answers: [{string.Join(", ", studentAnswers)}]");

            var results = test.GradeTest(studentAnswers);

            Console.WriteLine($"\nGrading Results:");
            Console.WriteLine($"  Questions answered correctly: {results.CorrectAnswers}/{results.TotalQuestions}");
            Console.WriteLine($"  Points earned: {results.TotalPointsEarned}/{results.TotalPointsPossible}");
            Console.WriteLine($"  Percentage: {results.Percentage}%");

            Console.WriteLine($"\nDetailed breakdown:");
            foreach (var detail in results.Details)
            {
                string status = detail.IsCorrect ? "✓ Correct" : "✗ Incorrect";
                Console.WriteLine($"  Q{detail.QuestionNumber}: {status}");
                Console.WriteLine($"    Student: {detail.StudentAnswer}");
                Console.WriteLine($"    Correct: {detail.CorrectAnswer} ({detail.CorrectLetter})");
                Console.WriteLine($"    Points: {detail.PointsEarned}/{detail.PointsPossible}");
                Console.WriteLine();
            }

            // Demonstrate file saving
            Console.WriteLine("6. Saving to files...");
            try
            {
                test.SaveToFile("example_test_student.txt", showAnswers: false);
                test.SaveToFile("example_test_teacher.txt", showAnswers: true);
                Console.WriteLine("   ✓ Files saved successfully!");
                Console.WriteLine("   - example_test_student.txt (for students)");
                Console.WriteLine("   - example_test_teacher.txt (with answer key)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ Error saving files: {ex.Message}");
            }

            Console.WriteLine("\n=== Example Complete ===");
        }

        // Alternative entry point for running just this example
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "simple")
            {
                RunExample();
            }
            else
            {
                Console.WriteLine("Use 'dotnet run simple' to run this simple example,");
                Console.WriteLine("or 'dotnet run' for the full interactive program.");
            }
        }
    }
}