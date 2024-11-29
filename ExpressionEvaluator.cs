using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpressionEvaluation
{
    public class ExpressionEvaluator
    {
        private readonly Dictionary<string, double> _variables;
        private readonly Dictionary<string, int> _precedencia;

        public ExpressionEvaluator(Dictionary<string, double> variables)
        {
            _variables = variables ?? new Dictionary<string, double>();
            _precedencia = new Dictionary<string, int>()
            {
                {"^", 4},
                {"*", 3},
                {"/", 3},
                {"%", 3},
                {"+", 2},
                {"-", 2},
                {"(", 1}
            };
        }

        public double Evaluate(string expression)
        {
            string postfix = ConvertToPostfix(expression);
            return EvaluatePostfix(postfix);
        }

        private string ConvertToPostfix(string infix)
        {
            Stack<string> pilaOperadores = new Stack<string>();
            List<string> salida = new List<string>();

            int i = 0;
            while (i < infix.Length)
            {
                char token = infix[i];

                if (char.IsWhiteSpace(token))
                {
                    i++;
                    continue;
                }

                if (char.IsDigit(token) || token == '.')
                {
                    string operando = "";
                    bool hayPuntoDecimal = false;

                    while (i < infix.Length && (char.IsDigit(infix[i]) || infix[i] == '.'))
                    {
                        if (infix[i] == '.')
                        {
                            if (hayPuntoDecimal)
                            {
                                throw new Exception("Número mal formado: más de un punto decimal.");
                            }
                            hayPuntoDecimal = true;
                        }
                        operando += infix[i];
                        i++;
                    }
                    salida.Add(operando);
                    continue;
                }
                else if (char.IsLetter(token))
                {
                    // Manejo de variables (letras o palabras)
                    string variable = "";
                    while (i < infix.Length && char.IsLetterOrDigit(infix[i]))
                    {
                        variable += infix[i];
                        i++;
                    }
                    salida.Add(variable);
                    continue;
                }
                else if (token == '(')
                {
                    pilaOperadores.Push(token.ToString());
                    i++;
                }
                else if (token == ')')
                {
                    while (pilaOperadores.Count > 0 && pilaOperadores.Peek() != "(")
                    {
                        salida.Add(pilaOperadores.Pop());
                    }
                    if (pilaOperadores.Count == 0)
                    {
                        throw new Exception("Paréntesis desbalanceados: falta '('");
                    }
                    pilaOperadores.Pop(); // Eliminar '(' de la pila
                    i++;
                }
                else // Operador
                {
                    string opToken = token.ToString();
                    if (!_precedencia.ContainsKey(opToken))
                    {
                        throw new Exception($"Operador desconocido: {opToken}");
                    }
                    while (pilaOperadores.Count > 0 && _precedencia.ContainsKey(pilaOperadores.Peek()) && _precedencia[pilaOperadores.Peek()] >= _precedencia[opToken])
                    {
                        salida.Add(pilaOperadores.Pop());
                    }
                    pilaOperadores.Push(opToken);
                    i++;
                }
            }

            while (pilaOperadores.Count > 0)
            {
                if (pilaOperadores.Peek() == "(" || pilaOperadores.Peek() == ")")
                {
                    throw new Exception("Paréntesis desbalanceados.");
                }
                salida.Add(pilaOperadores.Pop());
            }

            return string.Join(" ", salida);
        }

        private double EvaluatePostfix(string postfix)
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
            return _precedencia.ContainsKey(token);
        }
    }
}
