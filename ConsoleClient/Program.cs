using ShuntingYardEvaluator;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator();


            Console.WriteLine($"Example variables:---------------------------start");
            Console.WriteLine("");

            Dictionary<string, double> variables = new Dictionary<string, double>()
            {
                {"estudiantes", 6},
                {"interes", 26},
                {"peso", 16.5 }
            };
            foreach (var item in variables)
            {

                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine("");
            Console.WriteLine($"Variables:---------------------------end");
            Console.WriteLine("");

            while (true)
            {
                Console.WriteLine("Introduce una expresion para evaluarla con las variables preconfiguradas: ");
                string expression = Console.ReadLine() ?? "";

                Console.WriteLine("");
                double result = evaluator.EvaluateFromInfixExpression(expression, variables);

                Console.WriteLine($"Expresión: {expression}");
                Console.WriteLine($"Resultado: {result}");
            }
        }
    }
}

