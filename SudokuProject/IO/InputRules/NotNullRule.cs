using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO.InputRules
{
    /// <summary>
    /// Checks that input is not null.
    /// </summary>
    public class NotNullRule : IInputRule
    {
        /// <summary>
        /// Validates that input exists.
        /// </summary>
        public bool RuleValidate(string input, int size, out string errorMessage)
        {
            errorMessage = "";
            if (input == null)
            {
                errorMessage = "the input cannot be null";
                return false;
            }
            return true;
        }
    }
}
