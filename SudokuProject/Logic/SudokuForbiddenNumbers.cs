using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;


namespace SudokuProject.Logic
{
    public class SudokuForbiddenNumbers : IMaskTracker
    {
        private int[] rowUsedNumbersMask;
        private int[] columnUsedNumbersMask;
        private int[] boxUsedNumbersMask;
        private int[,] cellToBoxIndexLookup;

        public int BoardSize { get; private set; }
        public int BoxSize { get; private set; }
        public int AllNumbersMask { get; private set; }

        public void Initialize(int boardSize)
        {
            this.BoardSize = boardSize;
            this.BoxSize = (int)Math.Sqrt(boardSize);
            this.AllNumbersMask = (1 << boardSize) - 1;

            this.rowUsedNumbersMask = new int[boardSize];
            this.columnUsedNumbersMask = new int[boardSize];
            this.boxUsedNumbersMask = new int[boardSize];
            this.cellToBoxIndexLookup = new int[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    this.cellToBoxIndexLookup[row, col] = (row / this.BoxSize) * this.BoxSize + (col / this.BoxSize);
                }
            }
        }

        public void AddNumberToMasks(int row, int col, int value)
        {
            int numberBit = 1 << (value - 1);
            int boxIndex = this.cellToBoxIndexLookup[row, col];
            this.rowUsedNumbersMask[row] |= numberBit;
            this.columnUsedNumbersMask[col] |= numberBit;
            this.boxUsedNumbersMask[boxIndex] |= numberBit;
        }

        public void SynchronizeMasksWithBoard(ISudokuBoard<int> board)
        {
            Array.Clear(this.rowUsedNumbersMask, 0, this.BoardSize);
            Array.Clear(this.columnUsedNumbersMask, 0, this.BoardSize);
            Array.Clear(this.boxUsedNumbersMask, 0, this.BoardSize);

            for (int row = 0; row < this.BoardSize; row++)
            {
                for (int col = 0; col < this.BoardSize; col++)
                {
                    if (board[row, col] != 0)
                    {
                        AddNumberToMasks(row, col, board[row, col]);
                    }
                }
            }
        }

        public int GetForbiddenNumbers(int row, int col)
        {
            int boxIndex = this.cellToBoxIndexLookup[row, col];
            return this.rowUsedNumbersMask[row] | this.columnUsedNumbersMask[col] | this.boxUsedNumbersMask[boxIndex];
        }

        public int GetAllowedNumbers(int row, int col)
        {
            return ~GetForbiddenNumbers(row, col) & this.AllNumbersMask;
        }

        public int GetRowMask(int row)
        {
            return this.rowUsedNumbersMask[row];
        }

        public int GetColumnMask(int col)
        {
            return this.columnUsedNumbersMask[col];
        }

        public int GetBoxMask(int boxIndex)
        {
            return this.boxUsedNumbersMask[boxIndex];
        }

        public int GetBoxIndex(int row, int col)
        {
            return this.cellToBoxIndexLookup[row, col];
        }

        public (int[], int[], int[]) SaveCurrentMasks()
        {
            int[] savedRowMasks = (int[])this.rowUsedNumbersMask.Clone();
            int[] savedColumnMasks = (int[])this.columnUsedNumbersMask.Clone();
            int[] savedBoxMasks = (int[])this.boxUsedNumbersMask.Clone();
            return (savedRowMasks, savedColumnMasks, savedBoxMasks);
        }

        public void RestoreSavedMasks(int[] rowMask, int[] colMask, int[] boxMask)
        {
            Array.Copy(rowMask, this.rowUsedNumbersMask, this.BoardSize);
            Array.Copy(colMask, this.columnUsedNumbersMask, this.BoardSize);
            Array.Copy(boxMask, this.boxUsedNumbersMask, this.BoardSize);
        }
    }
}

