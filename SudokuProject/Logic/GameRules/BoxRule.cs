using NPOI.SS.Formula.Functions;
using NPOI.XSSF.Streaming.Values;
using Org.BouncyCastle.Utilities;
using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Logic.GameRules
{
    public class BoxRule : ISudokuRule
    {
        public bool[,] BoxRuleList;
        private int GridLength;
        public BoxRule(int size)
        {
            this.BoxRuleList = new bool[size, size + 1];
            this.GridLength = (int)Math.Sqrt(size);
        }
        public void Initialize(ISudokuBoard<int> board)
        {
            Array.Clear(this.BoxRuleList, 0, this.BoxRuleList.Length);
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    int num = board[i, j];
                    if (num != 0)
                    {
                        int boxIndex = (i / this.GridLength) * this.GridLength + (j / this.GridLength);
                        this.BoxRuleList[boxIndex, num] = true;
                    }
                }
            }
        }
        public void Add(int row, int col, int value)
        {
            int boxIndex = (row / this.GridLength) * this.GridLength + (col / this.GridLength);
            this.BoxRuleList[boxIndex, value] = true;
        }
        public void Remove(int row, int col, int value)
        {
            int boxIndex = (row / this.GridLength) * this.GridLength + (col / this.GridLength);
            this.BoxRuleList[boxIndex, value] = false;
        }
        public bool IsValid(int row, int col, int number)
        {
            int boxIndex = (row / this.GridLength) * this.GridLength + (col / this.GridLength);
            if (this.BoxRuleList[boxIndex, number])
            {
                return false;
            }
            return true;
        }
    }
}
