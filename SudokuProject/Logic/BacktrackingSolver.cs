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
        private readonly List<ISudokuRule<int>> LogicRules;
        public BacktrackingSolver(List<ISudokuRule<int>> logicRules)
        {
            this.LogicRules = logicRules;
        }
        private bool IsValidMove(int row, int col, ISudokuBoard<int> board, int number)
        {
            for (int i = 0; i < this.LogicRules.Count; i++)
            {
                if (!(this.LogicRules[i].IsValid(row, col, board, number)))
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
            int minOptions = 10;

            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    int count = 0;
                    if (board.IsEmpty(i, j))
                    {
                        for (int z = 1; z <= 9; z++)
                        {
                            if (IsValidMove(i, j, board, z))
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
        public bool Solve(ISudokuBoard<int> board)
        {
            var index = FindBestCell(board);
            int row = index.Item1;
            int col = index.Item2;
            if (col == -1 || row == -1)
            {
                return true;
            }


            for (int num = 1; num <= 9; num++)
            {
                if (IsValidMove(row, col, board, num))
                {
                    board[row, col] = num;
                    if (Solve(board))
                    {
                        return true;
                    }
                    board[row, col] = 0;
                }
            }
            return false;
        }
    }
}
