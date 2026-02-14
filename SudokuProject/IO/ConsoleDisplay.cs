using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO
{
    /// <summary>
    /// Console implementation of the user interface.
    /// </summary>
    public class ConsoleDisplay : IUserInterface
    {
        /// <summary>
        /// Prints the board to the console.
        /// </summary>
        public void PrintBoard(ISudokuBoard<int> board)
        {
            Console.WriteLine(board.ToString());
        }

        /// <summary>
        /// Prints a message to the console.
        /// </summary>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Asks for and reads one line of input.
        /// </summary>
        /// <returns>User input text.</returns>
        public string GetInput()
        {
            Console.WriteLine("enter the sudoku board string");
            string input = Console.ReadLine();
            return input;
        }
    }
}
