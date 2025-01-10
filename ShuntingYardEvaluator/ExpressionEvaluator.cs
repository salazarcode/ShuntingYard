using System.Globalization;

namespace ShuntingYardEvaluator
{
    public class ExpressionEvaluator
    {
        private readonly Dictionary<string, int> _precedence = new Dictionary<string, int>();

        public ExpressionEvaluator()
        {
            var _precedence = new Dictionary<string, int>()
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

        public double EvaluateFromInfixExpression(string expression, Dictionary<string, double>? _variables = null)
        {
            string postfixExpression = ConvertToPostfix(expression);
            double res = EvaluatePostfix(postfixExpression, _variables ?? new Dictionary<string, double>());
            return res;
        }
        /// <summary>
        /// Convierte expresiones de la forma infija a postfija.
        /// Como por ejemplo: 2 + 3 * 4 -> 2 3 4 * +
        /// </summary>
        /// <param name="infix"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                    if (!_precedence.ContainsKey(opToken))
                    {
                        throw new Exception($"Operador desconocido: {opToken}");
                    }
                    while (pilaOperadores.Count > 0 && _precedence.ContainsKey(pilaOperadores.Peek()) && _precedence[pilaOperadores.Peek()] >= _precedence[opToken])
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

        /// <summary>
        /// Evalua expreciones en notación postfija 
        /// por medio de el algoritmo Shunting Yard.
        /// </summary>
        /// <param name="postfix"></param>
        /// <param name="_variables"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private double EvaluatePostfix(string postfix, Dictionary<string, double> _variables)
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

        /// <summary>
        /// Verifica si dentro del array 
        /// de precedencia existe un operador.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsOperator(string token) => _precedence.ContainsKey(token);
    }
}
