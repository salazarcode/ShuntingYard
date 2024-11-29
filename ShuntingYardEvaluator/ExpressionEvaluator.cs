using ShuntingYardEvaluator.Components;
using ShuntingYardEvaluator.Configuration;

namespace ShuntingYardEvaluator
{
    public class ExpressionEvaluator
    {
        private readonly Precedence _precedence = new Precedence();

        public double EvaluateFromInfixExpression(string expression, Dictionary<string, double>? _variables = null)
        {
            string postfixExpression = new InfixToPostfixParser(_precedence).ConvertToPostfix(expression);
            var res = new ShuntingYardAlgorithm(_precedence).EvaluatePostfix(postfixExpression, _variables ?? new Dictionary<string, double>());
            return res;
        }
    }
}
