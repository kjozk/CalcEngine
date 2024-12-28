using CalcEngine.IO.Operators;

namespace CalcEngine.IO.Expressions
{
    /// <summary>
    /// 四則演算クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryExpression<T> : IExpression<T>
        where T : struct, IComparable, IConvertible
    {
        /// <summary>
        /// 左辺の式。
        /// </summary>
        public IExpression<T> Left { get; }

        /// <summary>
        /// 右辺の式。
        /// </summary>
        public IExpression<T> Right { get; }

        /// <summary>
        /// 二項演算子。
        /// </summary>
        public BinaryOperator<T, T> Operator { get; }

        /// <summary>
        /// ArithmeticExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="left">左辺の式。</param>
        /// <param name="right">右辺の式。</param>
        /// <param name="operator">演算子。</param>
        public BinaryExpression(IExpression<T> left, IExpression<T> right, BinaryOperator<T, T> @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <param name="source">評価に使用するソース。</param>
        /// <returns>評価結果。</returns>
        public T Evaluate()
        {
            var leftValue = Left.Evaluate();
            var rightValue = Right.Evaluate();
            return Operator.Operation(leftValue, rightValue);
        }

        /// <summary>
        /// 四則演算式を文字列として表現します。
        /// </summary>
        /// <returns>四則演算式の文字列表現。</returns>
        public override string ToString()
        {
            return $"{Left} {Operator.Symbol} {Right}";
        }
    }
}
