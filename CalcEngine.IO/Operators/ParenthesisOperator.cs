using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcEngine.IO.Operators
{
    // 括弧を表す演算子クラス
    public class ParenthesisOperator : IOperator
    {
        public int Precedence { get; }
        public string Symbol { get; }

        public ParenthesisOperator(string symbol, int precedence)
        {
            Symbol = symbol;
            Precedence = precedence;
        }

        public bool IsLeftParenthesis => Symbol == "(";
        public bool IsRightParenthesis => Symbol == ")";

        public override string ToString()
        {
            return Symbol;
        }
    }
}
