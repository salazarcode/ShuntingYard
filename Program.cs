using ShuntingYardEvaluator;

namespace ShuntingYard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, double> variables = new Dictionary<string, double>()
            {
                {"estudiantes", 6},
                {"interes", 26},
                {"peso", 16.5 }
            };
            ExpressionEvaluator evaluator = new ExpressionEvaluator(variables);

                
            Console.WriteLine($"Variables:---------------------------start");

            foreach (var item in variables)
            {

                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine($"Variables:---------------------------end");

            Console.WriteLine("");

            while (true) { 
                Console.WriteLine("Introduce una expresion para evaluarla con las variables preconfiguradas: ");
                string expression = Console.ReadLine() ?? "";

                double result = evaluator.Evaluate(expression);

                Console.WriteLine($"Expresión: {expression}");
                Console.WriteLine($"Resultado: {result}");
            }
        }
    }
}

