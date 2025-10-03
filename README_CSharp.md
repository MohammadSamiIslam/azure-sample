# Multiple Choice Test Paper Creator (C#)

A C# program that allows teachers to create multiple choice tests using a `TestPaper` class. This system provides comprehensive functionality for creating, managing, and grading multiple choice tests.

## Features

- **Easy Test Creation**: Create tests with customizable titles, subjects, and instructions
- **Question Management**: Add, remove, and organize multiple choice questions
- **Flexible Scoring**: Assign different point values to questions
- **Test Display**: Generate formatted test papers for students and teachers
- **Answer Keys**: Automatically generate answer keys with correct answers highlighted
- **Grading System**: Grade student responses and calculate scores with detailed feedback
- **File Export**: Save tests to text files
- **Validation**: Built-in validation for questions and answers
- **Test Shuffling**: Shuffle questions and options for different test versions
- **Case-Insensitive Grading**: Accept answers regardless of case

## Project Structure

- `TestPaper.cs` - Main TestPaper class and supporting classes (GradingResult, QuestionResult)
- `Question.cs` - Question class for individual multiple choice questions
- `Program.cs` - Interactive console application demonstrating usage
- `TestRunner.cs` - Comprehensive test suite to verify functionality
- `TestPaperSystem.csproj` - .NET 6.0 project file

## Requirements

- .NET 6.0 SDK or later
- Any C# IDE (Visual Studio, VS Code, Rider, etc.)

## Quick Start

### Building and Running

```bash
# Build the project
dotnet build

# Run the interactive program
dotnet run

# Run the test suite
dotnet run test

# Or run tests directly
dotnet run -- test
```

### Basic Usage

```csharp
using TestPaperSystem;

// Create a new test
var test = new TestPaper(
    title: "C# Basics Quiz",
    subject: "Computer Science", 
    teacherName: "Dr. Smith",
    instructions: "Choose the best answer for each question."
);

// Add questions
test.AddQuestion(
    "What is the correct way to declare a string in C#?",
    new List<string> { "string s;", "String s;", "str s;", "char[] s;" },
    "string s;",
    points: 1
);

test.AddQuestion(
    "Which access modifier makes a member accessible only within the same class?",
    new List<string> { "public", "private", "protected", "internal" },
    "private", 
    points: 2
);

// Display the test
Console.WriteLine(test.DisplayTest());

// Display with answers (for teachers)
Console.WriteLine(test.DisplayTest(showAnswers: true));

// Grade student responses
var studentAnswers = new List<string> { "string s;", "private" };
var results = test.GradeTest(studentAnswers);

Console.WriteLine($"Score: {results.TotalPointsEarned}/{results.TotalPointsPossible} ({results.Percentage}%)");
```

## Class Documentation

### TestPaper Class

The main class for creating and managing multiple choice tests.

#### Constructor
```csharp
public TestPaper(string title, string subject = null, string teacherName = null, string instructions = null)
```

#### Key Methods

- `AddQuestion(string questionText, List<string> options, string correctAnswer, int points = 1)` - Add a new question
- `AddQuestion(Question question)` - Add a pre-created Question object
- `RemoveQuestion(int index)` - Remove a question by index
- `DisplayTest(bool showAnswers = false)` - Generate formatted test display
- `SaveToFile(string filename, bool showAnswers = false)` - Save test to file
- `GradeTest(List<string> studentAnswers)` - Grade student responses
- `GetQuestionCount()` - Get total number of questions
- `TotalPoints` - Property that gets total possible points
- `ShuffleQuestions()` - Randomize question order
- `ShuffleOptions()` - Randomize option order within questions
- `Clone()` - Create a copy of the test

#### Properties

- `Title` - Test title
- `Subject` - Subject name
- `TeacherName` - Teacher's name
- `Instructions` - Special instructions
- `Questions` - Read-only list of questions
- `TotalPoints` - Total possible points (calculated)

### Question Class

Represents individual multiple choice questions.

#### Constructor
```csharp
public Question(string questionText, List<string> options, string correctAnswer, int points = 1)
```

#### Key Methods

- `CheckAnswer(string answer)` - Check if an answer is correct (case-insensitive)
- `ToString(int? questionNumber = null, bool showCorrectAnswer = false)` - Display formatted question
- `GetCorrectAnswerLetter()` - Get the letter (A, B, C, etc.) of the correct answer
- `GetAnswerLetter(string answer)` - Get the letter for any answer option

