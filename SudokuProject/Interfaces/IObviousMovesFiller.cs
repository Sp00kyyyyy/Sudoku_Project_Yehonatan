using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface IObviousMovesFiller
    {
        void FillAllObviousCells(ISudokuBoard<int> board);
    }
}
