using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface ISolver<T> where T : IEquatable<T>
    {
        bool Solve(ISudokuBoard<T> board);
    }
}
