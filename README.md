# Multiple Choice Test Paper Creator

This repository contains a comprehensive C# program that allows teachers to create multiple choice tests using a `TestPaper` class.

## Quick Start

```bash
# Build and run the interactive program
dotnet build
dotnet run

# Run the simple example
dotnet run simple

# Run the test suite
dotnet run test
```

## Documentation

See [README_CSharp.md](README_CSharp.md) for complete documentation, examples, and API reference.

## Features

- Create multiple choice tests with customizable titles, subjects, and instructions
- Add, remove, and organize questions with flexible point values
- Generate formatted test papers for students and teachers
- Automatic answer key generation
- Comprehensive grading system with detailed feedback
- File export capabilities
- Input validation and error handling
- Test shuffling for creating different versions

## Files

- `TestPaper.cs` - Main TestPaper class and supporting classes
- `Question.cs` - Question class for individual questions  
- `Program.cs` - Interactive console application
- `SimpleExample.cs` - Basic usage example
- `TestRunner.cs` - Comprehensive test suite
- `TestPaperSystem.csproj` - .NET 6.0 project file

## Basic Usage Example

```csharp
using TestPaperSystem;

// Create a new test
var test = new TestPaper(
    title: "C# Basics Quiz",
    subject: "Computer Science", 
    teacherName: "Dr. Smith"
);

// Add questions
test.AddQuestion(
    "What is the correct way to declare a string in C#?",
    new List<string> { "string s;", "String s;", "str s;", "char[] s;" },
    "string s;",
    points: 1
);

// Display the test
Console.WriteLine(test.DisplayTest());

// Grade student responses
var studentAnswers = new List<string> { "string s;" };
var results = test.GradeTest(studentAnswers);
Console.WriteLine($"Score: {results.Percentage}%");
```

## Requirements

- .NET 6.0 SDK or later
- Any C# IDE (Visual Studio, VS Code, Rider, etc.)

## License

This is a sample project for educational purposes.