using System.Text;
using CalcEngine.IO.Expressions;
using CalcEngine.IO.Operators;

namespace CalcEngine.IO
{
    public static class ExpressionParser<TResult>
            where TResult : struct, IConvertible, IComparable, IEquatable<TResult>
    {
        private static readonly List<BinaryOperator<TResult, TResult>> ArithmeticOperators =
        new List<BinaryOperator<TResult, TResult>>
        {
                new BinaryOperator<TResult, TResult>("+", 1, (a, b) => (dynamic)a + (dynamic)b),
                new BinaryOperator<TResult, TResult>("-", 1, (a, b) => (dynamic)a - (dynamic)b),
                new BinaryOperator<TResult, TResult>("*", 2, (a, b) => (dynamic)a * (dynamic)b),
                new BinaryOperator<TResult, TResult>("×", 2, (a, b) => (dynamic)a * (dynamic)b),
                new BinaryOperator<TResult, TResult>("/", 2, (a, b) => (dynamic)a / (dynamic)b),
                new BinaryOperator<TResult, TResult>("÷", 2, (a, b) => (dynamic)a / (dynamic)b)
        };

        private static readonly List<BinaryOperator<TResult, bool>> ComparisonOperators =
        new List<BinaryOperator<TResult, bool>>
        {
                new BinaryOperator<TResult, bool>(">", 0, (a, b) => (dynamic)a > (dynamic)b),
                new BinaryOperator<TResult, bool>("<", 0, (a, b) => (dynamic)a < (dynamic)b),
                new BinaryOperator<TResult, bool>(">=", 0, (a, b) => (dynamic)a >= (dynamic)b),
                new BinaryOperator<TResult, bool>("<=", 0, (a, b) => (dynamic)a <= (dynamic)b),
                new BinaryOperator<TResult, bool>("==", 0, (a, b) => (dynamic)a == (dynamic)b)
        };

        private static readonly List<UnaryOperator<TResult, TResult>> UnaryOperators =
        new List<UnaryOperator<TResult, TResult>>
        {
                new UnaryOperator<TResult, TResult>("+", 4, a => +(dynamic)a),
                new UnaryOperator<TResult, TResult>("-", 4, a => -(dynamic)a),
        };

        private static readonly ParenthesisOperator LeftParenthesis = new ParenthesisOperator("(", 3);
        private static readonly ParenthesisOperator RightParenthesis = new ParenthesisOperator(")", 3);

        public static bool TryParseArithmeticExpression(string expression, out IExpression<TResult>? result)
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

        public static IExpression<TResult> ParseArithmeticExpression(string expression)
        {
            var tokens = Tokenize(expression);
            var operandStack = new Stack<IExpression<TResult>>();
            var operatorStack = new Stack<IOperator>();
            var parenthesisStack = new Stack<ParenthesisOperator>(); // 括弧を追跡するスタック

            foreach (var token in tokens)
            {
                if (TryParseToken(token, out TResult number))
                {
                    operandStack.Push(new ConstantExpression<TResult>(number));
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
                        tokens.Add(ch.ToString()); // 単項マイナスを処理
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

        private static bool TryParseToken(string token, out TResult result)
        {
            try
            {
                result = (TResult)Convert.ChangeType(token, typeof(TResult));
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        private static void ApplyOperator(Stack<IExpression<TResult>> operandStack, IOperator operatorSymbol)
        {
            if (operatorSymbol is UnaryOperator<TResult, TResult> unaryOperator)
            {
                if (operandStack.Count < 1)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。");
                }

                var operand = operandStack.Pop();
                operandStack.Push(new UnaryExpression<TResult>(operand, unaryOperator));
                return;
            }

            if (operatorSymbol is BinaryOperator<TResult, TResult> binaryOperator)
            {
                if (operandStack.Count < 2)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。");
                }

                var right = operandStack.Pop();
                var left = operandStack.Pop();
                operandStack.Push(new ArithmeticExpression<TResult>(left, right, binaryOperator));
                return;
            }

            if (operatorSymbol is ParenthesisOperator parenthesisOperator)
            {
                if (operandStack.Count < 1)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。");
                }

                var innerExpression = operandStack.Pop();
                operandStack.Push(new ParenthesisExpression<TResult>(innerExpression));
                return;
            }

            throw new SyntaxErrorException($"無効な演算子: {operatorSymbol}");
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

            throw new SyntaxErrorException($"無効な演算子: {operatorSymbol}");
        }

        private static bool IsOperator(IOperator op)
        {
            return op is BinaryOperator<TResult, TResult> || op is UnaryOperator<TResult, TResult>;
        }
    }
}
