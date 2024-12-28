using System.Text;
using CalcEngine.IO.Expressions;
using CalcEngine.IO.Operators;

namespace CalcEngine.IO
{
    /// <summary>
    /// 文字列から数式を解析するクラス。
    /// </summary>
    /// <typeparam name="TResult">数式の結果の型</typeparam>
    public static class ExpressionParser<TResult>
            where TResult : struct, IConvertible, IComparable, IEquatable<TResult>
    {
        /// <summary>
        /// 算術演算子のリスト
        /// </summary>
        private static readonly List<BinaryOperator<TResult, TResult>> ArithmeticOperators = new List<BinaryOperator<TResult, TResult>>
        {
                new BinaryOperator<TResult, TResult>("+", 1, (a, b) => (dynamic)a + (dynamic)b),
                new BinaryOperator<TResult, TResult>("-", 1, (a, b) => (dynamic)a - (dynamic)b),
                new BinaryOperator<TResult, TResult>("*", 2, (a, b) => (dynamic)a * (dynamic)b),
                new BinaryOperator<TResult, TResult>("×", 2, (a, b) => (dynamic)a * (dynamic)b),
                new BinaryOperator<TResult, TResult>("/", 2, (a, b) => (dynamic)a / (dynamic)b),
                new BinaryOperator<TResult, TResult>("÷", 2, (a, b) => (dynamic)a / (dynamic)b)
        };

        /// <summary>
        /// 比較演算子のリスト
        /// </summary>
        private static readonly List<BinaryOperator<TResult, bool>> ComparisonOperators = new List<BinaryOperator<TResult, bool>>
        {
                new BinaryOperator<TResult, bool>(">", 0, (a, b) => (dynamic)a > (dynamic)b),
                new BinaryOperator<TResult, bool>("<", 0, (a, b) => (dynamic)a < (dynamic)b),
                new BinaryOperator<TResult, bool>(">=", 0, (a, b) => (dynamic)a >= (dynamic)b),
                new BinaryOperator<TResult, bool>("<=", 0, (a, b) => (dynamic)a <= (dynamic)b),
                new BinaryOperator<TResult, bool>("==", 0, (a, b) => (dynamic)a == (dynamic)b)
        };

        /// <summary>
        /// 単項演算子のリスト
        /// </summary>
        private static readonly List<UnaryOperator<TResult, TResult>> UnaryOperators = new List<UnaryOperator<TResult, TResult>>
        {
                new UnaryOperator<TResult, TResult>("+", 4, a => +(dynamic)a),
                new UnaryOperator<TResult, TResult>("-", 4, a => -(dynamic)a),
        };

        // 括弧演算子
        private static readonly ParenthesisOperator LeftParenthesis = new ParenthesisOperator("(", 3);
        private static readonly ParenthesisOperator RightParenthesis = new ParenthesisOperator(")", 3);

        /// <summary>
        /// 式を解析するメソッド
        /// </summary>
        /// <param name="expression">式文字列</param>
        /// <param name="result">解析結果の式木</param>
        /// <returns>
        /// true: 解析成功
        /// false: 解析失敗
        /// </returns>
        public static bool TryParse(string expression, out IExpression<TResult>? result)
        {
            try
            {
                result = Parse(expression);
                return true;
            }
            catch (SyntaxErrorException)
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// 式を解析するメソッド
        /// </summary>
        /// <param name="expression">式文字列</param>
        /// <returns>解析結果の式木</returns>
        /// <exception cref="SyntaxErrorException"></exception>
        public static IExpression<TResult> Parse(string expression)
        {
            var operandStack = new Stack<IExpression<TResult>>();
            var operatorStack = new Stack<IOperator>();
            var parenthesisStack = new Stack<ParenthesisOperator>(); // 括弧を追跡するスタック

            // 式をトークンに分割
            var tokens = Tokenize(expression);

            foreach (var token in tokens)
            {
                if (TryParseToken(token, out TResult number))
                {
                    // 数値の場合はオペランドスタックに追加
                    operandStack.Push(new ConstantExpression<TResult>(number));
                }
                else if (token == LeftParenthesis.Symbol)
                {
                    // 左括弧の場合はオペレータスタックに追加
                    operatorStack.Push(LeftParenthesis);    // TODO: ここでLeftParenthesisを使うのはおかしい
                    parenthesisStack.Push(LeftParenthesis); // 左括弧をスタックに追加
                }
                else if (token == RightParenthesis.Symbol)
                {
                    // 右括弧の場合は対応する左括弧までオペレータを適用
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
                else if (operandStack.Count == 0 && UnaryOperators.Any(op => op.Symbol == token))
                {
                    // 式の先頭に単項演算子がある場合、オペランドスタックに追加
                    operatorStack.Push(UnaryOperators.First(op => op.Symbol == token));
                }
                else if (ArithmeticOperators.Any(op => op.Symbol == token))
                {
                    // 四則演算子の場合はオペレータスタックに追加
                    while (operatorStack.Count > 0 && HasHigherPrecedence(operatorStack.Peek(), token))
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    operatorStack.Push(ArithmeticOperators.First(op => op.Symbol == token));
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
                throw new SyntaxErrorException("対応する右括弧がありません。左括弧が {parenthesisStack.Count} 個多いです。 expression=\"{expression}\", ");
            }

            if (operandStack.Count != 1)
            {
                throw new SyntaxErrorException($"数式の構文が無効です。数式のルートが複数あります。expression=\"{expression}\", operandStack=[{string.Join(",", "\"" + operandStack + "\"")}]");
            }

            return operandStack.Pop();
        }

        /// <summary>
        /// 式をトークンに分割するメソッド
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var number = new StringBuilder();
            bool lastCharWasOperator = true;    // 式の先頭に単項演算子がある場合を考慮して初期値をtrueに設定

            foreach (var ch in expression)
            {
                if (char.IsDigit(ch) || ch == '.')  // TODO: ドイツ語の場合はカンマも許可すべき
                {
                    // 数値の場合は数値文字列に追加
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

                    if (UnaryOperators.Any(ope => ope.Symbol == ch.ToString()) && lastCharWasOperator)  // TODO: 単項演算子は1文字である必要あり
                    {
                        number.Append(ch); // 単項演算子を数字と一緒に処理
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

        // トークンを数値に変換するメソッド
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

        // 演算子を適用するメソッド
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

        // 演算子の優先順位を比較するメソッド
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

        // 演算子の優先順位を取得するメソッド
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
    }
}
