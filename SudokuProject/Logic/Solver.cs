using SudokuProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Logic
{
    /// <summary>
    /// Solves Sudoku using obvious moves and backtracking.
    /// </summary>
    public class Solver : ISolver<int>
    {
        private static int[] bitCount = new int[1025];
        private static bool bitCountInitialized = false;

        private List<ISudokuRule> sudokuRules;
        private IMaskTracker maskTracker;
        private IObviousMovesFiller obviousMoves;
        private BoardStateManager stateManager;

        /// <summary>
        /// Creates a solver with all required parameters.
        /// </summary>
        public Solver(List<ISudokuRule> rules, IMaskTracker tracker, IObviousMovesFiller obvious, BoardStateManager boardStateManager)
        {
            this.sudokuRules = rules;
            this.maskTracker = tracker;
            this.obviousMoves = obvious;
            this.stateManager = boardStateManager;

            if (bitCountInitialized == false)
            {
                InitializeBitCount();
            }
        }

        /// <summary>
        /// Builds a lookup table that maps each bitmask to its number of set bits (popcount)
        /// </summary>
        private static void InitializeBitCount()
        {
            for (int mask = 0; mask < 1025; mask++)
            {
                int count = 0;
                int bitsLeft = mask;
                while (bitsLeft > 0)
                {
                    bitsLeft = bitsLeft & (bitsLeft - 1);
                    count++;
                }
                bitCount[mask] = count;
            }
            bitCountInitialized = true;
        }

        /// <summary>
        /// Solves the board if possible.
        /// </summary>
        /// <param name="board">Board to solve.</param>
        /// <returns>True if solved; otherwise false.</returns>
        public bool Solve(ISudokuBoard<int> board)
        {
            this.maskTracker.Initialize(board.Size);

            for (int ruleIndex = 0; ruleIndex < this.sudokuRules.Count; ruleIndex++)
            {
                this.sudokuRules[ruleIndex].Initialize(board);
            }

            this.maskTracker.SynchronizeMasksWithBoard(board);
            this.obviousMoves.FillAllObviousCells(board);

            return SolveUsingBacktracking(board);
        }

        /// <summary>
        /// Uses recursive backtracking to finish the solution.
        /// </summary>
        private bool SolveUsingBacktracking(ISudokuBoard<int> board)
        {
            int bestEmptyCellRow = -1;
            int bestEmptyCellColumn = -1;
            int bestEmptyCellForbiddenMask = 0;
            int minimumOptionsCount = maskTracker.BoardSize + 1;

            for (int row = 0; row < this.maskTracker.BoardSize; row++)
            {
                int currentRowMask = this.maskTracker.GetRowMask(row);
                bool shouldCheckMoreCells = minimumOptionsCount > 1;

                if (shouldCheckMoreCells)
                {
                    for (int col = 0; col < this.maskTracker.BoardSize; col++)
                    {
                        bool cellIsEmpty = board[row, col] == 0;
                        shouldCheckMoreCells = minimumOptionsCount > 1;

                        if (cellIsEmpty && shouldCheckMoreCells)
                        {
                            int forbiddenNumbersMask = this.maskTracker.GetForbiddenNumbers(row, col);
                            int availableOptionsCount = this.maskTracker.BoardSize - bitCount[forbiddenNumbersMask];

                            bool cellHasNoValidOptions = availableOptionsCount == 0;
                            if (cellHasNoValidOptions)
                            {
                                return false;
                            }

                            bool cellHasFewerOptions = availableOptionsCount < minimumOptionsCount;
                            if (cellHasFewerOptions)
                            {
                                minimumOptionsCount = availableOptionsCount;
                                bestEmptyCellRow = row;
                                bestEmptyCellColumn = col;
                                bestEmptyCellForbiddenMask = forbiddenNumbersMask;
                            }
                        }
                    }
                }
            }

            bool allCellsFilled = bestEmptyCellRow == -1;
            if (allCellsFilled)
            {
                return true;
            }

            int allowedNumbersMask = ~bestEmptyCellForbiddenMask & this.maskTracker.AllNumbersMask;

            for (int candidateNumber = this.maskTracker.BoardSize; candidateNumber >= 1; candidateNumber--)
            {
                int candidateNumberBit = 1 << (candidateNumber - 1);
                bool numberIsAllowed = (allowedNumbersMask & candidateNumberBit) != 0;

                if (numberIsAllowed)
                {
                    bool noRulesExist = this.sudokuRules.Count == 0;
                    bool numberPassesAllRules = CheckIfNumberIsValidForAllRules(bestEmptyCellRow, bestEmptyCellColumn, candidateNumber);

                    if (noRulesExist || numberPassesAllRules)
                    {
                        var savedState = this.stateManager.SaveCompleteState(board);

                        board[bestEmptyCellRow, bestEmptyCellColumn] = candidateNumber;
                        maskTracker.AddNumberToMasks(bestEmptyCellRow, bestEmptyCellColumn, candidateNumber);

                        for (int ruleIndex = 0; ruleIndex < this.sudokuRules.Count; ruleIndex++)
                        {
                            this.sudokuRules[ruleIndex].Add(bestEmptyCellRow, bestEmptyCellColumn, candidateNumber);
                        }

                        this.obviousMoves.FillAllObviousCells(board);

                        bool solutionFound = SolveUsingBacktracking(board);
                        if (solutionFound)
                        {
                            return true;
                        }

                        this.stateManager.RestoreCompleteState(board, savedState);

                        for (int ruleIndex = 0; ruleIndex < sudokuRules.Count; ruleIndex++)
                        {
                            sudokuRules[ruleIndex].Remove(bestEmptyCellRow, bestEmptyCellColumn, candidateNumber);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks all rules for one candidate value.
        /// </summary>
        private bool CheckIfNumberIsValidForAllRules(int row, int col, int number)
        {
            for (int ruleIndex = 0; ruleIndex < this.sudokuRules.Count; ruleIndex++)
            {
                bool ruleIsViolated = this.sudokuRules[ruleIndex].IsValid(row, col, number) == false;
                if (ruleIsViolated)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
