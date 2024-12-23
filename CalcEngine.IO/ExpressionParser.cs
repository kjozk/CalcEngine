using System.Text;
using CalcEngine.IO.Expressions;

namespace CalcEngine.IO
{
    // 数式文字列を解析してIExpression<TSource, TResult>を返すメソッド
    public static class ExpressionParser
    {
        private static readonly List<BinaryOperator<double, double>> ArithmeticOperators = new List<BinaryOperator<double, double>>
        {
            new BinaryOperator<double, double>("+", 1, (a, b) => a + b),
            new BinaryOperator<double, double>("-", 1, (a, b) => a - b),
            new BinaryOperator<double, double>("*", 2, (a, b) => a * b),
            new BinaryOperator<double, double>("×", 2, (a, b) => a * b),
            new BinaryOperator<double, double>("/", 2, (a, b) => a / b),
            new BinaryOperator<double, double>("÷", 2, (a, b) => a / b)
        };

        private static readonly List<BinaryOperator<double, bool>> ComparisonOperators = new List<BinaryOperator<double, bool>>
        {
            new BinaryOperator<double, bool>(">", 0, (a, b) => a > b),
            new BinaryOperator<double, bool>("<", 0, (a, b) => a < b),
            new BinaryOperator<double, bool>(">=", 0, (a, b) => a >= b),
            new BinaryOperator<double, bool>("<=", 0, (a, b) => a <= b),
            new BinaryOperator<double, bool>("==", 0, (a, b) => a == b)
        };

        /// <summary>
        /// 数式文字列を解析してIExpression<double, double>を返します。
        /// </summary>
        /// <param name="expression">解析する数式文字列。</param>
        /// <returns>解析された式。</returns>
        /// <exception cref="SyntaxErrorException">数式の構文が無効な場合にスローされます。</exception>
        public static IExpression<double, double> ParseArithmeticExpression(string expression)
        {
            // 数式文字列をトークンに分割
            var tokens = Tokenize(expression);
            var operandStack = new Stack<IExpression<double, double>>();
            var operatorStack = new Stack<string>();
            var parenthesisStack = new Stack<string>();

            foreach (var token in tokens)
            {
                // トークンが数値の場合、オペランドスタックにプッシュ
                if (double.TryParse(token, out double number))
                {
                    operandStack.Push(new ConstantExpression<double>(number));
                }
                // トークンが開きカッコの場合、スタックにプッシュ
                else if (token == "(")
                {
                    parenthesisStack.Push(token);
                }
                // トークンが閉じカッコの場合、開きカッコに出会うまで演算子を適用
                else if (token == ")")
                {
                    while (parenthesisStack.Count > 0 && parenthesisStack.Peek() != "(")
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    if (parenthesisStack.Count == 0 || parenthesisStack.Pop() != "(")
                    {
                        throw new SyntaxErrorException("数式の構文が無効です。");
                    }
                }
                // トークンが演算子の場合、演算子スタックにプッシュ
                else if (ArithmeticOperators.Any(op => op.Symbol == token))
                {
                    // 演算子の優先順位を考慮して演算子を適用
                    while (operatorStack.Count > 0 && HasHigherPrecedence(operatorStack.Peek(), token))
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    // 無効なトークンの場合、例外をスロー
                    throw new SyntaxErrorException($"無効なトークン: {token}");
                }
            }

            // 残りの演算子をすべて適用
            while (operatorStack.Count > 0)
            {
                ApplyOperator(operandStack, operatorStack.Pop());
            }

            // 最終的な結果が1つのオペランドでなければ、構文エラー
            if (operandStack.Count != 1)
            {
                throw new SyntaxErrorException("数式の構文が無効です。");
            }

            // 最終結果を返す
            return operandStack.Pop();
        }

        /// <summary>
        /// 数式文字列をトークンに分割します。
        /// </summary>
        /// <param name="expression">解析する数式文字列。</param>
        /// <returns>トークンのリスト。</returns>
        private static List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var number = new StringBuilder();
            bool lastCharWasOperator = true;

            foreach (var ch in expression)
            {
                // 数字または小数点の場合、現在の数値トークンに追加
                if (char.IsDigit(ch) || ch == '.')
                {
                    number.Append(ch);
                    lastCharWasOperator = false;
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    // 数値トークンが存在する場合、トークンリストに追加
                    if (number.Length > 0)
                    {
                        tokens.Add(number.ToString());
                        number.Clear();
                    }

                    // マイナス記号が演算子の直後に来た場合、負の数として扱う
                    if (ch == '-' && lastCharWasOperator)
                    {
                        number.Append(ch);
                    }
                    else
                    {
                        // その他の演算子をトークンリストに追加
                        tokens.Add(ch.ToString());
                        lastCharWasOperator = true;
                    }
                }
            }

            // 最後の数値トークンをトークンリストに追加
            if (number.Length > 0)
            {
                tokens.Add(number.ToString());
            }

            return tokens;
        }

        /// <summary>
        /// 演算子を適用します。
        /// </summary>
        /// <param name="operandStack">オペランドスタック。</param>
        /// <param name="operatorSymbol">演算子のシンボル。</param>
        /// <exception cref="SyntaxErrorException">無効な演算子が指定された場合にスローされます。</exception>
        private static void ApplyOperator(Stack<IExpression<double, double>> operandStack, string operatorSymbol)
        {
            if (operandStack.Count < 2)
            {
                throw new SyntaxErrorException("数式の構文が無効です。");
            }

            var right = operandStack.Pop();
            var left = operandStack.Pop();

            var arithmeticOperator = ArithmeticOperators.FirstOrDefault(op => op.Symbol == operatorSymbol);
            if (arithmeticOperator != null)
            {
                operandStack.Push(new ArithmeticExpression<double>(left, right, arithmeticOperator));
                return;
            }

            throw new SyntaxErrorException($"無効な演算子: {operatorSymbol}");
        }

        /// <summary>
        /// 演算子の優先順位を比較します。
        /// </summary>
        /// <param name="operator1">演算子1。</param>
        /// <param name="operator2">演算子2。</param>
        /// <returns>演算子1が演算子2よりも高い優先順位を持つ場合はtrue、それ以外の場合はfalse。</returns>
        private static bool HasHigherPrecedence(string operator1, string operator2)
        {
            int precedence1 = GetPrecedence(operator1);
            int precedence2 = GetPrecedence(operator2);
            return precedence1 >= precedence2;
        }

        /// <summary>
        /// 演算子の優先順位を取得します。
        /// </summary>
        /// <param name="operatorSymbol">演算子のシンボル。</param>
        /// <returns>演算子の優先順位。</returns>
        /// <exception cref="SyntaxErrorException">無効な演算子が指定された場合にスローされます。</exception>
        private static int GetPrecedence(string operatorSymbol)
        {
            var arithmeticOperator = ArithmeticOperators.FirstOrDefault(op => op.Symbol == operatorSymbol);
            if (arithmeticOperator != null) return arithmeticOperator.Precedence;

            var comparisonOperator = ComparisonOperators.FirstOrDefault(op => op.Symbol == operatorSymbol);
            if (comparisonOperator != null) return comparisonOperator.Precedence;

            throw new SyntaxErrorException($"無効な演算子: {operatorSymbol}");
        }
    }
}
