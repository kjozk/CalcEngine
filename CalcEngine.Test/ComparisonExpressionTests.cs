using CalcEngine.IO;

namespace CalcEngine.Test
{
    [TestClass]
    public sealed class ComparisonExpressionTests
    {
        // 大なり比較のテストケース
        [TestMethod]
        public void GreaterThanTest()
        {
            IExpression<bool> expression;

            // 5 > 3 = true
            expression = ExpressionParser<double>.ParseComparison("5 > 3");
            Assert.IsTrue(expression.Evaluate());

            // 3 > 5 = false
            expression = ExpressionParser<double>.ParseComparison("3 > 5");
            Assert.IsFalse(expression.Evaluate());

            // 5 > 5 = false
            expression = ExpressionParser<double>.ParseComparison("5 > 5");
            Assert.IsFalse(expression.Evaluate());

            // 5 > 3 + 1 = true
            expression = ExpressionParser<double>.ParseComparison("5 > 3 + 1");
            Assert.IsTrue(expression.Evaluate());

            // 5 > 3 + 3 = false
            expression = ExpressionParser<double>.ParseComparison("5 > 3 + 3");
            Assert.IsFalse(expression.Evaluate());

            // 5 - 1 > 3 = true
            expression = ExpressionParser<double>.ParseComparison("5 - 1 > 3");
            Assert.IsTrue(expression.Evaluate());

            expression = ExpressionParser<double>.ParseComparison("5 - 2 > 3");
            Assert.IsFalse(expression.Evaluate());

            expression = ExpressionParser<double>.ParseComparison("5 - 2 > 3 - 1");
            Assert.IsTrue(expression.Evaluate());

            // 10 / 2 > 3 = true
            expression = ExpressionParser<double>.ParseComparison("10 / 2 > 3");
            Assert.IsTrue(expression.Evaluate());

            // 10 / 2 > 5 = false
            expression = ExpressionParser<double>.ParseComparison("10 / 2 > 5");
            Assert.IsFalse(expression.Evaluate());

            // 2 * 3 > 5 = true
            expression = ExpressionParser<double>.ParseComparison("2 * 3 > 5");
            Assert.IsTrue(expression.Evaluate());

            // 2 * 2 > 5 = false
            expression = ExpressionParser<double>.ParseComparison("2 * 2 > 5");
            Assert.IsFalse(expression.Evaluate());
        }

        // 小なり比較のテストケース
        [TestMethod]
        public void LessThanTest()
        {
            IExpression<bool> expression;

            // 3 < 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 < 5");
            Assert.IsTrue(expression.Evaluate());

            // 5 < 3 = false
            expression = ExpressionParser<double>.ParseComparison("5 < 3");
            Assert.IsFalse(expression.Evaluate());

            // 5 < 5 = false
            expression = ExpressionParser<double>.ParseComparison("5 < 5");
            Assert.IsFalse(expression.Evaluate());

            // 3 + 1 < 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 + 1 < 5");
            Assert.IsTrue(expression.Evaluate());

            // 3 + 3 < 5 = false
            expression = ExpressionParser<double>.ParseComparison("3 + 3 < 5");
            Assert.IsFalse(expression.Evaluate());

            // 5 < 10 / 2 = true
            expression = ExpressionParser<double>.ParseComparison("5 < 10 / 2");
            Assert.IsFalse(expression.Evaluate());

            // 5 < 2 * 3 = true
            expression = ExpressionParser<double>.ParseComparison("5 < 2 * 3");
            Assert.IsTrue(expression.Evaluate());

            // 5 < 2 * 2 = false
            expression = ExpressionParser<double>.ParseComparison("5 < 2 * 2");
            Assert.IsFalse(expression.Evaluate());
        }

