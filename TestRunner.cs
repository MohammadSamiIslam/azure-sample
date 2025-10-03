using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPaperSystem
{
    /// <summary>
    /// Simple test runner to verify the functionality of the TestPaper system.
    /// </summary>
    public class TestRunner
    {
        private static int _testsRun = 0;
        private static int _testsPassed = 0;

        public static void RunAllTests()
        {
            Console.WriteLine("Running TestPaper System Tests...\n");

            TestQuestionCreation();
            TestQuestionValidation();
            TestTestPaperCreation();
            TestQuestionManagement();
            TestGrading();
            TestFileOperations();
            TestEdgeCases();

            Console.WriteLine($"\n=== TEST SUMMARY ===");
            Console.WriteLine($"Tests Run: {_testsRun}");
            Console.WriteLine($"Tests Passed: {_testsPassed}");
            Console.WriteLine($"Tests Failed: {_testsRun - _testsPassed}");

            if (_testsPassed == _testsRun)
            {
                Console.WriteLine("✅ All tests passed!");
            }
            else
            {
                Console.WriteLine("❌ Some tests failed!");
            }
        }

        private static void TestQuestionCreation()
        {
            Console.WriteLine("Testing Question Creation...");

            // Test basic question creation
            AssertNoException(() =>
            {
                var question = new Question("What is 2+2?", new List<string> { "3", "4", "5" }, "4");
                Assert(question.QuestionText == "What is 2+2?", "Question text should be set correctly");
                Assert(question.Options.Count == 3, "Should have 3 options");
                Assert(question.CorrectAnswer == "4", "Correct answer should be set");
                Assert(question.Points == 1, "Default points should be 1");
            }, "Basic question creation");

            // Test question with custom points
            AssertNoException(() =>
            {
                var question = new Question("Hard question", new List<string> { "A", "B" }, "A", 5);
                Assert(question.Points == 5, "Custom points should be set correctly");
            }, "Question with custom points");

            Console.WriteLine("✓ Question creation tests completed\n");
        }

        private static void TestQuestionValidation()
        {
            Console.WriteLine("Testing Question Validation...");

            // Test insufficient options
            AssertException<ArgumentException>(() =>
            {
                new Question("Invalid", new List<string> { "A" }, "A");
            }, "Should require at least 2 options");

            // Test incorrect answer
            AssertException<ArgumentException>(() =>
            {
                new Question("Invalid", new List<string> { "A", "B" }, "C");
            }, "Should reject answer not in options");

            // Test zero points
            AssertException<ArgumentException>(() =>
            {
                new Question("Invalid", new List<string> { "A", "B" }, "A", 0);
            }, "Should reject zero points");

            // Test null question text
            AssertException<ArgumentNullException>(() =>
            {
                new Question(null, new List<string> { "A", "B" }, "A");
            }, "Should reject null question text");

            Console.WriteLine("✓ Question validation tests completed\n");
        }

        private static void TestTestPaperCreation()
        {
            Console.WriteLine("Testing TestPaper Creation...");

            AssertNoException(() =>
            {
                var test = new TestPaper("Sample Test", "Math", "Teacher");
                Assert(test.Title == "Sample Test", "Title should be set");
                Assert(test.Subject == "Math", "Subject should be set");
                Assert(test.TeacherName == "Teacher", "Teacher name should be set");
                Assert(test.GetQuestionCount() == 0, "Should start with no questions");
                Assert(test.TotalPoints == 0, "Should start with zero points");
            }, "Basic test paper creation");

            Console.WriteLine("✓ TestPaper creation tests completed\n");
        }

        private static void TestQuestionManagement()
        {
            Console.WriteLine("Testing Question Management...");

            AssertNoException(() =>
            {
                var test = new TestPaper("Test");
                
                // Add questions
                test.AddQuestion("Q1", new List<string> { "A", "B" }, "A", 2);
                test.AddQuestion("Q2", new List<string> { "X", "Y" }, "Y", 1);
                
                Assert(test.GetQuestionCount() == 2, "Should have 2 questions");
                Assert(test.TotalPoints == 3, "Should have 3 total points");
                
                // Remove question
                var removed = test.RemoveQuestion(0);
                Assert(test.GetQuestionCount() == 1, "Should have 1 question after removal");
                Assert(test.TotalPoints == 1, "Should have 1 point after removal");
                Assert(removed.QuestionText == "Q1", "Should return removed question");
                
            }, "Question management");

            Console.WriteLine("✓ Question management tests completed\n");
        }

        private static void TestGrading()
        {
            Console.WriteLine("Testing Grading System...");

            AssertNoException(() =>
            {
                var test = new TestPaper("Grading Test");
                test.AddQuestion("Q1", new List<string> { "A", "B" }, "A", 2);
                test.AddQuestion("Q2", new List<string> { "X", "Y" }, "Y", 1);
                
                // Test perfect score
                var perfectAnswers = new List<string> { "A", "Y" };
                var perfectResults = test.GradeTest(perfectAnswers);
                
                Assert(perfectResults.CorrectAnswers == 2, "Should have 2 correct answers");
                Assert(perfectResults.TotalPointsEarned == 3, "Should earn all 3 points");
                Assert(perfectResults.Percentage == 100.0, "Should be 100%");
                
                // Test partial score
                var partialAnswers = new List<string> { "B", "Y" };
                var partialResults = test.GradeTest(partialAnswers);
                
                Assert(partialResults.CorrectAnswers == 1, "Should have 1 correct answer");
                Assert(partialResults.TotalPointsEarned == 1, "Should earn 1 point");
                Assert(partialResults.Percentage == 33.33, "Should be 33.33%");
                
            }, "Grading system");

            Console.WriteLine("✓ Grading tests completed\n");
        }

        private static void TestFileOperations()
        {
            Console.WriteLine("Testing File Operations...");

            AssertNoException(() =>
            {
                var test = new TestPaper("File Test");
                test.AddQuestion("Q1", new List<string> { "A", "B" }, "A");
                
                // Test display
                string studentDisplay = test.DisplayTest(false);
                string teacherDisplay = test.DisplayTest(true);
                
                Assert(studentDisplay.Contains("File Test"), "Should contain test title");
                Assert(!studentDisplay.Contains("ANSWER KEY"), "Student version should not have answer key");
                Assert(teacherDisplay.Contains("ANSWER KEY"), "Teacher version should have answer key");
                
                // Test save (basic validation - actual file I/O depends on file system)
                string testContent = test.DisplayTest();
                Assert(!string.IsNullOrEmpty(testContent), "Should generate non-empty content");
                
            }, "File operations");

            Console.WriteLine("✓ File operation tests completed\n");
        }

        private static void TestEdgeCases()
        {
            Console.WriteLine("Testing Edge Cases...");

            // Test grading with wrong number of answers
            AssertException<ArgumentException>(() =>
            {
                var test = new TestPaper("Edge Test");
                test.AddQuestion("Q1", new List<string> { "A", "B" }, "A");
                test.GradeTest(new List<string> { "A", "B" }); // Too many answers
            }, "Should reject wrong number of answers");

            // Test removing question with invalid index
            AssertException<ArgumentOutOfRangeException>(() =>
            {
                var test = new TestPaper("Edge Test");
                test.RemoveQuestion(0); // No questions exist
            }, "Should reject invalid removal index");

            // Test case-insensitive answer checking
            AssertNoException(() =>
            {
                var question = new Question("Test", new List<string> { "Answer" }, "Answer");
                Assert(question.CheckAnswer("answer"), "Should accept lowercase answer");
                Assert(question.CheckAnswer("ANSWER"), "Should accept uppercase answer");
            }, "Case-insensitive answer checking");

            Console.WriteLine("✓ Edge case tests completed\n");
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"Assertion failed: {message}");
            }
        }

        private static void AssertException<T>(Action action, string message) where T : Exception
        {
            _testsRun++;
            try
            {
                action();
                Console.WriteLine($"❌ Expected exception {typeof(T).Name}: {message}");
            }
            catch (T)
            {
                Console.WriteLine($"✓ Correctly threw {typeof(T).Name}: {message}");
                _testsPassed++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Expected {typeof(T).Name} but got {ex.GetType().Name}: {message}");
            }
        }

        private static void AssertNoException(Action action, string message)
        {
            _testsRun++;
            try
            {
                action();
                Console.WriteLine($"✓ {message}");
                _testsPassed++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unexpected exception in '{message}': {ex.Message}");
            }
        }

        // Entry point for running tests independently
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "test")
            {
                RunAllTests();
            }
            else
            {
                Console.WriteLine("Use 'dotnet run test' to run tests, or 'dotnet run' for the main program.");
            }
        }
    }
}