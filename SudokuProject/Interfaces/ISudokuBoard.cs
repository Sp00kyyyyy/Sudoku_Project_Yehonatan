using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Represents a Sudoku board.
    /// </summary>
    public interface ISudokuBoard<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Checks if a cell is empty.
        /// </summary>
        /// <param name="row">Row index of the cell.</param>
        /// <param name="col">Column index of the cell.</param>
        /// <returns>True if the cell is empty; otherwise false.</returns>
        bool IsEmpty(int row, int col);

        /// <summary>
        /// Gets or sets a value in the board.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <param name="col">Column index.</param>
        /// <returns>Value stored at the given position.</returns>
        T this[int row, int col]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the board size.
        /// </summary>
        int Size
        {
            get;
        }

        /// <summary>
        /// Converts the board to one flat string.
        /// </summary>
        /// <returns>Board values as a single string.</returns>
        string ToSimpleString();
    }
}
