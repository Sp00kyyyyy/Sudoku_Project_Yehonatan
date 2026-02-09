using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;


namespace SudokuProject.Logic
{
    public class BacktrackingSolver : ISolver<int>
    {
        private static readonly int[] BitToNum = new int[1025];
        private static readonly int[] BitCount = new int[1025];
        private static bool initilaze = false;

        private readonly List<ISudokuRule> Rules;
        private int[] RowMask;
        private int[] ColMask;
        private int[] BoxMask;
        private int[,] BoxLookup;
        private int AllOnesmask;
        private int BoxSize;
        private int BoardSize;

        public BacktrackingSolver(List<ISudokuRule> rules)
        {
            this.Rules = rules;
            if (!initilaze) Initilaze();
        }

        private static void Initilaze()
        {
            for (int i = 0; i < 10; i++) BitToNum[1 << i] = i + 1;
            for (int i = 0; i < 1024; i++)
            {
                int c = 0, n = i;
                while (n > 0) { n &= (n - 1); c++; }
                BitCount[i] = c;
            }
            initilaze = true;
        }

        public bool Solve(ISudokuBoard<int> board)
        {
            BoardSize = board.Size;
            BoxSize = (int)Math.Sqrt(BoardSize);
            AllOnesmask = (1 << BoardSize) - 1;

            RowMask = new int[BoardSize];
            ColMask = new int[BoardSize];
            BoxMask = new int[BoardSize];
            BoxLookup = new int[BoardSize, BoardSize];

            for (int r = 0; r < BoardSize; r++)
                for (int c = 0; c < BoardSize; c++)
                    BoxLookup[r, c] = (r / BoxSize) * BoxSize + (c / BoxSize);

            for (int i = 0; i < Rules.Count; i++) Rules[i].Initialize(board);

            SyncMasks(board);
            FillObviousCells(board);

            return Backtrack(board);
        }

        private void SyncMasks(ISudokuBoard<int> board)
        {
            Array.Clear(RowMask, 0, BoardSize);
            Array.Clear(ColMask, 0, BoardSize);
            Array.Clear(BoxMask, 0, BoardSize);

            for (int r = 0; r < BoardSize; r++)
                for (int c = 0; c < BoardSize; c++)
                    if (board[r, c] != 0)
                        Add(r, c, board[r, c]);
        }

        private void FillObviousCells(ISudokuBoard<int> board)
        {
            bool changed;
            do
            {
                changed = false;
                changed |= NakedSingles(board);
                changed |= HiddenSinglesRows(board);
                changed |= HiddenSinglesCols(board);
                changed |= HiddenSinglesBoxes(board);
            } while (changed);
        }

