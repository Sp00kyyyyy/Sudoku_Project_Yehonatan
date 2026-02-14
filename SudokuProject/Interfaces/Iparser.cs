using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Converts text input into a Sudoku board.
    /// </summary>
    public interface IParser<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Parses text into a board object.
        /// </summary>
        /// <param name="input">text representation of the board.</param>
        /// <param name="size">Board width and height.</param>
        /// <returns>A parsed Sudoku board.</returns>
        ISudokuBoard<T> ParseInput(string input, int size);
    }
}
