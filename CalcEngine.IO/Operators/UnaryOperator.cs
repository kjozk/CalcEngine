namespace CalcEngine.IO.Operators
{
    /// <summary>
    /// 単項演算子クラス
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class UnaryOperator<TSource, TResult> : IOperator
    {
        public int Precedence { get; }
        public string Symbol { get; }
        public Func<TSource, TResult> Operation { get; }

        public UnaryOperator(string symbol, int precedence, Func<TSource, TResult> operation)
        {
            Symbol = symbol;
            Precedence = precedence;
            Operation = operation;
        }

        /// <summary>
        /// UnaryOperator を文字列として表現します。
        /// </summary>
        /// <returns>演算子のシンボル。</returns>
        public override string ToString()
        {
            return Symbol;
        }
    }
}
