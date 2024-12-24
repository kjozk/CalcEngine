using CalcEngine.IO.Operators;

namespace CalcEngine.IO.Expressions
{
    /// <summary>
    /// 比較演算クラス
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class ComparisonExpression<TSource> : IExpression<bool>
        where TSource : struct, IComparable, IConvertible
    {
        /// <summary>
        /// 左辺の式。
        /// </summary>
        public IExpression<TSource> Left { get; }

        /// <summary>
        /// 右辺の式。
        /// </summary>
        public IExpression<TSource> Right { get; }

        /// <summary>
        /// 二項演算子。
        /// </summary>
        public BinaryOperator<TSource, bool> Operator { get; }

        /// <summary>
        /// ComparisonExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="left">左辺の式。</param>
        /// <param name="right">右辺の式。</param>
        /// <param name="operation">演算を行う関数。</param>
        public ComparisonExpression(IExpression<TSource> left, IExpression<TSource> right, BinaryOperator<TSource, bool> operation)
        {
            this.Left = left;
            this.Right = right;
            this.Operator = operation;
        }

        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <returns>評価結果。</returns>
        public bool Evaluate()
        {
            var leftValue = this.Left.Evaluate();
            var rightValue = this.Right.Evaluate();
            return this.Operator.Operation(leftValue, rightValue);
        }

        /// <summary>
        /// 比較演算式を文字列として表現します。
        /// </summary>
        /// <returns>比較演算式の文字列表現。</returns>
        public override string ToString()
        {
            return $"({this.Left} {this.Operator.Symbol} {this.Right})";
        }
    }
}
