using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Fills cells that have an obvious correct value.
    /// </summary>
    public interface IObviousMovesFiller
    {
        /// <summary>
        /// Repeatedly fills all obvious cells.
        /// </summary>
        /// <param name="board">Board to update.</param>
        void FillAllObviousCells(ISudokuBoard<int> board);
    }
}
