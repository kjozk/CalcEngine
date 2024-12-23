using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcEngine.IO
{
    // 二項演算子クラス
    public class BinaryOperator<TSource, TResult> : IOperator
    {
        public int Precedence { get; }
        public string Symbol { get; }
        public Func<TSource, TSource, TResult> Operation { get; }

        /// <summary>
        /// Operator クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="symbol">演算子のシンボル。</param>
        /// <param name="precedence">演算子の優先順位。</param>
        /// <param name="operation">演算を行う関数。</param>
        public BinaryOperator(string symbol, int precedence, Func<TSource, TSource, TResult> operation)
        {
            Symbol = symbol;
            Precedence = precedence;
            Operation = operation;
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
}
