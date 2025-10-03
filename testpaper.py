class Question:
    """Represents a multiple choice question."""
    
    def __init__(self, question_text, options, correct_answer, points=1):
        """
        Initialize a multiple choice question.
        
        Args:
            question_text (str): The question text
            options (list): List of answer options (A, B, C, D, etc.)
            correct_answer (str): The correct answer (should match one of the options)
            points (int): Points for this question (default: 1)
        """
        if len(options) < 2:
            raise ValueError("A question must have at least 2 options")
        if correct_answer not in options:
            raise ValueError("Correct answer must be one of the provided options")
        
        self.question_text = question_text
        self.options = options
        self.correct_answer = correct_answer
        self.points = points
    
    def __str__(self):
        """String representation of the question."""
        question_str = f"{self.question_text}\n"
        for i, option in enumerate(self.options):
            letter = chr(65 + i)  # Convert to A, B, C, D, etc.
            question_str += f"  {letter}) {option}\n"
        return question_str
    
    def check_answer(self, answer):
        """Check if the provided answer is correct."""
        return answer == self.correct_answer


class Testpaper:
    """Represents a multiple choice test paper that teachers can create."""
    
    def __init__(self, title, subject=None, teacher_name=None, instructions=None):
        """
        Initialize a test paper.
        
        Args:
            title (str): Title of the test
            subject (str): Subject name (optional)
            teacher_name (str): Name of the teacher creating the test (optional)
            instructions (str): Special instructions for students (optional)
        """
        self.title = title
        self.subject = subject
        self.teacher_name = teacher_name
        self.instructions = instructions
        self.questions = []
        self.total_points = 0
    
    def add_question(self, question_text, options, correct_answer, points=1):
        """
        Add a multiple choice question to the test paper.
        
        Args:
            question_text (str): The question text
            options (list): List of answer options
            correct_answer (str): The correct answer
            points (int): Points for this question (default: 1)
        
        Returns:
            Question: The created question object
        """
        question = Question(question_text, options, correct_answer, points)
        self.questions.append(question)
        self.total_points += points
        return question
    
    def add_question_object(self, question):
        """
        Add a pre-created Question object to the test paper.
        
        Args:
            question (Question): The question object to add
        """
        if not isinstance(question, Question):
            raise TypeError("Expected a Question object")
        
        self.questions.append(question)
        self.total_points += question.points
    
    def remove_question(self, index):
        """
        Remove a question by its index.
        
        Args:
            index (int): Index of the question to remove (0-based)
        
        Returns:
            Question: The removed question object
        """
        if 0 <= index < len(self.questions):
            removed_question = self.questions.pop(index)
            self.total_points -= removed_question.points
            return removed_question
        else:
            raise IndexError("Question index out of range")
    
    def get_question_count(self):
        """Get the total number of questions in the test."""
        return len(self.questions)
    
    def get_total_points(self):
        """Get the total points for the entire test."""
        return self.total_points
    
    def display_test(self, show_answers=False):
        """
        Display the formatted test paper.
        
        Args:
            show_answers (bool): Whether to show correct answers (for teacher's copy)
        
        Returns:
            str: Formatted test paper
        """
        output = []
        output.append("=" * 60)
        output.append(f"TEST: {self.title}")
        if self.subject:
            output.append(f"Subject: {self.subject}")
        if self.teacher_name:
            output.append(f"Teacher: {self.teacher_name}")
        output.append(f"Total Questions: {self.get_question_count()}")
        output.append(f"Total Points: {self.get_total_points()}")
        output.append("=" * 60)
        
        if self.instructions:
            output.append("\nINSTRUCTIONS:")
            output.append(self.instructions)
            output.append("")
        
        output.append("\nQUESTIONS:")
        output.append("-" * 40)
        
        for i, question in enumerate(self.questions, 1):
            output.append(f"\n{i}. {question.question_text} ({question.points} point{'s' if question.points != 1 else ''})")
            
            for j, option in enumerate(question.options):
                letter = chr(65 + j)  # Convert to A, B, C, D, etc.
                marker = "→ " if show_answers and option == question.correct_answer else "   "
                output.append(f"{marker}{letter}) {option}")
        
        if show_answers:
            output.append("\n" + "=" * 60)
            output.append("ANSWER KEY:")
            output.append("-" * 20)
            for i, question in enumerate(self.questions, 1):
                # Find the letter corresponding to the correct answer
                correct_letter = chr(65 + question.options.index(question.correct_answer))
                output.append(f"{i}. {correct_letter}) {question.correct_answer}")
        
        return "\n".join(output)
    
    def save_to_file(self, filename, show_answers=False):
        """
        Save the test paper to a file.
        
        Args:
            filename (str): Name of the file to save to
            show_answers (bool): Whether to include answers in the saved file
        """
        with open(filename, 'w') as f:
            f.write(self.display_test(show_answers))
    
    def grade_test(self, student_answers):
        """
        Grade a student's test answers.
        
        Args:
            student_answers (list): List of student's answers in order
        
        Returns:
            dict: Grading results with score, percentage, and details
        """
        if len(student_answers) != len(self.questions):
            raise ValueError("Number of answers must match number of questions")
        
        correct_count = 0
        total_points_earned = 0
        details = []
        
        for i, (question, answer) in enumerate(zip(self.questions, student_answers), 1):
            is_correct = question.check_answer(answer)
            points_earned = question.points if is_correct else 0
            
            if is_correct:
                correct_count += 1
                total_points_earned += question.points
            
            correct_letter = chr(65 + question.options.index(question.correct_answer))
            details.append({
                'question_number': i,
                'student_answer': answer,
                'correct_answer': question.correct_answer,
                'correct_letter': correct_letter,
                'is_correct': is_correct,
                'points_earned': points_earned,
                'points_possible': question.points
            })
        
        percentage = (total_points_earned / self.total_points * 100) if self.total_points > 0 else 0
        
        return {
            'total_questions': len(self.questions),
            'correct_answers': correct_count,
            'total_points_earned': total_points_earned,
            'total_points_possible': self.total_points,
            'percentage': round(percentage, 2),
            'details': details
        }