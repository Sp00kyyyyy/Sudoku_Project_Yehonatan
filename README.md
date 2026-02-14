Sudoku Project
This is Yehonatan Sudoku solver project in C#.

What it does
You paste a Sudoku board as one long string of 81 characters.
Each character is a digit from 0 to 9.
0 means an empty cell.

The program:
Reads the input from the command line
Checks that the input is valid
Turns the string into a 9×9 board
Solves the Sudoku
Prints the solved board
Prints the solved board again as an 81‑character string

If the input is wrong or the Sudoku can’t be solved, the program prints a clear message and keeps running.

Requirements
.NET 8.0 or newer

How to run
Open a terminal in the solution folder.


Main parts of the project
Input and output
ConsoleDisplay / IUserInterface: reads input and prints messages
InputValidator + input rules: checks the raw string
StringParser: converts the string into a SudokuBoard

Core logic
SudokuBoard: stores the board values
SudokuForbiddenNumbers: tracks forbidden numbers using bit masks
ObviousMovesFiller: fills easy moves first (obvious singles)
Solver: solves using bit masks + backtracking

Performance
I use bit masks to make checking allowed numbers very fast.
Before deep backtracking starts, the solver also fills easy obvious moves to reduce the search.

Tests
I added tests for input validation, parsing, and solver correctness using known puzzles and expected answers.
