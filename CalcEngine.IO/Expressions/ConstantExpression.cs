namespace CalcEngine.IO.Expressions
{
    // 定数式の具象クラス
    public class ConstantExpression<T> : IExpression<T>
        where T : struct, IComparable, IConvertible
    {
        private T Value { get; }

        /// <summary>
        /// ConstantExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="value">定数の値。</param>
        public ConstantExpression(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <param name="source">評価に使用するソース。</param>
        /// <returns>評価結果。</returns>
        public T Evaluate()
        {
            return this.Value;
        }

        /// <summary>
        /// 定数式を文字列として表現します。
        /// </summary>
        /// <returns>定数の文字列表現。</returns>
        public override string ToString()
        {
            return this.Value.ToString() ?? string.Empty;
        }
    }
}
