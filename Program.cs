using System;
using System.Collections.Generic;
using TestPaperSystem;

namespace TestPaperSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check command line arguments
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "test":
                        TestRunner.RunAllTests();
                        return;
                    case "simple":
                        SimpleExample.RunExample();
                        return;
                }
            }

            Console.WriteLine("Welcome to the Multiple Choice Test Creation System!");
            Console.WriteLine("This program demonstrates how teachers can create tests using the TestPaper class.\n");

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. View sample test");
                Console.WriteLine("2. Create test interactively");
                Console.WriteLine("3. View grading demo");
                Console.WriteLine("4. Run system tests");
                Console.WriteLine("5. Exit");

                Console.Write("\nEnter your choice (1-5): ");
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        ShowSampleTest();
                        break;
                    case "2":
                        CreateTestInteractively();
                        break;
                    case "3":
                        DemonstrateGrading();
                        break;
                    case "4":
                        TestRunner.RunAllTests();
                        break;
                    case "5":
                        Console.WriteLine("Thank you for using the Test Creation System!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\n" + new string('-', 50) + "\n");
            }
        }

        static void ShowSampleTest()
        {
            Console.WriteLine("\n" + new string('=', 50));
            
            var test = CreateSampleTest();
            
            Console.WriteLine("STUDENT VERSION:");
            Console.WriteLine(test.DisplayTest(showAnswers: false));

            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("TEACHER VERSION (with answers):");
            Console.WriteLine(test.DisplayTest(showAnswers: true));

            // Save to files
            try
            {
                test.SaveToFile("sample_test_student.txt", showAnswers: false);
                test.SaveToFile("sample_test_teacher.txt", showAnswers: true);
                Console.WriteLine("\nTest saved to 'sample_test_student.txt' and 'sample_test_teacher.txt'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError saving files: {ex.Message}");
            }
        }

        static TestPaper CreateSampleTest()
        {
            var test = new TestPaper(
                title: "Introduction to C# Programming",
                subject: "Computer Science",
                teacherName: "Dr. Johnson",
                instructions: "Choose the best answer for each question. Mark your answers clearly."
            );

            test.AddQuestion(
                "What is the correct way to declare an integer variable in C#?",
                new List<string> { "int x;", "integer x;", "var x = int;", "Int32 x = new Int32();" },
                "int x;",
                points: 2
            );

            test.AddQuestion(
                "Which of the following is NOT a valid C# access modifier?",
                new List<string> { "public", "private", "protected", "global" },
                "global",
                points: 1
            );

            test.AddQuestion(
                "What does the 'var' keyword do in C#?",
                new List<string> { 
                    "Creates a variable that can hold any type", 
                    "Allows implicit type inference", 
                    "Creates a variant type", 
                    "Declares a volatile variable" 
                },
                "Allows implicit type inference",
                points: 1
            );

            test.AddQuestion(
                "Which collection type in C# is ordered and allows duplicate elements?",
                new List<string> { "HashSet<T>", "Dictionary<K,V>", "List<T>", "SortedSet<T>" },
                "List<T>",
                points: 1
            );

            // Add a bonus question
            var bonusQuestion = new Question(
                "What is the time complexity of adding an element to the end of a List<T> in C#?",
                new List<string> { "O(1) amortized", "O(n)", "O(log n)", "O(n²)" },
                "O(1) amortized",
                points: 3
            );
            test.AddQuestion(bonusQuestion);

            return test;
        }

        static void CreateTestInteractively()
        {
            Console.WriteLine("\n=== Multiple Choice Test Creator ===\n");

            // Get test details
            Console.Write("Enter test title: ");
            string title = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty. Returning to main menu.");
                return;
            }

            Console.Write("Enter subject (optional): ");
            string subject = Console.ReadLine();
            subject = string.IsNullOrWhiteSpace(subject) ? null : subject;

            Console.Write("Enter your name (optional): ");
            string teacherName = Console.ReadLine();
            teacherName = string.IsNullOrWhiteSpace(teacherName) ? null : teacherName;

            Console.Write("Enter special instructions (optional): ");
            string instructions = Console.ReadLine();
            instructions = string.IsNullOrWhiteSpace(instructions) ? null : instructions;

            // Create the test paper
            var test = new TestPaper(title, subject, teacherName, instructions);

            Console.WriteLine($"\nCreated test: {title}");
            Console.WriteLine("Now let's add some questions...\n");

            int questionCount = 1;
            while (true)
            {
                Console.WriteLine($"--- Question {questionCount} ---");
                Console.Write("Enter question text (or 'done' to finish): ");
                string questionText = Console.ReadLine() ?? "";

                if (questionText.ToLower() == "done")
                    break;

                if (string.IsNullOrWhiteSpace(questionText))
                {
                    Console.WriteLine("Question text cannot be empty. Please try again.");
                    continue;
                }

                // Get options
                var options = new List<string>();
                char[] optionLetters = { 'A', 'B', 'C', 'D', 'E', 'F' };

                Console.WriteLine("Enter answer options (minimum 2, press Enter on empty line to finish):");
                for (int i = 0; i < optionLetters.Length; i++)
                {
                    Console.Write($"Option {optionLetters[i]}: ");
                    string option = Console.ReadLine() ?? "";
                    
                    if (string.IsNullOrWhiteSpace(option))
                        break;
                    
                    options.Add(option.Trim());
                }

                if (options.Count < 2)
                {
                    Console.WriteLine("Error: Need at least 2 options. Please try again.");
                    continue;
                }

                // Get correct answer
                Console.WriteLine($"Available options: {string.Join(", ", options)}");
                string correctAnswer;
                while (true)
                {
                    Console.Write("Enter the correct answer (exactly as typed above): ");
                    correctAnswer = Console.ReadLine() ?? "";
                    
                    if (options.Contains(correctAnswer))
                        break;
                    
                    Console.WriteLine("Error: Correct answer must match one of the options exactly.");
                }

                // Get points
                int points = 1;
                while (true)
                {
                    Console.Write("Enter points for this question (default 1): ");
                    string pointsInput = Console.ReadLine() ?? "";
                    
                    if (string.IsNullOrWhiteSpace(pointsInput))
                    {
                        points = 1;
                        break;
                    }
                    
                    if (int.TryParse(pointsInput, out points) && points > 0)
                        break;
                    
                    Console.WriteLine("Points must be a positive number.");
                }

                // Add the question
                try
                {
                    test.AddQuestion(questionText, options, correctAnswer, points);
                    Console.WriteLine($"Question {questionCount} added successfully!\n");
                    questionCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding question: {ex.Message}");
                }
            }

            if (test.GetQuestionCount() > 0)
            {
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine("Your completed test:");
                Console.WriteLine(test.DisplayTest(showAnswers: true));

                Console.Write("\nSave test to file? (y/n): ");
                string save = Console.ReadLine()?.ToLower() ?? "";
                
                if (save == "y" || save == "yes")
                {
                    Console.Write("Enter filename (without extension): ");
                    string filename = Console.ReadLine() ?? "test";
                    
                    if (string.IsNullOrWhiteSpace(filename))
                        filename = "test";

                    try
                    {
                        test.SaveToFile($"{filename}_student.txt", showAnswers: false);
                        test.SaveToFile($"{filename}_teacher.txt", showAnswers: true);
                        Console.WriteLine($"Test saved to '{filename}_student.txt' and '{filename}_teacher.txt'");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving files: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No questions were added to the test.");
            }
        }

        static void DemonstrateGrading()
        {
            Console.WriteLine("\n=== Grading Demo ===");

            // Create a simple test
            var test = new TestPaper("Quick Math Quiz", "Mathematics", "Ms. Johnson");
            test.AddQuestion("What is 2 + 2?", new List<string> { "3", "4", "5", "6" }, "4");
            test.AddQuestion("What is 5 * 3?", new List<string> { "13", "15", "17", "20" }, "15");
            test.AddQuestion("What is 10 / 2?", new List<string> { "4", "5", "6", "7" }, "5");

            Console.WriteLine("Test Questions:");
            Console.WriteLine(test.DisplayTest());

            // Simulate student answers
            var studentAnswers = new List<string> { "4", "15", "6" }; // Two correct, one wrong

            Console.WriteLine($"\nStudent Answers: [{string.Join(", ", studentAnswers)}]");

            // Grade the test
            try
            {
                var results = test.GradeTest(studentAnswers);

                Console.WriteLine($"\n=== GRADING RESULTS ===");
                Console.WriteLine($"Total Questions: {results.TotalQuestions}");
                Console.WriteLine($"Correct Answers: {results.CorrectAnswers}");
                Console.WriteLine($"Score: {results.TotalPointsEarned}/{results.TotalPointsPossible}");
                Console.WriteLine($"Percentage: {results.Percentage}%");

                Console.WriteLine("\nDetailed Results:");
                foreach (var detail in results.Details)
                {
                    string status = detail.IsCorrect ? "✓" : "✗";
                    Console.WriteLine($"  Q{detail.QuestionNumber}: {status} " +
                                    $"Student: {detail.StudentAnswer}, " +
                                    $"Correct: {detail.CorrectAnswer} ({detail.CorrectLetter})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error grading test: {ex.Message}");
            }
        }
    }
}