using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuProject.Interfaces
{
    /// <summary>
    /// Defines one input validation rule.
    /// </summary>
    public interface IInputRule
    {
        /// <summary>
        /// Checks if an input string passes this rule.
        /// </summary>
        /// <param name="input">Input text to check.</param>
        /// <param name="size">Board size for context.</param>
        /// <param name="errorMessage">Error message when validation fails.</param>
        /// <returns>True if valid; otherwise false.</returns>
        bool RuleValidate(string input, int size, out string errorMessage);
    }
}
