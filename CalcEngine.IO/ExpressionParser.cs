using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
                new BinaryOperator<TResult, TResult>("÷", 2, (a, b) => (dynamic)a / (dynamic)b),
                new BinaryOperator<TResult, TResult>("%", 2, (a, b) => (dynamic)a % (dynamic)b) // 余算演算子
            };

        /// <summary>
        /// 比較演算子のリスト
        /// </summary>
        private static readonly List<BinaryOperator<TResult, bool>> ComparisonOperators = new List<BinaryOperator<TResult, bool>>
            {
                new BinaryOperator<TResult, bool>("≧", 0, (a, b) => (dynamic)a >= (dynamic)b),
                new BinaryOperator<TResult, bool>("≦", 0, (a, b) => (dynamic)a <= (dynamic)b),
                new BinaryOperator<TResult, bool>(">=", 0, (a, b) => (dynamic)a >= (dynamic)b),
                new BinaryOperator<TResult, bool>("<=", 0, (a, b) => (dynamic)a <= (dynamic)b),
                new BinaryOperator<TResult, bool>("==", 0, (a, b) => (dynamic)a == (dynamic)b),
                new BinaryOperator<TResult, bool>("!=", 0, (a, b) => (dynamic)a != (dynamic)b), // 不等号演算子
                new BinaryOperator<TResult, bool>("≠", 0, (a, b) => (dynamic)a != (dynamic)b), // 不等号演算子
                // > よりも >= が先に評価されるように並べる
                new BinaryOperator<TResult, bool>(">", 0, (a, b) => (dynamic)a > (dynamic)b),
                new BinaryOperator<TResult, bool>("<", 0, (a, b) => (dynamic)a < (dynamic)b),
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
            Debug.WriteLine($"Parse: 開始 \"{expression}\"");
            var operandStack = new Stack<IExpression<TResult>>();
            var operatorStack = new Stack<IOperator>();

            // 式をトークンに分割
            var tokens = Tokenize(expression);
            Debug.WriteLine($"Parse: トークン = [{string.Join(", ", tokens.Select(t => "\"" + t + "\""))}]");

            try
            {
                var result = Parse(tokens, 0);

                Debug.WriteLine($"Parse: 終了 \"{result} = {result.Evaluate()}\"");

                return result;
            }
            catch (Exception ex)
            {
                throw new SyntaxErrorException($"数式 {expression} の構文が無効です。", ex);
            }
        }

        /// <summary>
        /// 式を解析するメソッド
        /// </summary>
        /// <param name="tokens">トークンのリスト</param>
        /// <param name="nest">解析のネスト数</param>
        /// <returns></returns>
        /// <exception cref="SyntaxErrorException"></exception>
        public static IExpression<TResult> Parse(List<string> tokens, int nest)
        {
            var operandStack = new Stack<IExpression<TResult>>();
            var operatorStack = new Stack<IOperator>();

            for (int index = 0; index < tokens.Count; index++)
            {
                var token = tokens.ElementAt(index);
                if (TryParseToken(token, out TResult number))
                {
                    // 数値の場合はオペランドスタックに追加
                    IExpression<TResult> constantExpression = new ConstantExpression<TResult>(number);

                    if (operandStack.Count == 0 && operatorStack.Any() && operatorStack.Peek() is UnaryOperator<TResult, TResult> unaryOperator)
                    {
                        // 左のオペランドなしで単項演算子がある場合は適用
                        operatorStack.Pop();
                        constantExpression = new UnaryExpression<TResult>(constantExpression, unaryOperator);
                    }

                    operandStack.Push(constantExpression);
                    Debug.WriteLine($"Parse[{nest}-{index}]: 定数 = \"{constantExpression}\"");
                }
                else if (token == LeftParenthesis.Symbol)
                {
                    // 現在のindex+1から右括弧が見つかる手前までのtokensを切り出す
                    var innerTokens = ExtractInnerTokens(tokens, index + 1);

                    // 切り出したトークンを再帰的に解析
                    var innerExpression = Parse(innerTokens, nest + 1);
                    var expression = new ParenthesisExpression<TResult>(innerExpression);

                    operandStack.Push(expression);
                    Debug.WriteLine($"Parse[{nest}-{index}]: 括弧式 = \"{expression}\"");

                    // indexを更新
                    index = index + innerTokens.Count + 1;
                }
                else if (operandStack.Count <= operatorStack.Count && UnaryOperators.Any(op => op.Symbol == token))
                {
                    // オペランドスタックの数がオペレータスタックの数より少ない場合は単項演算子として扱う
                    var unaryOperator = UnaryOperators.First(op => op.Symbol == token);
                    operatorStack.Push(unaryOperator);

                    Debug.WriteLine($"Parse[{nest}-{index}]: 単項演算子 = \"{unaryOperator}\"");
                }
                else if (ArithmeticOperators.Any(op => op.Symbol == token))
                {
                    Debug.WriteLine($"Parse[{nest}-{index}]: 二項演算子 = \"{token}\"");

                    // 演算子スタックにある演算子の優先順位が高い場合は適用
                    while (operatorStack.Any() && HasHigherPrecedence(operatorStack.Peek(), token))
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                        Debug.WriteLine($"Parse[{nest}-{index}]: 式 = \"{operandStack.Peek()}\"");
                    }
                    var binaryOperator = ArithmeticOperators.First(op => op.Symbol == token);
                    operatorStack.Push(binaryOperator);
                }
                else
                {
                    throw new SyntaxErrorException($"無効なトークン: {token}");
                }
            }

            while (operatorStack.Any())
            {
                ApplyOperator(operandStack, operatorStack.Pop());
                Debug.WriteLine($"Parse[{nest}]: 式 = \"{operandStack.Peek()}\"");
            }

            if (operandStack.Count != 1)
            {
                throw new SyntaxErrorException($"数式の構文が無効です。数式のルートが複数あります。operandStack=[{string.Join(", ", operandStack.Select(t => "\"" + t + "\""))}]");
            }

            return operandStack.Pop();
        }

        /// <summary>
        /// 比較演算式を解析するメソッド
        /// </summary>
        /// <param name="expression">式文字列</param>
        /// <returns>解析結果の式木</returns>
        /// <exception cref="SyntaxErrorException"></exception>
        public static IExpression<bool> ParseComparison(string expression)
        {
            Debug.WriteLine($"ParseComparison: 開始 \"{expression}\"");
            var operandStack = new Stack<IExpression<TResult>>();
            var operatorStack = new Stack<IOperator>();

            // 式をトークンに分割
            var tokens = Tokenize(expression);
            Debug.WriteLine($"ParseComparison: トークン = [{string.Join(", ", tokens.Select(t => "\"" + t + "\""))}]");

            try
            {
                var result = ParseComparison(tokens, 0);

                Debug.WriteLine($"ParseComparison: 終了 \"{result} = {result.Evaluate()}\"");

                return result;
            }
            catch (Exception ex)
            {
                throw new SyntaxErrorException($"数式 {expression} の構文が無効です。", ex);
            }
        }

        private static IExpression<bool> ParseComparison(List<string> tokens, int nest)
        {
            var operandStack = new Stack<IExpression<TResult>>();
            var operatorStack = new Stack<IOperator>();

            // 比較演算子のインデックスを探す
            int comparisonOperatorIndex = tokens.FindIndex(token => ComparisonOperators.Any(op => op.Symbol == token));

            if (comparisonOperatorIndex == -1)
            {
                throw new SyntaxErrorException("比較演算子が見つかりません。");
            }

            // 比較演算子の前後でトークンを分割
            var leftTokens = tokens.Take(comparisonOperatorIndex).ToList();
            var rightTokens = tokens.Skip(comparisonOperatorIndex + 1).ToList();
            var comparisonOperator = ComparisonOperators.First(op => op.Symbol == tokens[comparisonOperatorIndex]);

            // 左側のトークンを解析
            var leftExpression = Parse(leftTokens, nest + 1);

            // 右側のトークンを解析
            var rightExpression = Parse(rightTokens, nest + 1);

            return new ComparisonExpression<TResult>(leftExpression, rightExpression, comparisonOperator);
        }

        /// <summary>
        /// 括弧内のトークンを切り出すメソッド
        /// </summary>
        /// <param name="tokens">基となるトークン</param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        /// <exception cref="SyntaxErrorException"></exception>
        private static List<string> ExtractInnerTokens(List<string> tokens, int startIndex)
        {
            var innerTokens = new List<string>();
            int parenthesisCount = 1;   // 閉じられていない括弧の数

            int index = startIndex;
            while (index < tokens.Count && parenthesisCount > 0)
            {
                if (tokens.ElementAt(index) == LeftParenthesis.Symbol)
                {
                    parenthesisCount++;
                }
                else if (tokens.ElementAt(index) == RightParenthesis.Symbol)
                {
                    parenthesisCount--;
                }

                if (parenthesisCount > 0)
                {
                    innerTokens.Add(tokens.ElementAt(index));
                }
                index++;
            }

            if (parenthesisCount != 0)
            {
                throw new SyntaxErrorException("対応する右括弧がありません。");
            }

            return innerTokens;
        }

        /// <summary>
        /// 式をトークンに分割するメソッド
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            // 演算子のシンボルを取得
            var operatorSymbols = ArithmeticOperators.Select(op => Regex.Escape(op.Symbol))
                                .Concat(ComparisonOperators.Select(op => Regex.Escape(op.Symbol)))
                                .Concat(UnaryOperators.Select(op => Regex.Escape(op.Symbol)))
                                .Concat(new[] { Regex.Escape(LeftParenthesis.Symbol), Regex.Escape(RightParenthesis.Symbol) });

            // 正規表現パターンを生成
            var pattern = $@"\d+(\.\d+)?|{string.Join("|", operatorSymbols)}|\s+";

            var regex = new Regex(pattern);

            foreach (Match match in regex.Matches(expression))
            {
                var token = match.Value;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    tokens.Add(token);
                }
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
                var expression = new UnaryExpression<TResult>(operand, unaryOperator);

                operandStack.Push(expression);
                return;
            }

            if (operatorSymbol is BinaryOperator<TResult, TResult> binaryOperator)
            {
                if (operandStack.Count < 2)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。二項演算子にオペランドが足りません。");
                }

                var right = operandStack.Pop();
                var left = operandStack.Pop();
                var expression = new ArithmeticExpression<TResult>(left, right, binaryOperator);

                operandStack.Push(expression);
                return;
            }

            if (operatorSymbol is ParenthesisOperator parenthesisOperator)
            {
                if (operandStack.Count < 1)
                {
                    throw new SyntaxErrorException("数式の構文が無効です。");
                }

                var innerExpression = operandStack.Pop();

                var expression = new ParenthesisExpression<TResult>(innerExpression);

                operandStack.Push(expression);
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
