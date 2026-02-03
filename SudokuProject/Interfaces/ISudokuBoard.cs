using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface ISudokuBoard<T> where T : IEquatable<T>
    {
        bool IsEmpty(int row, int col); T this[int row, int col]
        {
            get;
            set;
        }
        int Size
        {
            get;
        }
        string ToSimpleString();
    }
}
