using CalcEngine.IO.Operators;

namespace CalcEngine.IO.Expressions
{
    // 単項演算式クラス
    public class UnaryExpression<T> : IExpression<T> where T : struct, IConvertible
    {
        private readonly IExpression<T> _operand;
        private readonly UnaryOperator<T, T> _operator;

        /// <summary>
        /// UnaryExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="operand">単項演算のオペランド。</param>
        /// <param name="operator">適用する単項演算子。</param>
        public UnaryExpression(IExpression<T> operand, UnaryOperator<T, T> @operator)
        {
            _operand = operand;
            _operator = @operator;
        }

        /// <summary>
        /// 単項演算を評価します。
        /// </summary>
        /// <returns>評価結果。</returns>
        public T Evaluate()
        {
            var value = _operand.Evaluate();
            return _operator.Operation(value);
        }

        /// <summary>
        /// 単項演算式を文字列として表現します。
        /// </summary>
        /// <returns>単項演算式の文字列表現。</returns>
        public override string ToString()
        {
            return $"{_operator.Symbol}{_operand}";
        }
    }
}
