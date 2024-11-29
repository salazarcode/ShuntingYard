using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardEvaluator.Configuration
{
    internal class Precedence : Dictionary<string, int>
    {
        public Precedence()
        {
            var precedences = new Dictionary<string, int>()
            {
                {"^", 4},
                {"*", 3},
                {"/", 3},
                {"%", 3},
                {"+", 2},
                {"-", 2},
                {"(", 1}
            };

            foreach (var item in precedences)
            {
                Add(item.Key, item.Value);
            }
        }
    }
}
