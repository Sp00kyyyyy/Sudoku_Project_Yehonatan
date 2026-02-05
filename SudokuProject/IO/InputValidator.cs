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

        public bool Validate(int size, string input, out string errorMessege)
        {
            List<IInputRule> ruleList = this.RulesList;
            errorMessege = "";
            for (int i = 0; i < ruleList.Count; i++)
            {
                if (ruleList[i].RuleValidate(input, size, out errorMessege) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
