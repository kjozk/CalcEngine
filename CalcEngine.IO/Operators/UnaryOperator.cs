namespace CalcEngine.IO.Operators
{
    /// <summary>
    /// �P�����Z�q�N���X
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
        /// UnaryOperator �𕶎���Ƃ��ĕ\�����܂��B
        /// </summary>
        /// <returns>���Z�q�̃V���{���B</returns>
        public override string ToString()
        {
            return Symbol;
        }
    }
}
