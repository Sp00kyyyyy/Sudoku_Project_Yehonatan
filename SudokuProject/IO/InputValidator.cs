using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuProject.Interfaces;
using SudokuProject.IO.InputRules;

namespace SudokuProject.IO
{
    /// <summary>
    /// Validates input by running a list of rules.
    /// </summary>
    public class InputValidator
    {
        private readonly List<IInputRule> RulesList;

        /// <summary>
        /// Creates a validator with initial rules.
        /// </summary>
        public InputValidator(List<IInputRule> rules)
        {
            this.RulesList = rules;
        }

        /// <summary>
        /// Adds one more validation rule.
        /// </summary>
        public void AddRule(IInputRule rule)
        {
            this.RulesList.Add(rule);
        }

        /// <summary>
        /// Runs all rules against the input.
        /// </summary>
        /// <returns>True if all rules pass; otherwise false.</returns>
        public bool Validate(int size, string input, out string errorMessage)
        {
            List<IInputRule> ruleList = this.RulesList;
            errorMessage = "";
            for (int i = 0; i < ruleList.Count; i++)
            {
                if (ruleList[i].RuleValidate(input, size, out errorMessage) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