        private bool NakedSingles(ISudokuBoard<int> board)
        {
            bool found = false;
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int forbidden = RowMask[row] | ColMask[col] | BoxMask[BoxLookup[row, col]];
                        int allowed = ~forbidden & AllOnesmask;
                        if (allowed != 0 && (allowed & (allowed - 1)) == 0)
                        {
                            board[row, col] = BitToNum[allowed];
                            Add(row, col, board[row, col]);
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        private bool HiddenSinglesRows(ISudokuBoard<int> board)
        {
            bool found = false;
            for (int row = 0; row < BoardSize; row++)
            {
                for (int value = 1; value <= BoardSize; value++)
                {
                    int bit = 1 << (value - 1);
                    if ((RowMask[row] & bit) == 0)
                    {
                        int pos = -1, count = 0;
                        for (int col = 0; col < BoardSize && count <= 1; col++)
                        {
                            if (board[row, col] == 0 && ((RowMask[row] | ColMask[col] | BoxMask[BoxLookup[row, col]]) & bit) == 0)
                            {
                                pos = col;
                                count++;
                            }
                        }
                        if (count == 1)
                        {
                            board[row, pos] = value;
                            Add(row, pos, value);
                            found = true;
                        }
                    }
                }
            }
            return found;
        }


        private bool HiddenSinglesCols(ISudokuBoard<int> board)
        {
            bool found = false;
            for (int col = 0; col < BoardSize; col++)
            {
                for (int value = 1; value <= BoardSize; value++)
                {
                    int bit = 1 << (value - 1);
                    if ((ColMask[col] & bit) == 0)
                    {
                        int pos = -1, count = 0;
                        for (int row = 0; row < BoardSize && count <= 1; row++)
                        {
                            if (board[row, col] == 0 && ((RowMask[row] | ColMask[col] | BoxMask[BoxLookup[row, col]]) & bit) == 0)
                            {
                                pos = row;
                                count++;
                            }
                        }
                        if (count == 1)
                        {
                            board[pos, col] = value;
                            Add(pos, col, value);
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        private bool HiddenSinglesBoxes(ISudokuBoard<int> board)
        {
            bool found = false;
            for (int boxIdx = 0; boxIdx < BoardSize; boxIdx++)
            {
                int boxRow = (boxIdx / BoxSize) * BoxSize;
                int boxCol = (boxIdx % BoxSize) * BoxSize;

                for (int value = 1; value <= BoardSize; value++)
                {
                    int bit = 1 << (value - 1);
                    if ((BoxMask[boxIdx] & bit) == 0)
                    {
                        int posRow = -1, posCol = -1, count = 0;
                        for (int innerRow = 0; innerRow < BoxSize && count <= 1; innerRow++)
                        {
                            for (int innerCol = 0; innerCol < BoxSize && count <= 1; innerCol++)
                            {
                                int r = boxRow + innerRow, c = boxCol + innerCol;
                                if (board[r, c] == 0 && ((RowMask[r] | ColMask[c] | BoxMask[boxIdx]) & bit) == 0)
                                {
                                    posRow = r; posCol = c;
                                    count++;
                                }
                            }
                        }
                        if (count == 1)
                        {
                            board[posRow, posCol] = value;
                            Add(posRow, posCol, value);
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        private bool Backtrack(ISudokuBoard<int> board)
        {
            int bestRow = -1, bestCol = -1, bestForbidden = 0;
            int minOptions = BoardSize + 1;

            for (int row = 0; row < BoardSize && minOptions > 1; row++)
            {
                int currentRowMask = RowMask[row];
                for (int col = 0; col < BoardSize && minOptions > 1; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int forbidden = currentRowMask | ColMask[col] | BoxMask[BoxLookup[row, col]];
                        int optionsCount = BoardSize - BitCount[forbidden];

                        if (optionsCount == 0) return false;

                        if (optionsCount < minOptions)
                        {
                            minOptions = optionsCount;
                            bestRow = row;
                            bestCol = col;
                            bestForbidden = forbidden;
                        }
                    }
                }
            }

            if (bestRow == -1) return true;

            int allowedNumbers = ~bestForbidden & AllOnesmask;

            for (int num = BoardSize; num >= 1; num--)
            {
                int numBit = 1 << (num - 1);
                if ((allowedNumbers & numBit) != 0)
                {
                    if (Rules.Count == 0 || IsValid(bestRow, bestCol, num))
                    {
                        int[,] boardState = SaveBoard(board);

                        board[bestRow, bestCol] = num;
                        Add(bestRow, bestCol, num);
                        for (int j = 0; j < Rules.Count; j++) Rules[j].Add(bestRow, bestCol, num);

                        FillObviousCells(board);

                        if (Backtrack(board)) return true;

                        RestoreBoard(board, boardState);
                        SyncMasks(board);

                        for (int j = 0; j < Rules.Count; j++) Rules[j].Remove(bestRow, bestCol, num);
                    }
                }
            }
            return false;
        }


        private int[,] SaveBoard(ISudokuBoard<int> board)
        {
            int[,] boardState = new int[BoardSize, BoardSize];
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    boardState[row, col] = board[row, col];
            return boardState;
        }

        private void RestoreBoard(ISudokuBoard<int> board, int[,] boardState)
        {
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    board[row, col] = boardState[row, col];
        }

        private void Add(int row, int col, int value)
        {
            int bit = 1 << (value - 1);
            int box = BoxLookup[row, col];
            RowMask[row] |= bit;
            ColMask[col] |= bit;
            BoxMask[box] |= bit;
        }

        private bool IsValid(int row, int col, int value)
        {
            for (int i = 0; i < Rules.Count; i++)
                if (!Rules[i].IsValid(row, col, value)) return false;
            return true;
        }
    }
}