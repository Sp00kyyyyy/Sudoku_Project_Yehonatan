using NPOI.SS.Formula.Functions;
using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Logic.GameRules
{
    public class BoxRule : ISudokuRule<int>
    {
        public bool IsValid(int row, int col, ISudokuBoard<int> board, int number)
        {
            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (board[i, j] == number && !(i == row && j == col))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
