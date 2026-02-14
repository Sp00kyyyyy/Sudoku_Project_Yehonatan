using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Logic
{
    /// <summary>
    /// Fills easy Sudoku moves before backtracking.
    /// </summary>
    public class ObviousMovesFiller : IObviousMovesFiller
    {
        private static int[] bitMaskToNumber = new int[1025];
        private static bool bitMaskToNumberInitialized = false;

        private IMaskTracker maskTracker;

        /// <summary>
        /// Creates a filler that uses mask data.
        /// </summary>
        public ObviousMovesFiller(IMaskTracker tracker)
        {
            this.maskTracker = tracker;
            if (bitMaskToNumberInitialized == false)
            {
                InitializeBitMaskToNumber();
            }
        }

        /// <summary>
        /// Builds a lookup table from one-bit mask to number.
        /// </summary>
        private static void InitializeBitMaskToNumber()
        {
            for (int number = 0; number < 9; number++)
            {
                bitMaskToNumber[1 << number] = number + 1;
            }
            bitMaskToNumberInitialized = true;
        }

        /// <summary>
        /// Repeats easy strategies until no new cell is filled.
        /// </summary>
        public void FillAllObviousCells(ISudokuBoard<int> board)
        {
            bool anyChangeMade = true;
            while (anyChangeMade)
            {
                anyChangeMade = false;
                anyChangeMade = FillNakedSingles(board) || anyChangeMade;
                anyChangeMade = FillHiddenSinglesInRows(board) || anyChangeMade;
                anyChangeMade = FillHiddenSinglesInColumns(board) || anyChangeMade;
                anyChangeMade = FillHiddenSinglesInBoxes(board) || anyChangeMade;
            }
        }

        private bool FillNakedSingles(ISudokuBoard<int> board)
        {
            bool foundAnyNakedSingle = false;
            for (int row = 0; row < this.maskTracker.BoardSize; row++)
            {
                for (int col = 0; col < this.maskTracker.BoardSize; col++)
                {
                    bool cellIsEmpty = board[row, col] == 0;
                    if (cellIsEmpty)
                    {
                        int allowedNumbersMask = this.maskTracker.GetAllowedNumbers(row, col);
                        bool onlyOneNumberAllowed = allowedNumbersMask != 0 && (allowedNumbersMask & (allowedNumbersMask - 1)) == 0;

                        if (onlyOneNumberAllowed)
                        {
                            int theOnlyAllowedNumber = bitMaskToNumber[allowedNumbersMask];
                            board[row, col] = theOnlyAllowedNumber;
                            this.maskTracker.AddNumberToMasks(row, col, theOnlyAllowedNumber);
                            foundAnyNakedSingle = true;
                        }
                    }
                }
            }
            return foundAnyNakedSingle;
        }

        private bool FillHiddenSinglesInRows(ISudokuBoard<int> board)
        {
            bool foundAnyHiddenSingle = false;
            for (int row = 0; row < this.maskTracker.BoardSize; row++)
            {
                for (int candidateNumber = 1; candidateNumber <= this.maskTracker.BoardSize; candidateNumber++)
                {
                    int candidateBit = 1 << (candidateNumber - 1);
                    bool numberAlreadyUsedInRow = (this.maskTracker.GetRowMask(row) & candidateBit) != 0;
                    if (numberAlreadyUsedInRow == false)
                    {
                        int validPositionColumn = -1;
                        int validPositionCount = 0;
                        for (int col = 0; col < this.maskTracker.BoardSize; col++)
                        {
                            bool cellIsEmpty = board[row, col] == 0;
                            bool numberAllowedInCell = (this.maskTracker.GetForbiddenNumbers(row, col) & candidateBit) == 0;
                            if (cellIsEmpty && numberAllowedInCell)
                            {
                                validPositionColumn = col;
                                validPositionCount++;
                            }
                            if (validPositionCount > 1)
                            {
                                col = this.maskTracker.BoardSize;
                            }
                        }
                        bool exactlyOneValidPosition = validPositionCount == 1;
                        if (exactlyOneValidPosition)
                        {
                            board[row, validPositionColumn] = candidateNumber;
                            this.maskTracker.AddNumberToMasks(row, validPositionColumn, candidateNumber);
                            foundAnyHiddenSingle = true;
                        }
                    }
                }
            }
            return foundAnyHiddenSingle;
        }

        private bool FillHiddenSinglesInColumns(ISudokuBoard<int> board)
        {
            bool foundAnyHiddenSingle = false;
            for (int col = 0; col < this.maskTracker.BoardSize; col++)
            {
                for (int candidateNumber = 1; candidateNumber <= this.maskTracker.BoardSize; candidateNumber++)
                {
                    int candidateBit = 1 << (candidateNumber - 1);
                    bool numberAlreadyUsedInColumn = (this.maskTracker.GetColumnMask(col) & candidateBit) != 0;
                    if (numberAlreadyUsedInColumn == false)
                    {
                        int validPositionRow = -1;
                        int validPositionCount = 0;

                        for (int row = 0; row < this.maskTracker.BoardSize; row++)
                        {
                            bool cellIsEmpty = board[row, col] == 0;
                            bool numberAllowedInCell = (this.maskTracker.GetForbiddenNumbers(row, col) & candidateBit) == 0;
                            if (cellIsEmpty && numberAllowedInCell)
                            {
                                validPositionRow = row;
                                validPositionCount++;
                            }
                            if (validPositionCount > 1)
                            {
                                row = this.maskTracker.BoardSize;
                            }
                        }
                        bool exactlyOneValidPosition = validPositionCount == 1;
                        if (exactlyOneValidPosition)
                        {
                            board[validPositionRow, col] = candidateNumber;
                            this.maskTracker.AddNumberToMasks(validPositionRow, col, candidateNumber);
                            foundAnyHiddenSingle = true;
                        }
                    }
                }
            }
            return foundAnyHiddenSingle;
        }

        private bool FillHiddenSinglesInBoxes(ISudokuBoard<int> board)
        {
            bool foundAnyHiddenSingle = false;
            for (int boxIndex = 0; boxIndex < this.maskTracker.BoardSize; boxIndex++)
            {
                int boxStartRow = (boxIndex / this.maskTracker.BoxSize) * this.maskTracker.BoxSize;
                int boxStartCol = (boxIndex % this.maskTracker.BoxSize) * this.maskTracker.BoxSize;
                for (int candidateNumber = 1; candidateNumber <= this.maskTracker.BoardSize; candidateNumber++)
                {
                    int candidateBit = 1 << (candidateNumber - 1);
                    bool numberAlreadyUsedInBox = (this.maskTracker.GetBoxMask(boxIndex) & candidateBit) != 0;
                    if (numberAlreadyUsedInBox == false)
                    {
                        int validPositionRow = -1;
                        int validPositionCol = -1;
                        int validPositionCount = 0;
                        for (int row = 0; row < this.maskTracker.BoxSize; row++)
                        {
                            for (int col = 0; col < this.maskTracker.BoxSize; col++)
                            {
                                int currentRow = boxStartRow + row;
                                int currentCol = boxStartCol + col;
                                bool cellIsEmpty = board[currentRow, currentCol] == 0;
                                bool numberAllowedInCell = (this.maskTracker.GetForbiddenNumbers(currentRow, currentCol) & candidateBit) == 0;
                                if (cellIsEmpty && numberAllowedInCell)
                                {
                                    validPositionRow = currentRow;
                                    validPositionCol = currentCol;
                                    validPositionCount++;
                                }
                                if (validPositionCount > 1)
                                {
                                    row = this.maskTracker.BoxSize;
                                    col = this.maskTracker.BoxSize;
                                }
                            }
                        }
                        bool exactlyOneValidPosition = validPositionCount == 1;
                        if (exactlyOneValidPosition)
                        {
                            board[validPositionRow, validPositionCol] = candidateNumber;
                            this.maskTracker.AddNumberToMasks(validPositionRow, validPositionCol, candidateNumber);
                            foundAnyHiddenSingle = true;
                        }
                    }
                }
            }
            return foundAnyHiddenSingle;
        }
    }
}
