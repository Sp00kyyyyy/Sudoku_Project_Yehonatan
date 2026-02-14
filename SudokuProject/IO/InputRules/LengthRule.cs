using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO.InputRules
{
    /// <summary>
    /// Checks that input length matches board size.
    /// </summary>
    public class LengthRule : IInputRule
    {
        /// <summary>
        /// Validates that input has exactly size * size characters and that the root of size is a whole number.
        /// </summary>
        public bool RuleValidate(string input, int size, out string errorMessage)
        {
            errorMessage = "";
            if (input.Length == (size * size))
            {
                if (Math.Sqrt(size) % 1 == 0)
                {
                    return true;
                }
                else
                {
                    errorMessage = $"square of size should be whole number but instead got: {size}";
                }
            }
            else
                errorMessage = $"expected length: {size * size}, got: {input.Length}";
            
            return false;
        }
    }
}
