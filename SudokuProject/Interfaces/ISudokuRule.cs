using SudokuProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface ISudokuRule
    {
        void Initialize(ISudokuBoard<int> board);
        bool IsValid(int row, int col, int value);
        void Add(int row, int col, int value);
        void Remove(int row, int col, int value);
    }
}
