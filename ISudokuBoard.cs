using System;

/// <summary>
/// Represents a Sudoku board.
/// </summary>
/// <typeparam name="T">Type of each board value.</typeparam>
public interface ISudokuBoard<T> where T : IEquatable<T>
{
    /// <summary>
    /// Checks if a cell is empty.
    /// </summary>
    /// <returns>True if the cell is empty; otherwise false.</returns>
    bool IsEmpty(int row, int col);

    /// <summary>
    /// Gets or sets a value in the board.
    /// </summary>
    T this[int row, int col]
    {
        get;
        set;
    }

    /// <summary>
    /// Gets board width and height.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// Converts the board to one flat string.
    /// </summary>
    /// <returns>Board values as a single string.</returns>
    string ToSimpleString();
}
