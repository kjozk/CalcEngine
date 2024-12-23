namespace CalcEngine.IO.Expressions
{
    // 四則演算クラス
    public class ArithmeticExpression<T> : IExpression<T, T> where T : struct, IConvertible
    {
        private readonly IExpression<T, T> _left;
        private readonly IExpression<T, T> _right;
        private readonly BinaryOperator<T, T> _operator;

        /// <summary>
        /// ArithmeticExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="left">左辺の式。</param>
        /// <param name="right">右辺の式。</param>
        /// <param name="operator">演算子。</param>
        public ArithmeticExpression(IExpression<T, T> left, IExpression<T, T> right, BinaryOperator<T, T> @operator)
        {
            _left = left;
            _right = right;
            _operator = @operator;
        }

        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <param name="source">評価に使用するソース。</param>
        /// <returns>評価結果。</returns>
        public T Evaluate()
        {
            var leftValue = _left.Evaluate();
            var rightValue = _right.Evaluate();
            return _operator.Operation(leftValue, rightValue);
        }

        /// <summary>
        /// 四則演算式を文字列として表現します。
        /// </summary>
        /// <returns>四則演算式の文字列表現。</returns>
        public override string ToString()
        {
            return $"({_left} {_operator.Symbol} {_right})";
        }
    }
}
