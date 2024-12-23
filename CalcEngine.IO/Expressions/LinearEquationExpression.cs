namespace CalcEngine.IO.Expressions
{
    // 一次方程式の具象クラス
    public class LinearEquationExpression : IParameterizedExpression<double, double>
    {
        private readonly double _a;
        private readonly double _b;

        /// <summary>
        /// LinearEquationExpression クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="a">一次方程式の係数 a。</param>
        /// <param name="b">一次方程式の定数項 b。</param>
        public LinearEquationExpression(double a, double b)
        {
            _a = a;
            _b = b;
        }

        /// <summary>
        /// 一次方程式を評価します。
        /// </summary>
        /// <param name="x">変数 x の値。</param>
        /// <returns>評価結果。</returns>
        public double Evaluate(double x)
        {
            return _a * x + _b;
        }

        /// <summary>
        /// 一次方程式を文字列として表現します。
        /// </summary>
        /// <returns>一次方程式の文字列表現。</returns>
        public override string ToString()
        {
            return $"{_a} * x + {_b}";
        }
    }
}
