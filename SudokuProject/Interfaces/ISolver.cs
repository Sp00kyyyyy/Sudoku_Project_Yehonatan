using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Solves a Sudoku board.
    /// </summary>
    public interface ISolver<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Tries to solve the given board.
        /// </summary>
        /// <param name="board">Board to solve.</param>
        /// <returns>True if a solution was found, otherwise false.</returns>
        bool Solve(ISudokuBoard<T> board);
    }
}
