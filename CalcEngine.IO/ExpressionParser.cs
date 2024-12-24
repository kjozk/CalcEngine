using System.Text;
using CalcEngine.IO.Expressions;
using CalcEngine.IO.Operators;

namespace CalcEngine.IO
{
    public static class ExpressionParser
    {
        private static readonly List<BinaryOperator<double, double>> ArithmeticOperators =
        [
            new("+", 1, (a, b) => a + b),
            new("-", 1, (a, b) => a - b),
            new("*", 2, (a, b) => a * b),
            new("×", 2, (a, b) => a * b),
            new("/", 2, (a, b) => a / b),
            new("÷", 2, (a, b) => a / b)
        ];

        private static readonly List<BinaryOperator<double, bool>> ComparisonOperators =
        [
            new(">", 0, (a, b) => a > b),
            new("<", 0, (a, b) => a < b),
            new(">=", 0, (a, b) => a >= b),
            new("<=", 0, (a, b) => a <= b),
            new("==", 0, (a, b) => a == b)
        ];

        private static readonly UnaryOperator<double, double> NegationOperator = new UnaryOperator<double, double>("-", 4, a => -a);
        private static readonly ParenthesisOperator LeftParenthesis = new ParenthesisOperator("(", 3);
        private static readonly ParenthesisOperator RightParenthesis = new ParenthesisOperator(")", 3);

        public static bool TryParseArithmeticExpression(string expression, out IExpression<double>? result)
        {
            try
            {
                result = ParseArithmeticExpression(expression);
                return true;
            }
            catch (SyntaxErrorException)
            {
                result = null;
                return false;
            }
        }

        public static IExpression<double> ParseArithmeticExpression(string expression)
        {
            var tokens = Tokenize(expression);
            var operandStack = new Stack<IExpression<double>>();
            var operatorStack = new Stack<IOperator>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    operandStack.Push(new ConstantExpression<double>(number));
                }
                else if (token == LeftParenthesis.Symbol)
                {
                    operatorStack.Push(LeftParenthesis);
                }
                else if (token == RightParenthesis.Symbol)
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != LeftParenthesis)
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0 || operatorStack.Pop() != LeftParenthesis)
                    {
                        throw new SyntaxErrorException("数式の構文が無効です。");
                    }
                    var innerExpression = operandStack.Pop();
                    operandStack.Push(new ParenthesisExpression<double>(innerExpression));
                }
                else if (ArithmeticOperators.Any(op => op.Symbol == token))
                {
                    while (operatorStack.Count > 0 && HasHigherPrecedence(operatorStack.Peek(), token))
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    operatorStack.Push(ArithmeticOperators.First(op => op.Symbol == token));
                }
                else if (token == NegationOperator.Symbol)
                {
                    operatorStack.Push(NegationOperator);
                }
                else
                {
                    throw new SyntaxErrorException($"無効なトークン: {token}");
                }
            }

            while (operatorStack.Count > 0)
            {
                ApplyOperator(operandStack, operatorStack.Pop());
            }

            if (operandStack.Count != 1)
            {
                throw new SyntaxErrorException("数式の構文が無効です。");
            }

            return operandStack.Pop();
        }

        private static List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var number = new StringBuilder();
            bool lastCharWasOperator = true;

            foreach (var ch in expression)
            {
                if (char.IsDigit(ch) || ch == '.')
                {
                    number.Append(ch);
                    lastCharWasOperator = false;
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    if (number.Length > 0)
                    {
                        tokens.Add(number.ToString());
                        number.Clear();
                    }

                    if (ch == '-' && lastCharWasOperator)
                    {
                        number.Append(ch);
                    }
                    else
                    {
                        tokens.Add(ch.ToString());
                        lastCharWasOperator = true;
                    }
                }
            }

            if (number.Length > 0)
            {
                tokens.Add(number.ToString());
            }

            return tokens;
        }

        private static void ApplyOperator(Stack<IExpression<double>> operandStack, IOperator operatorSymbol)
        {
            if (operatorSymbol is UnaryOperator<double, double> unaryOperator)
            {
                if (operandStack.Count < 1)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。");
                }

                var operand = operandStack.Pop();
                operandStack.Push(new UnaryExpression<double>(operand, unaryOperator));
                return;
            }

            if (operatorSymbol is BinaryOperator<double, double> binaryOperator)
            {
                if (operandStack.Count < 2)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。");
                }

                var right = operandStack.Pop();
                var left = operandStack.Pop();
                operandStack.Push(new ArithmeticExpression<double>(left, right, binaryOperator));
                return;
            }

            throw new SyntaxErrorException($"無効な演算子: {operatorSymbol}");
        }

        private static bool HasHigherPrecedence(IOperator operator1, string operator2)
        {
            int precedence1 = GetPrecedence(operator1.Symbol);
            int precedence2 = GetPrecedence(operator2);
            return precedence1 >= precedence2;
        }

        private static int GetPrecedence(string operatorSymbol)
        {
            var arithmeticOperator = ArithmeticOperators.FirstOrDefault(op => op.Symbol == operatorSymbol);
            if (arithmeticOperator != null) return arithmeticOperator.Precedence;

            var comparisonOperator = ComparisonOperators.FirstOrDefault(op => op.Symbol == operatorSymbol);
            if (comparisonOperator != null) return comparisonOperator.Precedence;

            if (LeftParenthesis.Symbol == operatorSymbol)
            {
                return LeftParenthesis.Precedence;
            }
            if (RightParenthesis.Symbol == operatorSymbol)
            {
                return LeftParenthesis.Precedence;
            }

            throw new SyntaxErrorException($"無効な演算子: {operatorSymbol}");
        }
    }
}
