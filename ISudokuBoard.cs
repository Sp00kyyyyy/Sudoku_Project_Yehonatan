using System;

public interface ISudokuBoard<T> where T : IEquatable<T>
{

    bool IsEmpty(int row, int col);

    T this[int row, int col]
    {
        get;
        set;
    }
    int Size { get; }
    string ToSimpleString();
}


