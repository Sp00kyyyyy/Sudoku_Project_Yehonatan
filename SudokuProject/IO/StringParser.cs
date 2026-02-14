using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;
using SudokuProject.Models;

namespace SudokuProject.IO
{
    /// <summary>
    /// Converts a text board into a SudokuBoard object.
    /// </summary>
    public class StringParser : IParser<int>
    {
        /// <summary>
        /// Parses input text into board cells from left to right top to bottom.
        /// </summary>
        public ISudokuBoard<int> ParseInput(string input, int size)
        {
            ISudokuBoard<int> board = new SudokuBoard(size);
            int rows = size;
            int cols = size;
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = int.Parse(input[index].ToString());
                    index++;
                }
            }
            return board;
        }
    }
}
