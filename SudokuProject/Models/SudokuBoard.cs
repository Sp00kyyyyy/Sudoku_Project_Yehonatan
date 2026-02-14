using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.Models
{
    /// <summary>
    /// Represents a Sudoku board with integer cells.
    /// </summary>
    public class SudokuBoard : ISudokuBoard<int>
    {
        private int[,] Board;

        /// <summary>
        /// Gets board width and height.
        /// </summary>
        public int Size { get { return this.Board.GetLength(0); } }

        /// <summary>
        /// Gets or sets a cell value.
        /// </summary>
        public int this[int row, int col]
        {
            get => this.Board[row, col];
            set => this.Board[row, col] = value;
        }

        /// <summary>
        /// Creates an empty board.
        /// </summary>
        /// <param name="size">Board width and height.</param>
        public SudokuBoard(int size)
        {
            this.Board = new int[size, size];
        }

        /// <summary>
        /// Checks if a cell is empty (value 0).
        /// </summary>
        /// <returns>True when the cell is 0; otherwise false.</returns>
        public bool IsEmpty(int row, int col)
        {
            if (Board[row, col].Equals(0))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts the board to a single-line number string.
        /// </summary>
        /// <returns>All cell values without separators.</returns>
        public string ToSimpleString()
        {
            string simpleString = "";
            for (int i = 0; i < this.Board.GetLength(0); i++)
            {
                for (int j = 0; j < this.Board.GetLength(1); j++)
                {
                    simpleString += this.Board[i, j].ToString();
                }
            }
            return simpleString;
        }

        /// <summary>
        /// Builds a text view of the board.
        /// </summary>
        /// <returns>Formatted board text visualization.</returns>
        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder();
            int boxSize = (int)Math.Sqrt(this.Size);

            for (int i = 0; i < this.Size; i++)
            {
                if (i % boxSize == 0 && i != 0)
                {
                    strBuild.AppendLine("---------------------");
                }

                for (int j = 0; j < this.Size; j++)
                {
                    if (j % boxSize == 0 && j != 0)
                    {
                        strBuild.Append("| ");
                    }

                    if (this.Board[i, j] == 0)
                    {
                        strBuild.Append(". ");
                    }
                    else
                    {
                        strBuild.Append($"{this.Board[i, j]} ");
                    }
                }
                strBuild.AppendLine();
            }
            return strBuild.ToString();
        }
    }
}
