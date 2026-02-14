using SudokuProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Defines one Sudoku rule.
    /// </summary>
    public interface ISudokuRule
    {
        /// <summary>
        /// Prepares the rule using the current board.
        /// </summary>
        /// <param name="board">Board used to initialize this rule.</param>
        void Initialize(ISudokuBoard<int> board);

        /// <summary>
        /// Checks if a value is valid in a cell.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <param name="value">Value to test.</param>
        /// <returns>True if the value is valid; otherwise false.</returns>
        bool IsValid(int row, int col, int value);

        /// <summary>
        /// Adds a value to the rule state.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <param name="value">Value to add.</param>
        void Add(int row, int col, int value);

        /// <summary>
        /// Removes a value from the rule state.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <param name="value">Value to remove.</param>
        void Remove(int row, int col, int value);
    }
}
