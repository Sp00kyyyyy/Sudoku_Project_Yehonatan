using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO
{
    public class ConsoleDisplay : IUserInterface
    {
        public void PrintBoard(ISudokuBoard<int> board)
        {
            Console.WriteLine(board.ToString());
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string GetInput()
        {
            Console.WriteLine("enter the sudoku board string");
            string input = Console.ReadLine();
            return input;
        }
    }
}
