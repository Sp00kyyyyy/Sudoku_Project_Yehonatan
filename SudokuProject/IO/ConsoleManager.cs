using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;
using SudokuProject.Models;
using SudokuProject.Logic;
using System.Runtime.CompilerServices;

namespace SudokuProject.IO
{
    public class ConsoleManager
    {
        private readonly IUserInterface ui;
        private readonly ISolver<int> solver;
        private readonly InputValidator validator;
        private readonly StringParser parser;

        public ConsoleManager(IUserInterface ui, ISolver<int> solver, InputValidator validator, StringParser parser)
        {
            this.ui = ui;
            this.solver = solver;
            this.validator = validator;
            this.parser = parser;
        }

        public void Run(int size)
        {
            while (true)
            {
                string input = this.ui.GetInput();
                string massage;
                if (this.validator.Validate(size, input, out massage))
                {
                    ISudokuBoard<int> board = this.parser.ParseInput(input, size);
                    this.ui.PrintBoard(board);
                    if (this.solver.Solve(board))
                    {
                        ui.PrintBoard(board);
                        Console.WriteLine(board.ToSimpleString());
                    }
                    else
                        ui.ShowMessage("unsolvable sudoku");
                }
                else
                    Console.WriteLine(massage);
            }

        }
    }
}
