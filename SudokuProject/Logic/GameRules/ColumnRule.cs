using NPOI.SS.Formula.Functions;
using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Logic.GameRules
{
    public class CulumnRule : ISudokuRule<int>
    {
        public bool IsValid(int row, int col, ISudokuBoard<int> board, int number)
        {
            for (int i = 0; i < board.Size; i++)
            {
                if (board[i, col] == number && i != row)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
