using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.Logic
{
    public class BacktrackingSolver : ISolver<int>
    {
        private readonly List<ISudokuRule> LogicRules;
        public BacktrackingSolver(List<ISudokuRule> logicRules)
        {
            this.LogicRules = logicRules;
        }
        public void Initialize(ISudokuBoard<int> board)
        {
            for (int i = 0; i < this.LogicRules.Count; i++)
            {
                this.LogicRules[i].Initialize(board);
            }
        }
        private bool IsValidMove(int row, int col, int number)
        {
            for (int i = 0; i < this.LogicRules.Count; i++)
            {
                if (!(this.LogicRules[i].IsValid(row, col, number)))
                {
                    return false;
                }
            }
            return true;
        }
        private (int row, int col) FindBestCell(ISudokuBoard<int> board)
        {
            int bestcol = -1;
            int bestRow = -1;
            int minOptions = board.Size + 1;

            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    int count = 0;
                    if (board.IsEmpty(i, j))
                    {
                        for (int z = 1; z <= board.Size; z++)
                        {
                            if (IsValidMove(i, j, z))
                            {
                                count++;
                            }
                        }
                        if (count < minOptions)
                        {
                            minOptions = count;
                            bestcol = j;
                            bestRow = i;
                        }
                        if (minOptions == 1)
                            return (bestRow, bestcol);
                    }
                }
            }
            return (bestRow, bestcol);
        }
        public bool RecursiveSolve(ISudokuBoard<int> board)
        {
            var index = FindBestCell(board);
            int row = index.Item1;
            int col = index.Item2;
            if (col == -1 || row == -1)
            {
                return true;
            }


            for (int num = 1; num <= board.Size; num++)
            {
                if (IsValidMove(row, col, num))
                {
                    for (int i = 0; i < this.LogicRules.Count; i++)
                    {
                        this.LogicRules[i].Add(row, col, num);
                    }
                    board[row, col] = num;
                    if (RecursiveSolve(board))
                    {
                        return true;
                    }
                    for (int i = 0; i < this.LogicRules.Count; i++)
                    {
                        this.LogicRules[i].Remove(row, col, num);
                    }
                    board[row, col] = 0;
                }
            }
            return false;
        }
        public bool Solve(ISudokuBoard<int> board)
        {
            Initialize(board);
            return RecursiveSolve(board);
        }
    }
}