#### Properties

- `QuestionText` - The question text
- `Options` - List of answer options
- `CorrectAnswer` - The correct answer
- `Points` - Points awarded for correct answer

### GradingResult Class

Contains comprehensive grading information.

#### Properties

- `TotalQuestions` - Number of questions on the test
- `CorrectAnswers` - Number of questions answered correctly
- `TotalPointsEarned` - Points earned by the student
- `TotalPointsPossible` - Maximum possible points
- `Percentage` - Score as a percentage (rounded to 2 decimal places)
- `Details` - List of QuestionResult objects with per-question breakdown

### QuestionResult Class

Contains detailed information about a single question's grading.

#### Properties

- `QuestionNumber` - Question number (1-based)
- `StudentAnswer` - What the student answered
- `CorrectAnswer` - The correct answer
- `CorrectLetter` - Letter (A, B, C, etc.) of the correct answer
- `IsCorrect` - Whether the student was correct
- `PointsEarned` - Points earned for this question
- `PointsPossible` - Points possible for this question

## Interactive Program Features

The main program (`dotnet run`) provides:

1. **View Sample Test** - See a pre-built C# programming test
2. **Create Test Interactively** - Build your own test step by step
3. **View Grading Demo** - See how the grading system works
4. **Run System Tests** - Execute the comprehensive test suite
5. **Exit** - Close the program

## Example Output

### Student Version
```
============================================================
TEST: C# Basics Quiz
Subject: Computer Science
Teacher: Dr. Smith
Total Questions: 2
Total Points: 3
============================================================

INSTRUCTIONS:
Choose the best answer for each question.

QUESTIONS:
----------------------------------------

1. What is the correct way to declare a string in C#? (1 point)
   A) string s;
   B) String s;
   C) str s;
   D) char[] s;

2. Which access modifier makes a member accessible only within the same class? (2 points)
   A) public
   B) private
   C) protected
   D) internal
```

### Teacher Version (with answers)
```
============================================================
TEST: C# Basics Quiz
Subject: Computer Science
Teacher: Dr. Smith
Total Questions: 2
Total Points: 3
============================================================

INSTRUCTIONS:
Choose the best answer for each question.

QUESTIONS:
----------------------------------------

1. What is the correct way to declare a string in C#? (1 point)
→ A) string s;
   B) String s;
   C) str s;
   D) char[] s;

2. Which access modifier makes a member accessible only within the same class? (2 points)
   A) public
→ B) private
   C) protected
   D) internal

============================================================
ANSWER KEY:
--------------------
1. A) string s;
2. B) private
```

## Advanced Features

### Test Variations

```csharp
// Create multiple versions of the same test
var originalTest = CreateMyTest();
var version1 = originalTest.Clone();
var version2 = originalTest.Clone();

// Shuffle for different versions
version1.ShuffleQuestions();
version2.ShuffleOptions();

// Save different versions
version1.SaveToFile("test_version_A.txt");
version2.SaveToFile("test_version_B.txt");
```

### Detailed Grading

```csharp
var results = test.GradeTest(studentAnswers);

Console.WriteLine($"Overall Score: {results.Percentage}%");

foreach (var detail in results.Details)
{
    Console.WriteLine($"Question {detail.QuestionNumber}: " +
        $"{(detail.IsCorrect ? "✓" : "✗")} " +
        $"({detail.PointsEarned}/{detail.PointsPossible} points)");
    
    if (!detail.IsCorrect)
    {
        Console.WriteLine($"  Student answered: {detail.StudentAnswer}");
        Console.WriteLine($"  Correct answer: {detail.CorrectAnswer} ({detail.CorrectLetter})");
    }
}
```

## Error Handling

The system includes comprehensive validation:

- Questions must have at least 2 options
- Correct answer must be one of the provided options
- Points must be positive
- Student answers must match the number of questions
- File operations include proper exception handling

## Testing

The `TestRunner` class provides comprehensive unit tests covering:

- Question creation and validation
- TestPaper functionality
- Grading accuracy
- Edge cases and error conditions
- File operations

Run tests with: `dotnet run test`

## Contributing

This is a sample educational project. Feel free to extend it with additional features such as:

- Different question types (true/false, fill-in-the-blank)
- Time limits for tests
- Database persistence
- Web interface
- Student authentication
- Gradebook integration

## License

This is a sample project for educational purposes.