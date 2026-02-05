using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;

namespace SudokuProject.IO.InputRules
{
    public class AllNumbersRule : IInputRule
    {
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
