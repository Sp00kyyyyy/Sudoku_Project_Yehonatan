using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface ISudokuRule<T> where T : IEquatable<T>
    {
        bool IsValid(int row, int col, ISudokuBoard<T> board, T number);
    }
}
