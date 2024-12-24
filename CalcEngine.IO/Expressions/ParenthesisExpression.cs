namespace CalcEngine.IO.Expressions
{
    public class ParenthesisExpression<T> : IExpression<T> where T : struct, IComparable<T>
    {
        private readonly IExpression<T> _innerExpression;

        public ParenthesisExpression(IExpression<T> innerExpression)
        {
            _innerExpression = innerExpression ?? throw new ArgumentNullException(nameof(innerExpression));
        }

        public T Evaluate()
        {
            // 内部の式をそのまま評価して返す
            return _innerExpression.Evaluate();
        }

        public override string ToString()
        {
            // カッコ付きの文字列表現
            return $"({_innerExpression})";
        }
    }
}
