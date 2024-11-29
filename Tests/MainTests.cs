using ShuntingYardEvaluator;

namespace Tests
{
    public class MainTests
    {
        [Fact]
        public void Evaluate_SimpleExpression_ReturnsCorrectResult()
        {
            // Propiedad asociativa
            var variables = new Dictionary<string, double>();
            string expression = "2 + 3 * 4";

            // Act
            var result = new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables);

            // Assert
            Assert.Equal(14, result);
        }

        [Fact]
        public void Evaluate_ExpressionWithVariables_ReturnsCorrectResult()
        {
            // Arrange
            var variables = new Dictionary<string, double>
            {
                { "x", 5 },
                { "y", 10 }
            };
            string expression = "x * y + 2";

            // Act
            var result = new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables);

            // Assert
            Assert.Equal(52, result);
        }

        [Fact]
        public void Evaluate_ExpressionWithParentheses_ReturnsCorrectResult()
        {
            // Arrange
            var variables = new Dictionary<string, double>();
            string expression = "(2 + 3) * 4";

            // Act
            var result = new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables);

            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public void Evaluate_ExpressionWithDecimals_ReturnsCorrectResult()
        {
            // Arrange
            var variables = new Dictionary<string, double>();
            string expression = "5.5 * 2";

            // Act
            var result = new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables);

            // Assert
            Assert.Equal(11, result);
        }

        [Fact]
        public void Evaluate_ExpressionWithUnknownVariable_ThrowsException()
        {
            // Arrange
            var variables = new Dictionary<string, double>();
            string expression = "x + 2";

            // Act & Assert
            Assert.Throws<Exception>(() => new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables));
        }

        [Fact]
        public void Evaluate_MalformedExpression_ThrowsException()
        {
            // Arrange
            var variables = new Dictionary<string, double>();
            string expression = "2 + * 3";

            // Act & Assert
            Assert.Throws<Exception>(() => new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables));
        }

        [Fact]
        public void Evaluate_ExpressionWithAllOperators_ReturnsCorrectResult()
        {
            // Arrange
            var variables = new Dictionary<string, double>
            {
                { "a", 5 },
                { "b", 3 },
                { "c", 2 }
            };

            string expression = "a + b * c ^ 2 - (b % c)";

            // Act
            var result = new ExpressionEvaluator().EvaluateFromInfixExpression(expression, variables);

            // Calculando manualmente:
            // c ^ 2 = 4
            // b * 4 = 12
            // a + 12 = 17
            // b % c = 1
            // 17 - 1 = 16

            // Assert
            Assert.Equal(16, result);
        }
    }
}
