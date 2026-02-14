using System;
using System.Collections.Generic;
using System.Diagnostics;
using SudokuProject.IO;
using SudokuProject.IO.InputRules;
using SudokuProject.Interfaces;
using SudokuProject.Logic;

namespace SudokuProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var validator = new InputValidator(new List<IInputRule>
            {
                new NotNullRule(),
                new LengthRule(),
                new AllNumbersRule()
            });

            var parser = new StringParser();
            IMaskTracker maskTracker = new SudokuForbiddenNumbers();
            var solver = new Solver(new List<ISudokuRule>(), maskTracker, new ObviousMovesFiller(maskTracker), new BoardStateManager(maskTracker));

            while (true)
            {
                Console.WriteLine("Enter 81-digit Sudoku");
                var input = Console.ReadLine();


                if (!validator.Validate(9, input, out var errorMessage))
                {
                    Console.WriteLine($"Invalid input: {errorMessage}");
                    Console.WriteLine();
                    continue;
                }
                var board = parser.ParseInput(input, 9);
                var timer = Stopwatch.StartNew();
                var solved = solver.Solve(board);
                timer.Stop();

                if (!solved)
                {
                    Console.WriteLine("Unsolvable sudoku");
                    Console.WriteLine($"Time: {timer.ElapsedMilliseconds} ms");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine("Solved board:");
                Console.WriteLine(board.ToString());
                Console.WriteLine($"Solved string: {board.ToSimpleString()}");
                Console.WriteLine($"Time: {timer.ElapsedMilliseconds} ms");
                Console.WriteLine();
            }
        }
    }
}
