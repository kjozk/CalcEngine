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

        private static readonly List<UnaryOperator<double, double>> UnaryOperators =
        [
            new("+", 4, a => +a),
            new("-", 4, a => -a),
        ];

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
            var parenthesisStack = new Stack<ParenthesisOperator>(); // 括弧を追跡するスタック

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    operandStack.Push(new ConstantExpression<double>(number));
                }
                else if (token == LeftParenthesis.Symbol)
                {
                    operatorStack.Push(LeftParenthesis);
                    parenthesisStack.Push(LeftParenthesis); // 左括弧をスタックに追加
                }
                else if (token == RightParenthesis.Symbol)
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != LeftParenthesis)
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0 || operatorStack.Pop() != LeftParenthesis)
                    {
                        throw new SyntaxErrorException("対応する左括弧がありません。");
                    }
                    parenthesisStack.Pop(); // 対応する左括弧をスタックから削除
                }
                else if (ArithmeticOperators.Any(op => op.Symbol == token))
                {
                    // 単項演算子として処理するかどうかを判断
                    bool isUnaryOperator = false;
                    var unaryOperator = UnaryOperators.FirstOrDefault(ope => ope.Symbol == token);
                    if (unaryOperator != null)
                    {
                        // オペランドがまだ一つもない、もしくは1つしかない場合は単項演算子として処理
                        if (operandStack.Count == 0)
                        {
                            operatorStack.Push(unaryOperator); // 単項演算子としてプッシュ
                            isUnaryOperator = true;
                        }

                        // TODO: 直前のトークンが演算子の場合も単項演算子として処理

                    }

                    // 他の演算子は通常通り処理
                    if (!isUnaryOperator)
                    {
                        while (operatorStack.Count > 0 && HasHigherPrecedence(operatorStack.Peek(), token))
                        {
                            ApplyOperator(operandStack, operatorStack.Pop());
                        }
                        operatorStack.Push(ArithmeticOperators.First(op => op.Symbol == token));
                    }
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

            // 解析後に括弧が残っていれば対応が取れていない
            if (parenthesisStack.Count > 0)
            {
                throw new SyntaxErrorException("対応する右括弧がありません。");
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
                        tokens.Add(ch.ToString()); // Handle unary minus
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
                    throw new SyntaxErrorException("Invalid expression.");
                }

                var operand = operandStack.Pop();
                operandStack.Push(new UnaryExpression<double>(operand, unaryOperator));
                return;
            }

            if (operatorSymbol is BinaryOperator<double, double> binaryOperator)
            {
                if (operandStack.Count < 2)
                {
                    throw new SyntaxErrorException("Invalid expression.");
                }

                var right = operandStack.Pop();
                var left = operandStack.Pop();
                operandStack.Push(new ArithmeticExpression<double>(left, right, binaryOperator));
                return;
            }

            if (operatorSymbol is ParenthesisOperator parenthesisOperator)
            {
                if (operandStack.Count < 1)
                {
                    throw new SyntaxErrorException("Invalid expression.");
                }

                var innerExpression = operandStack.Pop();
                operandStack.Push(new ParenthesisExpression<double>(innerExpression));
                return;
            }

            throw new SyntaxErrorException($"Invalid operator: {operatorSymbol}");
        }

        private static bool HasHigherPrecedence(IOperator operator1, string operator2)
        {
            // 括弧は優先順位の比較を行わない
            if (operator1 == LeftParenthesis || operator2 == LeftParenthesis.Symbol)
            {
                return false;
            }
            if (operator1 == RightParenthesis || operator2 == RightParenthesis.Symbol)
            {
                return true; // 括弧が閉じられる場合は必ず優先
            }
            int precedence1 = GetPrecedence(operator1.Symbol);
            int precedence2 = GetPrecedence(operator2);
            return precedence1 >= precedence2;
        }

        private static int GetPrecedence(string operatorSymbol)
        {
            var arithmeticOperator = ArithmeticOperators.FirstOrDefault(op => op.Symbol == operatorSymbol);
            if (arithmeticOperator != null) return arithmeticOperator.Precedence;

            if (operatorSymbol == LeftParenthesis.Symbol || operatorSymbol == RightParenthesis.Symbol)
            {
                return 3;
            }

            throw new SyntaxErrorException($"Invalid operator: {operatorSymbol}");
        }

        private static bool IsOperator(IOperator op)
        {
            return op is BinaryOperator<double, double> || op is UnaryOperator<double, double>;
        }
    }
}
