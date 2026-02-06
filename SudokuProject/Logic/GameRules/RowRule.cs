using NPOI.SS.Formula.Functions;
using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SudokuProject.Logic.GameRules
{
    public class RowRule : ISudokuRule
    {
        public bool[,] RowRuleList;
        public RowRule(int size)
        {
            this.RowRuleList = new bool[size, size + 1];
        }
        public void Initialize(ISudokuBoard<int> board)
        {
            Array.Clear(this.RowRuleList, 0, this.RowRuleList.Length);
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    int num = board[i, j];
                    if (num != 0)
                    {
                        this.RowRuleList[i, num] = true;
                    }
                }
            }
        }
        public void Add(int row, int col, int value)
        {
            this.RowRuleList[row, value] = true;
        }
        public void Remove(int row, int col, int value)
        {
            this.RowRuleList[row, value] = false;
        }
        public bool IsValid(int row, int col, int number)
        {
            if (this.RowRuleList[row, number])
            {
                return false;
            }
            return true;
        }
    }
}
