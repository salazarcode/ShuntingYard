using ShuntingYardEvaluator;
using System.Drawing;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            while (true)
            {
                Dictionary<string, double> variables = GetVariablesFromUser();

                PrintVariables(variables);

                Console.WriteLine("");
                Console.WriteLine("Introduce una expresion para evaluarla con las variables preconfiguradas: ");
                string expression = Console.ReadLine() ?? "";

                Console.WriteLine("");
                double result = evaluator.EvaluateFromInfixExpression(expression, variables);

                Console.WriteLine($"Expresión: {expression}");
                Console.WriteLine($"Resultado: {result}");
            }
        }

        private static void PrintVariables(Dictionary<string, double> variables)
        {
            Console.WriteLine($"Variables:---------------------------start");
            Console.WriteLine("");
            foreach (var item in variables)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
            Console.WriteLine("");
            Console.WriteLine($"Variables:---------------------------end");
            Console.WriteLine("");
        }

        private static Dictionary<string, double> GetVariablesFromUser()
        {
            var variables = new Dictionary<string, double>(); 
            string variable = "";
            double valor = 0;
            while (true)
            {
                Console.WriteLine("Ingresa el nombre de la variable: ");
                variable = Console.ReadLine()?.Trim();
                if (variable == ":q")
                    break;

                Console.WriteLine($"Ingresa el valor de {variable}: ");
                valor = double.Parse(Console.ReadLine());

                variables.Add(variable, valor);
                Console.WriteLine("");
            }

            return variables;
        }
    }
}

