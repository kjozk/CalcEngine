namespace CalcEngine.IO
{
    // IOperator インターフェース
    public interface IOperator
    {
        int Precedence { get; }
        string Symbol { get; }
    }
}
