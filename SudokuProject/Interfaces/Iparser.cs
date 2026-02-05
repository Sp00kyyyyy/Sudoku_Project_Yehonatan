using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface IParser<T> where T : IEquatable<T>
    {
        ISudokuBoard<T> ParseInput(string input, int size);
    }
}
