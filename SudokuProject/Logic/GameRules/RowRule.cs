using NPOI.SS.Formula.Functions;
using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SudokuProject.Logic.GameRules
{
    public class RowRule : ISudokuRule<int>
    {
        public bool IsValid(int row, int col, ISudokuBoard<int> board, int number)
        {
            for (int i = 0; i < board.Size; i++)
            {
                if (board[row, i] == number && i != col)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
