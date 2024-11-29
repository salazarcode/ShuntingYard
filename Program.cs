using System.Globalization;
using System.Linq.Expressions;

namespace ShuntingYard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            Dictionary<string, double> variables = new Dictionary<string, double>()
            {
                {"estudiantes", 6},
                {"interes", 26},
                {"peso", 16.5 }
            };
                
            Console.WriteLine($"Variables:---------------------------start");

            foreach (var item in variables)
            {

                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine($"Variables:---------------------------end");

            Console.WriteLine("");

            while (true) { 
                Console.WriteLine("Introduce una expresion para evaluarla con las variables preconfiguradas: ");
                var expression = Console.ReadLine();

                double result = evaluator.EvaluateExpression(expression, variables);

                Console.WriteLine($"Expresión: {expression}");
                Console.WriteLine($"Resultado: {result}");
            }
        }
    }
}

