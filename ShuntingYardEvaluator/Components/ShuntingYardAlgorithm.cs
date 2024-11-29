using ShuntingYardEvaluator.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardEvaluator.Components
{
    internal class ShuntingYardAlgorithm
    {
        private readonly Precedence _precedence;
        public ShuntingYardAlgorithm(Precedence precendece)
        {
            _precedence = precendece;
        }
        internal double EvaluatePostfix(string postfix, Dictionary<string, double> _variables)
        {
            Stack<double> stack = new Stack<double>();
            string[] tokens = postfix.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push(number);
                }
                else if (IsOperator(token))
                {
                    if (stack.Count < 2)
                    {
                        throw new Exception("Expresión postfija mal formada.");
                    }

                    double b = stack.Pop();
                    double a = stack.Pop();

                    double result = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        "%" => a % b,
                        "^" => Math.Pow(a, b),
                        _ => throw new Exception($"Operador desconocido: {token}")
                    };

                    stack.Push(result);
                }
                else
                {
                    // Asumimos que es una variable
                    if (_variables.ContainsKey(token))
                    {
                        stack.Push(_variables[token]);
                    }
                    else
                    {
                        throw new Exception($"Variable no definida: {token}");
                    }
                }
            }

            if (stack.Count != 1)
            {
                throw new Exception("Expresión postfija mal formada.");
            }

            return stack.Pop();
        }

        private bool IsOperator(string token)
        {
            return _precedence.ContainsKey(token);
        }
    }
}
