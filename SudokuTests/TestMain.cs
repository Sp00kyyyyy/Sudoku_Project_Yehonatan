using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SudokuProject.IO;
using SudokuProject.IO.InputRules;
using SudokuProject.Interfaces;
using SudokuProject.Logic;

namespace SudokuTests;
internal static class TestMain
{
    private static int failedTests;
    private static void Main()
    {
        var speedFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "testfile.txt");
        var answersFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "correctness_answers.txt");

        RunTest("validation checks", () =>
        {
            var validator = new InputValidator(new List<IInputRule> { new NotNullRule(), new LengthRule(), new AllNumbersRule() });
            var validationCases = new (string Input, bool ExpectedValid, string MessagePart)[]
            {
                ("530070000600195000098000060800060003400803001700020006060000280000419005000080079", true, ""),
                (null, false, "cannot be null"), ("123", false, "expected length"),
                ("53007000060019500009800006080006000340080300170002000606000028000041900500008007a", false, "only numbers")
            };
            for (var caseIndex = 0; caseIndex < validationCases.Length; caseIndex++)
            {
                var validationCase = validationCases[caseIndex];
                var isValid = validator.Validate(9, validationCase.Input, out var message);
                Assert(isValid == validationCase.ExpectedValid, $"validation case {caseIndex + 1} failed");
                if (!validationCase.ExpectedValid) Assert(message.Contains(validationCase.MessagePart), $"validation message mismatch in case {caseIndex + 1}");
            }
            var parsedBoard = new StringParser().ParseInput("123456789" + new string('0', 72), 9);
            Assert(parsedBoard[0, 0] == 1 && parsedBoard[0, 8] == 9 && parsedBoard[8, 8] == 0, "parser failed");
        });

        RunTest("speed check (< 1 second per sudoku)", () =>
        {
            Assert(File.Exists(speedFilePath), $"Missing file: {speedFilePath}");
            var puzzleLines = File.ReadAllLines(speedFilePath);
            var parser = new StringParser();
            var solver = CreateSolver();
            var checkedPuzzles = 0;
            var failedPuzzles = 0;
            for (var lineIndex = 0; lineIndex < puzzleLines.Length; lineIndex++)
            {
                var puzzle = puzzleLines[lineIndex].Trim();
                if (puzzle.Length > 0)
                {
                    checkedPuzzles++;
                    if (puzzle.Length == 81)
                    {
                        var board = parser.ParseInput(puzzle, 9);
                        var timer = Stopwatch.StartNew();
                        var solved = solver.Solve(board);
                        timer.Stop();
                        if (!solved || timer.ElapsedMilliseconds >= 1000) 
                            failedPuzzles++;
                    }
                    else failedPuzzles++;
                }
            }
            Console.WriteLine($"Speed summary: checked={checkedPuzzles}, failed={failedPuzzles}");
            Assert(failedPuzzles == 0, $"{failedPuzzles} puzzles failed speed rule");
        });

        RunTest("correctness check (20 cases)", () =>
        {
            Assert(File.Exists(answersFilePath), $"Missing file: {answersFilePath}");
            var caseLines = File.ReadAllLines(answersFilePath);
            var easyCount = 0;
            var mediumCount = 0;
            var hardCount = 0;
            var totalCases = 0;
            for (var lineIndex = 0; lineIndex < caseLines.Length; lineIndex++)
            {
                var line = caseLines[lineIndex].Trim();
                if (line.Length > 0)
                {
                    var parts = line.Split('|');
                    var difficulty = parts[0];
                    if (difficulty == "easy")
                        easyCount++; 
                    else if (difficulty == "medium") 
                        mediumCount++;
                    else if (difficulty == "hard") 
                        hardCount++;
                    totalCases++;
                    var puzzle = parts[1];
                    var expectedSolution = parts[2];
                    var board = new StringParser().ParseInput(puzzle, 9);
                    var solved = CreateSolver().Solve(board);
                    var solvedBoard = board.ToSimpleString();
                    Assert(solved, $"case {totalCases} ({difficulty}) not solved");
                    Assert(solvedBoard == expectedSolution, $"case {totalCases} ({difficulty}) solution mismatch");
                }
            }
            Console.WriteLine($"Case distribution: easy={easyCount}, medium={mediumCount}, hard={hardCount}");
            Assert(totalCases == 20, $"expected 20 correctness cases, got {totalCases}");
        });

        Console.WriteLine($"TOTAL FAILURES: {failedTests}");
    }

    private static void RunTest(string testName, Action testBody)
    {
        try { testBody(); Console.WriteLine($"PASS: {testName}"); }
        catch (Exception ex) { failedTests++; Console.WriteLine($"FAIL: {testName}"); Console.WriteLine($"  {ex.Message}"); }
    }

    private static Solver CreateSolver()
    {
        IMaskTracker maskTracker = new SudokuForbiddenNumbers();
        return new Solver(new List<ISudokuRule>(), maskTracker, new ObviousMovesFiller(maskTracker), new BoardStateManager(maskTracker));
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
}
