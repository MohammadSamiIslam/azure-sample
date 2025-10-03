#!/usr/bin/env python3
"""
Simple test script to verify the Testpaper functionality.
"""

from testpaper import Testpaper, Question


def test_basic_functionality():
    """Test basic functionality of the Testpaper class."""
    
    print("Testing Testpaper functionality...")
    
    # Test 1: Create a basic test
    test = Testpaper("Sample Test", "Math", "Teacher A")
    assert test.title == "Sample Test"
    assert test.subject == "Math"
    assert test.teacher_name == "Teacher A"
    assert test.get_question_count() == 0
    assert test.get_total_points() == 0
    print("✓ Basic test creation works")
    
    # Test 2: Add questions
    test.add_question("What is 2+2?", ["3", "4", "5"], "4", 2)
    assert test.get_question_count() == 1
    assert test.get_total_points() == 2
    print("✓ Adding questions works")
    
    # Test 3: Add more questions
    test.add_question("What is 3*3?", ["6", "9", "12"], "9", 1)
    assert test.get_question_count() == 2
    assert test.get_total_points() == 3
    print("✓ Multiple questions work")
    
    # Test 4: Question validation
    try:
        Question("Invalid question", ["only one option"], "only one option")
        assert False, "Should have raised ValueError"
    except ValueError:
        print("✓ Question validation works")
    
    try:
        Question("Invalid question", ["A", "B"], "C")
        assert False, "Should have raised ValueError"
    except ValueError:
        print("✓ Correct answer validation works")
    
    # Test 5: Test grading
    answers = ["4", "9"]
    results = test.grade_test(answers)
    assert results['correct_answers'] == 2
    assert results['total_points_earned'] == 3
    assert results['percentage'] == 100.0
    print("✓ Perfect score grading works")
    
    # Test 6: Test partial grading
    wrong_answers = ["3", "9"]  # First wrong, second correct
    results = test.grade_test(wrong_answers)
    assert results['correct_answers'] == 1
    assert results['total_points_earned'] == 1
    assert results['percentage'] == 33.33
    print("✓ Partial score grading works")
    
    # Test 7: Test display
    display_output = test.display_test()
    assert "Sample Test" in display_output
    assert "What is 2+2?" in display_output
    print("✓ Test display works")
    
    # Test 8: Test with answers
    display_with_answers = test.display_test(show_answers=True)
    assert "ANSWER KEY:" in display_with_answers
    print("✓ Test display with answers works")
    
    print("\nAll tests passed! ✅")


def demo_usage():
    """Demonstrate typical usage."""
    
    print("\n" + "="*50)
    print("DEMO: Creating a Science Quiz")
    print("="*50)
    
    # Create a science quiz
    quiz = Testpaper(
        title="Basic Science Quiz",
        subject="Science",
        teacher_name="Dr. Johnson",
        instructions="Choose the best answer for each question."
    )
    
    # Add science questions
    quiz.add_question(
        "What is the chemical symbol for water?",
        ["H2O", "CO2", "NaCl", "O2"],
        "H2O",
        points=1
    )
    
    quiz.add_question(
        "Which planet is closest to the Sun?",
        ["Venus", "Earth", "Mercury", "Mars"],
        "Mercury",
        points=1
    )
    
    quiz.add_question(
        "What gas do plants absorb from the atmosphere during photosynthesis?",
        ["Oxygen", "Nitrogen", "Carbon Dioxide", "Hydrogen"],
        "Carbon Dioxide",
        points=2
    )
    
    # Display the quiz
    print("STUDENT VERSION:")
    print(quiz.display_test())
    
    print("\n" + "="*50)
    print("TEACHER VERSION (with answers):")
    print(quiz.display_test(show_answers=True))
    
    # Simulate grading
    print("\n" + "="*50)
    print("GRADING SIMULATION:")
    student_responses = ["H2O", "Venus", "Carbon Dioxide"]  # 2 correct, 1 wrong
    
    results = quiz.grade_test(student_responses)
    print(f"Student scored {results['total_points_earned']}/{results['total_points_possible']} ({results['percentage']}%)")
    
    for detail in results['details']:
        status = "Correct" if detail['is_correct'] else "Incorrect"
        print(f"Q{detail['question_number']}: {status} - Answer: {detail['student_answer']}")


if __name__ == "__main__":
    test_basic_functionality()
    demo_usage()