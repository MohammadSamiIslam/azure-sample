#!/usr/bin/env python3
"""
Example usage of the Testpaper class for creating multiple choice tests.
This demonstrates how teachers can create and manage test papers.
"""

from testpaper import Testpaper, Question


def create_sample_test():
    """Create a sample test to demonstrate the functionality."""
    
    # Create a new test paper
    test = Testpaper(
        title="Introduction to Python Programming",
        subject="Computer Science",
        teacher_name="Dr. Smith",
        instructions="Choose the best answer for each question. Mark your answers clearly."
    )
    
    # Add questions using the add_question method
    test.add_question(
        "What is the output of print(2 ** 3)?",
        ["6", "8", "9", "12"],
        "8",
        points=2
    )
    
    test.add_question(
        "Which of the following is NOT a valid Python data type?",
        ["int", "float", "char", "str"],
        "char",
        points=1
    )
    
    test.add_question(
        "What does the len() function return?",
        ["The last element of a sequence", "The number of elements in a sequence", "The first element of a sequence", "The middle element of a sequence"],
        "The number of elements in a sequence",
        points=1
    )
    
    test.add_question(
        "Which operator is used for floor division in Python?",
        ["/", "//", "%", "**"],
        "//",
        points=1
    )
    
    # You can also create Question objects separately and add them
    bonus_question = Question(
        "What is the time complexity of list.append() in Python?",
        ["O(1)", "O(n)", "O(log n)", "O(n²)"],
        "O(1)",
        points=3
    )
    test.add_question_object(bonus_question)
    
    return test


def interactive_test_creation():
    """Allow a teacher to interactively create a test."""
    
    print("=== Multiple Choice Test Creator ===\n")
    
    # Get test details
    title = input("Enter test title: ")
    subject = input("Enter subject (optional): ") or None
    teacher_name = input("Enter your name (optional): ") or None
    instructions = input("Enter special instructions (optional): ") or None
    
    # Create the test paper
    test = Testpaper(title, subject, teacher_name, instructions)
    
    print(f"\nCreated test: {title}")
    print("Now let's add some questions...\n")
    
    question_count = 1
    while True:
        print(f"--- Question {question_count} ---")
        question_text = input("Enter question text (or 'done' to finish): ")
        
        if question_text.lower() == 'done':
            break
        
        # Get options
        options = []
        option_letters = ['A', 'B', 'C', 'D', 'E', 'F']
        
        print("Enter answer options (minimum 2, press Enter on empty line to finish):")
        for i, letter in enumerate(option_letters):
            option = input(f"Option {letter}: ")
            if not option.strip():
                break
            options.append(option.strip())
        
        if len(options) < 2:
            print("Error: Need at least 2 options. Please try again.")
            continue
        
        # Get correct answer
        print(f"Available options: {options}")
        while True:
            correct_answer = input("Enter the correct answer (exactly as typed above): ")
            if correct_answer in options:
                break
            print("Error: Correct answer must match one of the options exactly.")
        
        # Get points
        while True:
            try:
                points = input("Enter points for this question (default 1): ") or "1"
                points = int(points)
                if points > 0:
                    break
                print("Points must be a positive number.")
            except ValueError:
                print("Please enter a valid number.")
        
        # Add the question
        test.add_question(question_text, options, correct_answer, points)
        print(f"Question {question_count} added successfully!\n")
        question_count += 1
    
    return test


def demonstrate_grading():
    """Demonstrate the grading functionality."""
    
    # Create a simple test
    test = Testpaper("Quick Math Quiz", "Mathematics", "Ms. Johnson")
    test.add_question("What is 2 + 2?", ["3", "4", "5", "6"], "4")
    test.add_question("What is 5 * 3?", ["13", "15", "17", "20"], "15")
    test.add_question("What is 10 / 2?", ["4", "5", "6", "7"], "5")
    
    print("=== Grading Demo ===")
    print("Test Questions:")
    print(test.display_test())
    
    # Simulate student answers
    student_answers = ["4", "15", "6"]  # Two correct, one wrong
    
    print("\nStudent Answers:", student_answers)
    
    # Grade the test
    results = test.grade_test(student_answers)
    
    print(f"\n=== GRADING RESULTS ===")
    print(f"Total Questions: {results['total_questions']}")
    print(f"Correct Answers: {results['correct_answers']}")
    print(f"Score: {results['total_points_earned']}/{results['total_points_possible']}")
    print(f"Percentage: {results['percentage']}%")
    
    print("\nDetailed Results:")
    for detail in results['details']:
        status = "✓" if detail['is_correct'] else "✗"
        print(f"  Q{detail['question_number']}: {status} "
              f"Student: {detail['student_answer']}, "
              f"Correct: {detail['correct_answer']} ({detail['correct_letter']})")


def main():
    """Main function to demonstrate the test creation system."""
    
    print("Welcome to the Multiple Choice Test Creation System!")
    print("This program demonstrates how teachers can create tests using the Testpaper class.\n")
    
    while True:
        print("Choose an option:")
        print("1. View sample test")
        print("2. Create test interactively")
        print("3. View grading demo")
        print("4. Exit")
        
        choice = input("\nEnter your choice (1-4): ").strip()
        
        if choice == "1":
            print("\n" + "="*50)
            test = create_sample_test()
            print("STUDENT VERSION:")
            print(test.display_test(show_answers=False))
            
            print("\n" + "="*50)
            print("TEACHER VERSION (with answers):")
            print(test.display_test(show_answers=True))
            
            # Save to files
            test.save_to_file("sample_test_student.txt", show_answers=False)
            test.save_to_file("sample_test_teacher.txt", show_answers=True)
            print("\nTest saved to 'sample_test_student.txt' and 'sample_test_teacher.txt'")
            
        elif choice == "2":
            print("\n" + "="*50)
            test = interactive_test_creation()
            
            if test.get_question_count() > 0:
                print("\n" + "="*50)
                print("Your completed test:")
                print(test.display_test(show_answers=True))
                
                save = input("\nSave test to file? (y/n): ").lower()
                if save == 'y':
                    filename = input("Enter filename (without extension): ")
                    test.save_to_file(f"{filename}_student.txt", show_answers=False)
                    test.save_to_file(f"{filename}_teacher.txt", show_answers=True)
                    print(f"Test saved to '{filename}_student.txt' and '{filename}_teacher.txt'")
            else:
                print("No questions were added to the test.")
        
        elif choice == "3":
            print("\n" + "="*50)
            demonstrate_grading()
        
        elif choice == "4":
            print("Thank you for using the Test Creation System!")
            break
        
        else:
            print("Invalid choice. Please try again.")
        
        print("\n" + "-"*50 + "\n")


if __name__ == "__main__":
    main()