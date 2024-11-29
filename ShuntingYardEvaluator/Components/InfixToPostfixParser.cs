using ShuntingYardEvaluator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuntingYardEvaluator.Components
{
    internal class InfixToPostfixParser
    {
        private readonly Precedence _precedence;
        public InfixToPostfixParser(Precedence precendece)
        {
            _precedence = precendece;
        }
        internal string ConvertToPostfix(string infix)
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
    }
}
