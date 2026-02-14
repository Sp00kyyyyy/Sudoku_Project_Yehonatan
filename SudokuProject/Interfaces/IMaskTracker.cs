using SudokuProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Reads mask values that describe used and allowed numbers.
    /// </summary>
    public interface IMaskReader
    {
        /// <summary>
        /// Gets a bit mask of numbers that cannot be used in a cell.
        /// </summary>
        int GetForbiddenNumbers(int row, int col);

        /// <summary>
        /// Gets a bit mask of numbers that can be used in a cell.
        /// </summary>
        int GetAllowedNumbers(int row, int col);

        /// <summary>
        /// Gets used numbers for one row.
        /// </summary>
        int GetRowMask(int row);

        /// <summary>
        /// Gets used numbers for one column.
        /// </summary>
        int GetColumnMask(int col);

        /// <summary>
        /// Gets used numbers for one box.
        /// </summary>
        int GetBoxMask(int boxIndex);

        /// <summary>
        /// Gets the box index for a cell.
        /// </summary>
        int GetBoxIndex(int row, int col);

        /// <summary>
        /// Gets the board size.
        /// </summary>
        int BoardSize { get; }

        /// <summary>
        /// Gets the width of one box.
        /// </summary>
        int BoxSize { get; }

        /// <summary>
        /// Gets a mask with all valid numbers turned on.
        /// </summary>
        int AllNumbersMask { get; }
    }

    /// <summary>
    /// Saves and restores mask arrays.
    /// </summary>
    public interface IMaskStateManager
    {
        /// <summary>
        /// Saves current row, column, and box masks.
        /// </summary>
        /// <returns>A tuple with saved mask arrays.</returns>
        (int[], int[], int[]) SaveCurrentMasks();

        /// <summary>
        /// Restores previously saved masks.
        /// </summary>
        void RestoreSavedMasks(int[] rowMask, int[] colMask, int[] boxMask);
    }

    /// <summary>
    /// Writes or updates mask values.
    /// </summary>
    public interface IMaskWriter
    {
        /// <summary>
        /// Creates mask structures for a board size.
        /// </summary>
        void Initialize(int boardSize);

        /// <summary>
        /// Marks one value as used in row, column, and box masks.
        /// </summary>
        void AddNumberToMasks(int row, int col, int value);

        /// <summary>
        /// Rebuilds masks from all values currently on the board.
        /// </summary>
        void SynchronizeMasksWithBoard(ISudokuBoard<int> board);
    }

    /// <summary>
    /// Unites the 3 interfaces from above to form a large contract.
    /// </summary>
    public interface IMaskTracker : IMaskReader, IMaskWriter, IMaskStateManager
    {
    }
}
