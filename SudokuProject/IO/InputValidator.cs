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
    public class InputValidator
    {
        private readonly List<IInputRule> RulesList;
        public InputValidator(List<IInputRule> rules)
        {
            this.RulesList = rules;
        }
        public void AddRule(IInputRule rule)
        {
            this.RulesList.Add(rule);
        }

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
