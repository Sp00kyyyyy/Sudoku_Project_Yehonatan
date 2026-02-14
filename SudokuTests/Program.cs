using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SudokuProject.IO;
using SudokuProject.IO.InputRules;
using SudokuProject.Interfaces;
using SudokuProject.Logic;

namespace SudokuTests;

internal static class Program
{
    private static int failures = 0;

    private static void Main()
    {
        RunTest("InputValidator accepts correct sudoku string", TestInputValidatorAcceptsCorrectString);
        RunTest("InputValidator rejects null", TestInputValidatorRejectsNull);
        RunTest("InputValidator rejects wrong length", TestInputValidatorRejectsWrongLength);
        RunTest("InputValidator rejects non-digit characters", TestInputValidatorRejectsNonDigitCharacters);
        RunTest("Parser maps sudoku string to board correctly", TestParserParsesCorrectly);
        RunTest("Solver solves a known valid board", TestSolverSolvesKnownBoard);
        RunTest("Solver marks unsolvable board as unsolvable", TestSolverRejectsUnsolvableBoard);
        RunTest("Solver solves each board in Downloads/testfile.txt under 1 second", TestSolverPerformanceOnDownloadFile);

        Console.WriteLine();
        Console.WriteLine($"Failures: {failures}");
        Environment.ExitCode = failures == 0 ? 0 : 1;
    }

    private static void RunTest(string name, Action test)
    {
        try
        {
            test();
            Console.WriteLine($"PASS: {name}");
        }
        catch (Exception ex)
        {
            failures++;
            Console.WriteLine($"FAIL: {name}");
            Console.WriteLine($"  {ex.Message}");
        }
    }

    private static InputValidator CreateValidator()
    {
        return new InputValidator(new List<IInputRule>
        {
            new NotNullRule(),
            new LengthRule(),
            new AllNumbersRule()
        });
    }

    private static Solver CreateSolver()
    {
        IMaskTracker tracker = new SudokuForbiddenNumbers();
        BoardStateManager state = new BoardStateManager(tracker);
        IObviousMovesFiller obvious = new ObviousMovesFiller(tracker);
        return new Solver(new List<ISudokuRule>(), tracker, obvious, state);
    }

    private static void TestInputValidatorAcceptsCorrectString()
    {
        var validator = CreateValidator();
        var valid = validator.Validate(9, "530070000600195000098000060800060003400803001700020006060000280000419005000080079", out var message);

        AssertTrue(valid, "Expected validator to accept a valid sudoku string.");
        AssertTrue(string.IsNullOrEmpty(message), "Expected no error message for valid input.");
    }

    private static void TestInputValidatorRejectsNull()
    {
        var validator = CreateValidator();
        var valid = validator.Validate(9, null, out var message);

        AssertTrue(valid == false, "Expected validator to reject null input.");
        AssertTrue(message.Contains("cannot be null"), "Expected null error message.");
    }

    private static void TestInputValidatorRejectsWrongLength()
    {
        var validator = CreateValidator();
        var valid = validator.Validate(9, "123", out var message);

        AssertTrue(valid == false, "Expected validator to reject wrong length.");
        AssertTrue(message.Contains("expected length"), "Expected length error message.");
    }

    private static void TestInputValidatorRejectsNonDigitCharacters()
    {
        var validator = CreateValidator();
        var valid = validator.Validate(9, "53007000060019500009800006080006000340080300170002000606000028000041900500008007a", out var message);

        AssertTrue(valid == false, "Expected validator to reject non-digit characters.");
        AssertTrue(message.Contains("only numbers"), "Expected numeric-only error message.");
    }

    private static void TestParserParsesCorrectly()
    {
        var parser = new StringParser();
        var board = parser.ParseInput("123456789" + new string('0', 72), 9);

        AssertEqual(1, board[0, 0], "Expected board[0,0] to be 1.");
        AssertEqual(9, board[0, 8], "Expected board[0,8] to be 9.");
        AssertEqual(0, board[8, 8], "Expected board[8,8] to be 0.");
    }

    private static void TestSolverSolvesKnownBoard()
    {
        var parser = new StringParser();
        var solver = CreateSolver();

        var puzzle = "530070000600195000098000060800060003400803001700020006060000280000419005000080079";
        var expected = "534678912672195348198342567859761423426853791713924856961537284287419635345286179";

        var board = parser.ParseInput(puzzle, 9);
        var solved = solver.Solve(board);

        AssertTrue(solved, "Expected solver to solve a valid board.");
        AssertEqual(expected, board.ToSimpleString(), "Solver output did not match expected solution.");
    }

    private static void TestSolverRejectsUnsolvableBoard()
    {
        var parser = new StringParser();
        var solver = CreateSolver();

        var unsolvable = "530570000600195000098000060800060003400803001700020006060000280000419005000080079";
        var board = parser.ParseInput(unsolvable, 9);

        var solved = solver.Solve(board);

        AssertTrue(solved == false, "Expected solver to return false for an unsolvable board.");
    }

    private static void TestSolverPerformanceOnDownloadFile()
    {
        var filePath = @"C:\Users\Lenovo\Downloads\testfile.txt";
        AssertTrue(File.Exists(filePath), "Expected dataset file at C:\\Users\\Lenovo\\Downloads\\testfile.txt.");

        var parser = new StringParser();
        var solver = CreateSolver();
        var lines = File.ReadAllLines(filePath);

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.Length == 0)
            {
                continue;
            }

            AssertTrue(line.Length == 81, $"Line {i + 1} has length {line.Length}, expected 81.");

            var board = parser.ParseInput(line, 9);
            var stopwatch = Stopwatch.StartNew();
            var solved = solver.Solve(board);
            stopwatch.Stop();

            AssertTrue(solved, $"Solver failed on line {i + 1}.");
            AssertTrue(stopwatch.ElapsedMilliseconds < 1000,
                $"Line {i + 1} took {stopwatch.ElapsedMilliseconds} ms (must be under 1000 ms)."
            );
        }
    }

    private static void AssertTrue(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception(message);
        }
    }

    private static void AssertEqual<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new Exception($"{message} Expected: {expected}. Actual: {actual}.");
        }
    }
}
