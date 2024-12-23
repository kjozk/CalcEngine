using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcEngine.IO
{
    // IExpression インターフェース
    public interface IExpression<TSource, TResult>
    {
        /// <summary>
        /// 式を評価します。
        /// </summary>
        /// <returns>評価結果。</returns>
        TResult Evaluate();
    }
}
