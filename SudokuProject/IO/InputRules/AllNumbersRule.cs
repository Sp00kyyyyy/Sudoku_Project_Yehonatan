using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO.InputRules
{
    /// <summary>
    /// Checks that all input characters are digits.
    /// </summary>
    public class AllNumbersRule : IInputRule
    {
        /// <summary>
        /// Validates that input contains only numbers.
        /// </summary>
        public bool RuleValidate(string input, int size, out string errorMessage)
        {
            errorMessage = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (!(char.IsDigit(input[i])))
                {
                    errorMessage = "the input must be only numbers";
                    return false;
                }
            }
            return true;
        }
    }
}
