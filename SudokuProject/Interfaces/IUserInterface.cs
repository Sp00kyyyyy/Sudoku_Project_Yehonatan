using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface IUserInterface
    {
        void PrintBoard(ISudokuBoard<int> board);

        void ShowMessage(string message);

        string GetInput();
    }
}
