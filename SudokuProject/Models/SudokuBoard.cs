using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.Models
{
    public class SudokuBoard : ISudokuBoard<int>
    {
        private int[,] Board;
        public int Size { get { return this.Board.GetLength(0); } }

        public int this[int row, int col]
        {
            get => this.Board[row, col];
            set => this.Board[row, col] = value;
        }

        public SudokuBoard(int size)
        {
            this.Board = new int[size, size];
        }


        public bool IsEmpty(int row, int col)
        {
            if (Board[row, col].Equals(0))
            {
                return true;
            }
            return false;
        }

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