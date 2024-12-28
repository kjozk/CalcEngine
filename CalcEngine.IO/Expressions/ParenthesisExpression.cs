namespace CalcEngine.IO.Expressions
{
    /// <summary>
    /// カッコで囲まれた式を表すクラス。
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class ParenthesisExpression<TResult> : IExpression<TResult> 
            where TResult : struct, IConvertible, IComparable, IEquatable<TResult>
    {
        private readonly IExpression<TResult> _innerExpression;

        public ParenthesisExpression(IExpression<TResult> innerExpression)
        {
            _innerExpression = innerExpression ?? throw new ArgumentNullException(nameof(innerExpression));
        }

        public TResult Evaluate()
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
