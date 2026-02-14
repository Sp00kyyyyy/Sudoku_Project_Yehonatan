using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface IMaskTracker
    {
        void Initialize(int boardSize);
        void AddNumberToMasks(int row, int col, int value);
        void SynchronizeMasksWithBoard(ISudokuBoard<int> board);
        int GetForbiddenNumbers(int row, int col);
        int GetAllowedNumbers(int row, int col);
        int GetRowMask(int row);
        int GetColumnMask(int col);
        int GetBoxMask(int boxIndex);
        int GetBoxIndex(int row, int col);
        int BoardSize { get; }
        int BoxSize { get; }
        int AllNumbersMask { get; }
        (int[], int[], int[]) SaveCurrentMasks();
        void RestoreSavedMasks(int[] rowMask, int[] colMask, int[] boxMask);
    }
}
