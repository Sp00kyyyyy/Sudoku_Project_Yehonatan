using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Shows messages and reads input from a user.
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        /// Prints the Sudoku board.
        /// </summary>
        /// <param name="board">Board to display.</param>
        void PrintBoard(ISudokuBoard<int> board);

        /// <summary>
        /// Prints a message for the user.
        /// </summary>
        /// <param name="message">Text to show.</param>
        void ShowMessage(string message);

        /// <summary>
        /// Reads user input.
        /// </summary>
        /// <returns>The text entered by the user.</returns>
        string GetInput();
    }
}
