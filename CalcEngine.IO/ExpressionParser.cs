﻿using System.Text;
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
            var tokens = Tokenize(expression);
            Stack<IExpression<double, double>> operandStack = new Stack<IExpression<double, double>>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    operandStack.Push(new ConstantExpression<double>(number));
                }
                else if (ArithmeticOperators.Any(op => op.Symbol == token))
                {
                    while (operatorStack.Count > 0 && HasHigherPrecedence(operatorStack.Peek(), token))
                    {
                        ApplyOperator(operandStack, operatorStack.Pop());
                    }
                    operatorStack.Push(token);
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
                if (char.IsDigit(ch) || ch == '.')
                {
                    number.Append(ch);
                    lastCharWasOperator = false;
                }
                else
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