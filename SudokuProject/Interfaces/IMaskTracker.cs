using SudokuProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    public interface IMaskReader
    {
        int GetForbiddenNumbers(int row, int col);
        int GetAllowedNumbers(int row, int col);
        int GetRowMask(int row);
        int GetColumnMask(int col);
        int GetBoxMask(int boxIndex);
        int GetBoxIndex(int row, int col);
        int BoardSize { get; }
        int BoxSize { get; }
        int AllNumbersMask { get; }
    }

    public interface IMaskStateManager
    {
        (int[], int[], int[]) SaveCurrentMasks();
        void RestoreSavedMasks(int[] rowMask, int[] colMask, int[] boxMask);
    }
    public interface IMaskWriter
    {
        void Initialize(int boardSize);
        void AddNumberToMasks(int row, int col, int value);
        void SynchronizeMasksWithBoard(ISudokuBoard<int> board);
    }
    public interface IMaskTracker : IMaskReader, IMaskWriter, IMaskStateManager
    {
    }
}
