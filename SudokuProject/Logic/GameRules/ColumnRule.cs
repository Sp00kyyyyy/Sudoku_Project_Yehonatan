using NPOI.SS.Formula.Functions;
using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Logic.GameRules
{
    public class ColumnRule : ISudokuRule
    {
        public bool[,] ColumnRuleList;
        public ColumnRule(int size)
        {
            this.ColumnRuleList = new bool[size, size + 1];
        }
        public void Initialize(ISudokuBoard<int> board)
        {
            Array.Clear(this.ColumnRuleList, 0, this.ColumnRuleList.Length);
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    int num = board[i, j];
                    if (num != 0)
                    {
                        this.ColumnRuleList[j, num] = true;
                    }
                }
            }
        }
        public void Add(int row, int col, int value)
        {
            this.ColumnRuleList[col, value] = true;
        }
        public void Remove(int row, int col, int value)
        {
            this.ColumnRuleList[col, value] = false;
        }
        public bool IsValid(int row, int col, int number)
        {
            if (this.ColumnRuleList[col, number])
            {
                return false;
            }
            return true;
        }
    }
}
