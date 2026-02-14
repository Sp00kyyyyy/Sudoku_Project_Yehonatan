using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.Logic
{
    public class BoardStateManager
    {
        private IMaskStateManager maskState;

        public BoardStateManager(IMaskStateManager stateManager)
        {
            this.maskState = stateManager;
        }

        public (int[,], int[], int[], int[]) SaveCompleteState(ISudokuBoard<int> board)
        {
            int boardSize = board.Size;
            int[,] boardSnapshot = new int[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    boardSnapshot[row, col] = board[row, col];
                }
            }

            (int[] savedRowMasks, int[] savedColumnMasks, int[] savedBoxMasks) = this.maskState.SaveCurrentMasks();

            return (boardSnapshot, savedRowMasks, savedColumnMasks, savedBoxMasks);
        }

        public void RestoreCompleteState(ISudokuBoard<int> board, (int[,], int[], int[], int[]) savedState)
        {
            (int[,] boardSnapshot, int[] rowMasks, int[] columnMasks, int[] boxMasks) = savedState;
            int boardSize = board.Size;

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    board[row, col] = boardSnapshot[row, col];
                }
            }

            this.maskState.RestoreSavedMasks(rowMasks, columnMasks, boxMasks);
        }
    }
}
