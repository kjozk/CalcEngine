using CalcEngine.IO;

namespace CalcEngine.Test
{
    [TestClass]
    public sealed class LogicalExpressionTests
    {
        // AND演算のテストケース
        [TestMethod]
        public void AndTest()
        {
            IExpression<bool> expression;

            // true AND true = true
            expression = ExpressionParser<bool>.ParseLogical("true AND true");
            Assert.IsTrue(expression.Evaluate());

            // true AND false = false
            expression = ExpressionParser<bool>.ParseLogical("true AND false");
            Assert.IsFalse(expression.Evaluate());

            // false AND true = false
            expression = ExpressionParser<bool>.ParseLogical("false AND true");
            Assert.IsFalse(expression.Evaluate());

            // false AND false = false
            expression = ExpressionParser<bool>.ParseLogical("false AND false");
            Assert.IsFalse(expression.Evaluate());
        }

        // OR演算のテストケース
        [TestMethod]
        public void OrTest()
        {
            IExpression<bool> expression;

            // true OR true = true
            expression = ExpressionParser<bool>.ParseLogical("true OR true");
            Assert.IsTrue(expression.Evaluate());

            // true OR false = true
            expression = ExpressionParser<bool>.ParseLogical("true OR false");
            Assert.IsTrue(expression.Evaluate());

            // false OR true = true
            expression = ExpressionParser<bool>.ParseLogical("false OR true");
            Assert.IsTrue(expression.Evaluate());

            // false OR false = false
            expression = ExpressionParser<bool>.ParseLogical("false OR false");
            Assert.IsFalse(expression.Evaluate());
        }

        // NOT演算のテストケース
        [TestMethod]
        public void NotTest()
        {
            IExpression<bool> expression;

            // NOT true = false
            expression = ExpressionParser<bool>.ParseLogical("NOT true");
            Assert.IsFalse(expression.Evaluate());

            // NOT false = true
            expression = ExpressionParser<bool>.ParseLogical("NOT false");
            Assert.IsTrue(expression.Evaluate());
        }

        // XOR演算のテストケース
        [TestMethod]
        public void XorTest()
        {
            IExpression<bool> expression;

            // true XOR true = false
            expression = ExpressionParser<bool>.ParseLogical("true XOR true");
            Assert.IsFalse(expression.Evaluate());

            // true XOR false = true
            expression = ExpressionParser<bool>.ParseLogical("true XOR false");
            Assert.IsTrue(expression.Evaluate());

            // false XOR true = true
            expression = ExpressionParser<bool>.ParseLogical("false XOR true");
            Assert.IsTrue(expression.Evaluate());

            // false XOR false = false
            expression = ExpressionParser<bool>.ParseLogical("false XOR false");
            Assert.IsFalse(expression.Evaluate());
        }

        // 混合論理演算のテストケース
        [TestMethod]
        public void MixedLogicalOperationsTest()
        {
            IExpression<bool> expression;

            // true AND false OR true = true
            expression = ExpressionParser<bool>.ParseLogical("true AND false OR true");
            Assert.IsTrue(expression.Evaluate());

            // (true AND false) OR true = true
            expression = ExpressionParser<bool>.ParseLogical("(true AND false) OR true");
            Assert.IsTrue(expression.Evaluate());

            // true AND (false OR true) = true
            expression = ExpressionParser<bool>.ParseLogical("true AND (false OR true)");
            Assert.IsTrue(expression.Evaluate());

            // NOT (true AND false) = true
            expression = ExpressionParser<bool>.ParseLogical("NOT (true AND false)");
            Assert.IsTrue(expression.Evaluate());

            // true AND NOT false = true
            expression = ExpressionParser<bool>.ParseLogical("true AND NOT false");
            Assert.IsTrue(expression.Evaluate());

            // (true OR false) AND NOT false = true
            expression = ExpressionParser<bool>.ParseLogical("(true OR false) AND NOT false");
            Assert.IsTrue(expression.Evaluate());
        }

        // ほかの論理演算子のテストケース
        [TestMethod]
        public void OtherLogicalOperatorsTest()
        {
            IExpression<bool> expression;

            // ⇔ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⇔ true");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ⇔ false");
            Assert.IsFalse(expression.Evaluate());

            // ˜ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("˜true");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("˜false");
            Assert.IsTrue(expression.Evaluate());

            // ! 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("!true");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("!false");
            Assert.IsTrue(expression.Evaluate());

            // ∥ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ∥ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ∥ false");
            Assert.IsFalse(expression.Evaluate());

            // + 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true + false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false + false");
            Assert.IsFalse(expression.Evaluate());

            // · 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("真·真");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("真·偽");
            Assert.IsFalse(expression.Evaluate());

            // ≔ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ≔ true");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ≔ false");
            Assert.IsFalse(expression.Evaluate());

            // ∧ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ∧ true");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ∧ false");
            Assert.IsFalse(expression.Evaluate());

            // ∨ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ∨ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ∨ false");
            Assert.IsFalse(expression.Evaluate());

            // NOR 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true NOR false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false NOR false");
            Assert.IsTrue(expression.Evaluate());

            // NAND 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true NAND false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true NAND true");
            Assert.IsFalse(expression.Evaluate());

            // ⊅ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⊅ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ⊅ true");
            Assert.IsFalse(expression.Evaluate());

            // ⊃ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⊃ false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ⊃ true");
            Assert.IsTrue(expression.Evaluate());

            // ↑ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ↑ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ↑ true");
            Assert.IsFalse(expression.Evaluate());

            // ⊄ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⊄ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ⊄ true");
            Assert.IsFalse(expression.Evaluate());

            // ↚ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ↚ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ↚ true");
            Assert.IsFalse(expression.Evaluate());

            // ← 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ← false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ← true");
            Assert.IsTrue(expression.Evaluate());

            // ⊂ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⊂ false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ⊂ true");
            Assert.IsTrue(expression.Evaluate());

            // P↮ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true P↮ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true P↮ true");
            Assert.IsFalse(expression.Evaluate());

            // ≢ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ≢ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ≢ true");
            Assert.IsFalse(expression.Evaluate());

            // ⊕ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⊕ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ⊕ true");
            Assert.IsFalse(expression.Evaluate());

            // ⊻ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ⊻ false");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ⊻ true");
            Assert.IsFalse(expression.Evaluate());

            // ↔ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ↔ false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ↔ true");
            Assert.IsTrue(expression.Evaluate());

            // ≡ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ≡ false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true ≡ true");
            Assert.IsTrue(expression.Evaluate());

            // XNOR 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true XNOR false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true XNOR true");
            Assert.IsTrue(expression.Evaluate());

            // IFF 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true IFF false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true IFF true");
            Assert.IsTrue(expression.Evaluate());

            // ↓ 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true ↓ false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false ↓ false");
            Assert.IsTrue(expression.Evaluate());

            // IMPLIES 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true IMPLIES false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("false IMPLIES true");
            Assert.IsTrue(expression.Evaluate());

            // EQUIV 演算子のテスト
            expression = ExpressionParser<bool>.ParseLogical("true EQUIV false");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("true EQUIV true");
            Assert.IsTrue(expression.Evaluate());

            // true と false 以外のテストケース
            expression = ExpressionParser<bool>.ParseLogical("yes AND no");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("YES OR NO");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("1 AND 0");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("1 OR 0");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("真 AND 偽");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<bool>.ParseLogical("真 OR 偽");
            Assert.IsTrue(expression.Evaluate());
        }
    }
}
