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
    /// <summary>
    /// Coordinates input, validation, parsing, solving, and output.
    /// </summary>
    public class ConsoleManager
    {
        private readonly IUserInterface ui;
        private readonly ISolver<int> solver;
        private readonly InputValidator validator;
        private readonly StringParser parser;

        /// <summary>
        /// Creates a console manager
        /// </summary>
        public ConsoleManager(IUserInterface ui, ISolver<int> solver, InputValidator validator, StringParser parser)
        {
            this.ui = ui;
            this.solver = solver;
            this.validator = validator;
            this.parser = parser;
        }

        /// <summary>
        /// Starts an endless loop that reads and solves Sudoku boards
        /// </summary>
        /// <param name="size">Board width and height</param>
        public void Run(int size)
        {
            while (true)
            {
                string input = this.ui.GetInput();
                string message;
                if (this.validator.Validate(size, input, out message))
                {
                    ISudokuBoard<int> board = this.parser.ParseInput(input, size);
                    this.ui.PrintBoard(board);
                    if (this.solver.Solve(board))
                    {
                        ui.PrintBoard(board);
                        ui.ShowMessage(board.ToSimpleString());
                    }
                    else
                    {
                        ui.ShowMessage("unsolvable sudoku");
                    }
                }
                else
                {
                    ui.ShowMessage(message);
                }
            }
        }
    }
}
