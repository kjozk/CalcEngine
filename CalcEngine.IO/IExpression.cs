using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcEngine.IO
{
    // IExpression インターフェース
    public interface IExpression<TResult>
    {
        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <returns>評価結果。</returns>
        TResult Evaluate();
    }

    // IExpression インターフェース
    public interface IExpression<TSource, TResult> : IExpression<TResult>
    {
    }

    // IParameterizedExpression インターフェース
    public interface IParameterizedExpression<TSource, TResult>
    {
        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <param name="parameter">評価に使用するパラメータ。</param>
        /// <returns>評価結果。</returns>
        TResult Evaluate(TSource parameter);
    }
}
