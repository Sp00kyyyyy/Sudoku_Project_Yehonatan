using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;
using SudokuProject.Models;

namespace SudokuProject.IO
{
    public class StringParser : IParser<int>
    {
        public ISudokuBoard<int> ParseInput(string input, int size)
        {
            SudokuBoard board = new SudokuBoard(size);
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