        // 等しい比較のテストケース
        [TestMethod]
        public void EqualToTest()
        {
            IExpression<bool> expression;

            // 5 == 5 = true
            expression = ExpressionParser<double>.ParseComparison("5 == 5");
            Assert.IsTrue(expression.Evaluate());

            // 5 == 3 = false
            expression = ExpressionParser<double>.ParseComparison("5 == 3");
            Assert.IsFalse(expression.Evaluate());

            // 3 == 5 = false
            expression = ExpressionParser<double>.ParseComparison("3 == 5");
            Assert.IsFalse(expression.Evaluate());

            // 3 + 2 == 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 + 2 == 5");
            Assert.IsTrue(expression.Evaluate());

            // 3 + 3 == 5 = false
            expression = ExpressionParser<double>.ParseComparison("3 + 3 == 5");
            Assert.IsFalse(expression.Evaluate());

            // 10 / 2 == 5 = true
            expression = ExpressionParser<double>.ParseComparison("10 / 2 == 5");
            Assert.IsTrue(expression.Evaluate());

            // 2 * 3 == 6 = true
            expression = ExpressionParser<double>.ParseComparison("2 * 3 == 6");
            Assert.IsTrue(expression.Evaluate());

            // 2 * 2 == 5 = false
            expression = ExpressionParser<double>.ParseComparison("2 * 2 == 5");
            Assert.IsFalse(expression.Evaluate());
        }

        // 大なり等しい比較のテストケース
        [TestMethod]
        public void GreaterThanOrEqualToTest()
        {
            IExpression<bool> expression;

            // 5 >= 3 = true
            expression = ExpressionParser<double>.ParseComparison("5 >= 3");
            Assert.IsTrue(expression.Evaluate());

            // 5 ≧ 3 = true
            expression = ExpressionParser<double>.ParseComparison("5 ≧ 3");
            Assert.IsTrue(expression.Evaluate());

            // 3 >= 5 = false
            expression = ExpressionParser<double>.ParseComparison("3 >= 5");
            Assert.IsFalse(expression.Evaluate());

            // 5 >= 5 = true
            expression = ExpressionParser<double>.ParseComparison("5 >= 5");
            Assert.IsTrue(expression.Evaluate());

            // 3 + 2 >= 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 + 2 >= 5");
            Assert.IsTrue(expression.Evaluate());

            // 3 + 3 >= 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 + 3 >= 5");
            Assert.IsTrue(expression.Evaluate());

            // 10 / 2 >= 5 = true
            expression = ExpressionParser<double>.ParseComparison("10 / 2 >= 5");
            Assert.IsTrue(expression.Evaluate());

            // 2 * 3 >= 6 = true
            expression = ExpressionParser<double>.ParseComparison("2 * 3 >= 6");
            Assert.IsTrue(expression.Evaluate());

            // 2 * 2 >= 5 = false
            expression = ExpressionParser<double>.ParseComparison("2 * 2 >= 5");
            Assert.IsFalse(expression.Evaluate());
        }

        // 小なり等しい比較のテストケース
        [TestMethod]
        public void LessThanOrEqualToTest()
        {
            IExpression<bool> expression;

            // 3 <= 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 <= 5");
            Assert.IsTrue(expression.Evaluate());

            // 5 <= 3 = false
            expression = ExpressionParser<double>.ParseComparison("5 <= 3");
            Assert.IsFalse(expression.Evaluate());

            // 5 <= 5 = true
            expression = ExpressionParser<double>.ParseComparison("5 <= 5");
            Assert.IsTrue(expression.Evaluate());

            // 3 + 1 <= 5 = true
            expression = ExpressionParser<double>.ParseComparison("3 + 1 <= 5");
            Assert.IsTrue(expression.Evaluate());

            // 3 + 3 <= 5 = false
            expression = ExpressionParser<double>.ParseComparison("3 + 3 <= 5");
            Assert.IsFalse(expression.Evaluate());

            // 5 <= 10 / 2 = true
            expression = ExpressionParser<double>.ParseComparison("5 <= 10 / 2");
            Assert.IsTrue(expression.Evaluate());

            // 5 <= 2 * 3 = true
            expression = ExpressionParser<double>.ParseComparison("5 <= 2 * 3");
            Assert.IsTrue(expression.Evaluate());

            // 5 <= 2 * 2 = false
            expression = ExpressionParser<double>.ParseComparison("5 <= 2 * 2");
            Assert.IsFalse(expression.Evaluate());
        }
    }
}
