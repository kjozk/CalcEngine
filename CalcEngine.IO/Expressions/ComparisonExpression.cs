namespace CalcEngine.IO.Expressions
{
    // 比較演算クラス
    public class ComparisonExpression<TSource> : IExpression<TSource, bool>
    {
        private readonly IExpression<TSource, TSource> _left;
        private readonly IExpression<TSource, TSource> _right;
        private readonly BinaryOperator<TSource, bool> _operator;

        /// <summary>
        /// ComparisonExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="left">左辺の式。</param>
        /// <param name="right">右辺の式。</param>
        /// <param name="operation">演算を行う関数。</param>
        public ComparisonExpression(IExpression<TSource, TSource> left, IExpression<TSource, TSource> right, BinaryOperator<TSource, bool> operation)
        {
            _left = left;
            _right = right;
            _operator = operation;
        }

        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <returns>評価結果。</returns>
        public bool Evaluate()
        {
            var leftValue = _left.Evaluate();
            var rightValue = _right.Evaluate();
            return _operator.Operation(leftValue, rightValue);
        }

        /// <summary>
        /// 比較演算式を文字列として表現します。
        /// </summary>
        /// <returns>比較演算式の文字列表現。</returns>
        public override string ToString()
        {
            return $"({_left} {_operator.Symbol} {_right})";
        }
    }
}
