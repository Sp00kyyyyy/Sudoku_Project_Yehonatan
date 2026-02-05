using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO.InputRules
{
    public class LengthRule : IInputRule
    {
        public bool RuleValidate(string input, int size, out string errorMessage)
        {
            errorMessage = "";
            if (input.Length == (size * size))
                return true;
            errorMessage = $"expected length: {size * size}, got: {input.Length}";
            return false;
        }
    }
}
