using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;


namespace SudokuProject.Logic
{
    /// <summary>
    /// Tracks forbidden and allowed Sudoku numbers with bit masks.
    /// </summary>
    public class SudokuForbiddenNumbers : IMaskTracker
    {
        private int[] rowUsedNumbersMask;
        private int[] columnUsedNumbersMask;
        private int[] boxUsedNumbersMask;
        private int[,] cellToBoxIndexLookup;

        /// <summary>
        /// Gets board size.
        /// </summary>
        public int BoardSize { get; private set; }

        /// <summary>
        /// Gets one box width/height.
        /// </summary>
        public int BoxSize { get; private set; }

        /// <summary>
        /// Gets a mask where all valid board numbers are valid.
        /// </summary>
        public int AllNumbersMask { get; private set; }

        /// <summary>
        /// Prepares arrays for the given board size
        /// </summary>
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

        /// <summary>
        /// Marks one number as used in row, column, and box
        /// </summary>
        public void AddNumberToMasks(int row, int col, int value)
        {
            int numberBit = 1 << (value - 1);
            int boxIndex = this.cellToBoxIndexLookup[row, col];
            this.rowUsedNumbersMask[row] |= numberBit;
            this.columnUsedNumbersMask[col] |= numberBit;
            this.boxUsedNumbersMask[boxIndex] |= numberBit;
        }

        /// <summary>
        /// Rebuilds masks from values currently placed on the board
        /// </summary>
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

        /// <summary>
        /// Gets numbers that cannot be placed in a cell.
        /// </summary>
        public int GetForbiddenNumbers(int row, int col)
        {
            int boxIndex = this.cellToBoxIndexLookup[row, col];
            return this.rowUsedNumbersMask[row] | this.columnUsedNumbersMask[col] | this.boxUsedNumbersMask[boxIndex];
        }

        /// <summary>
        /// Gets numbers that can be placed in a cell.
        /// </summary>
        public int GetAllowedNumbers(int row, int col)
        {
            return ~GetForbiddenNumbers(row, col) & this.AllNumbersMask;
        }

        /// <summary>
        /// Gets mask for one row.
        /// </summary>
        public int GetRowMask(int row)
        {
            return this.rowUsedNumbersMask[row];
        }

        /// <summary>
        /// Gets mask for one column.
        /// </summary>
        public int GetColumnMask(int col)
        {
            return this.columnUsedNumbersMask[col];
        }

        /// <summary>
        /// Gets mask for one box.
        /// </summary>
        public int GetBoxMask(int boxIndex)
        {
            return this.boxUsedNumbersMask[boxIndex];
        }

        /// <summary>
        /// Gets box index for a cell
        /// </summary>
        public int GetBoxIndex(int row, int col)
        {
            return this.cellToBoxIndexLookup[row, col];
        }

        /// <summary>
        /// Saves current row, column, and box masks
        /// </summary>
        public (int[], int[], int[]) SaveCurrentMasks()
        {
            int[] savedRowMasks = (int[])this.rowUsedNumbersMask.Clone();
            int[] savedColumnMasks = (int[])this.columnUsedNumbersMask.Clone();
            int[] savedBoxMasks = (int[])this.boxUsedNumbersMask.Clone();
            return (savedRowMasks, savedColumnMasks, savedBoxMasks);
        }

        /// <summary>
        /// Restores previously saved masks
        /// </summary>
        public void RestoreSavedMasks(int[] rowMask, int[] colMask, int[] boxMask)
        {
            Array.Copy(rowMask, this.rowUsedNumbersMask, this.BoardSize);
            Array.Copy(colMask, this.columnUsedNumbersMask, this.BoardSize);
            Array.Copy(boxMask, this.boxUsedNumbersMask, this.BoardSize);
        }
    }
}
