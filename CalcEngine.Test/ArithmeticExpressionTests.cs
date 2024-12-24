using CalcEngine.IO;

namespace CalcEngine.Test
{
    [TestClass]
    public sealed class ArithmeticExpressionTests
    {
        // 足し算のテストケース
        [TestMethod]
        public void AdditionTest()
        {
            IExpression<double> expression;

            // 3 + 4 = 7
            expression = ExpressionParser.ParseArithmeticExpression("3 + 4");
            Assert.AreEqual(7, expression.Evaluate());

            // 1 + 1 = 2
            expression = ExpressionParser.ParseArithmeticExpression("1 + 1");
            Assert.AreEqual(2, expression.Evaluate());

            // 0 + 0 = 0
            expression = ExpressionParser.ParseArithmeticExpression("0 + 0");
            Assert.AreEqual(0, expression.Evaluate());

            // -1 + 1 = 0
            expression = ExpressionParser.ParseArithmeticExpression("-1 + 1");
            Assert.AreEqual(0, expression.Evaluate());

            // 1000000 + 2000000 = 3000000
            expression = ExpressionParser.ParseArithmeticExpression("1000000 + 2000000");
            Assert.AreEqual(3000000, expression.Evaluate());

            // 123456789 + 987654321 = 1111111110
            expression = ExpressionParser.ParseArithmeticExpression("123456789 + 987654321");
            Assert.AreEqual(1111111110, expression.Evaluate());
        }

        // 引き算のテストケース
        [TestMethod]
        public void SubtractionTest()
        {
            IExpression<double> expression;

            // 5 - 3 = 2
            expression = ExpressionParser.ParseArithmeticExpression("5 - 3");
            Assert.AreEqual(2, expression.Evaluate());

            // 10 - 7 = 3
            expression = ExpressionParser.ParseArithmeticExpression("10 - 7");
            Assert.AreEqual(3, expression.Evaluate());

            // 0 - 0 = 0
            expression = ExpressionParser.ParseArithmeticExpression("0 - 0");
            Assert.AreEqual(0, expression.Evaluate());

            // -1 - 1 = -2
            expression = ExpressionParser.ParseArithmeticExpression("-1 - 1");
            Assert.AreEqual(-2, expression.Evaluate());

            // 1000000 - 500000 = 500000
            expression = ExpressionParser.ParseArithmeticExpression("1000000 - 500000");
            Assert.AreEqual(500000, expression.Evaluate());

            // 987654321 - 123456789 = 864197532
            expression = ExpressionParser.ParseArithmeticExpression("987654321 - 123456789");
            Assert.AreEqual(864197532, expression.Evaluate());
        }

        // 掛け算のテストケース
        [TestMethod]
        public void MultiplicationTest()
        {
            IExpression<double> expression;

            // 3 * 4 = 12
            expression = ExpressionParser.ParseArithmeticExpression("3 * 4");
            Assert.AreEqual(12, expression.Evaluate());

            // 2 × 5 = 10
            expression = ExpressionParser.ParseArithmeticExpression("2 × 5");
            Assert.AreEqual(10, expression.Evaluate());

            // 0 * 5 = 0
            expression = ExpressionParser.ParseArithmeticExpression("0 * 5");
            Assert.AreEqual(0, expression.Evaluate());

            // -2 × 3 = -6
            expression = ExpressionParser.ParseArithmeticExpression("-2 × 3");
            Assert.AreEqual(-6, expression.Evaluate());

            // 1000 * 2000 = 2000000
            expression = ExpressionParser.ParseArithmeticExpression("1000 * 2000");
            Assert.AreEqual(2000000, expression.Evaluate());

            // 12345 * 6789 = 83810205
            expression = ExpressionParser.ParseArithmeticExpression("12345 * 6789");
            Assert.AreEqual(83810205, expression.Evaluate());
        }

        // 割り算のテストケース
        [TestMethod]
        public void DivisionTest()
        {
            IExpression<double> expression;

            // 8 / 2 = 4
            expression = ExpressionParser.ParseArithmeticExpression("8 / 2");
            Assert.AreEqual(4, expression.Evaluate());

            // 9 ÷ 3 = 3
            expression = ExpressionParser.ParseArithmeticExpression("9 ÷ 3");
            Assert.AreEqual(3, expression.Evaluate());

            // 0 / 1 = 0
            expression = ExpressionParser.ParseArithmeticExpression("0 / 1");
            Assert.AreEqual(0, expression.Evaluate());

            // -6 ÷ 2 = -3
            expression = ExpressionParser.ParseArithmeticExpression("-6 ÷ 2");
            Assert.AreEqual(-3, expression.Evaluate());

            // 1000000 / 2 = 500000
            expression = ExpressionParser.ParseArithmeticExpression("1000000 / 2");
            Assert.AreEqual(500000, expression.Evaluate());

            // 123456789 / 3 = 41152263
            expression = ExpressionParser.ParseArithmeticExpression("123456789 / 3");
            Assert.AreEqual(41152263, expression.Evaluate());
        }

        // 混合演算のテストケース
        [TestMethod]
        public void MixedOperationsTest()
        {
            IExpression<double> expression;

            // 3 + 5 * 2 = 13
            expression = ExpressionParser.ParseArithmeticExpression("3 + 5 * 2");
            Assert.AreEqual(13, expression.Evaluate());

            // 10 - 2 / 2 = 9
            expression = ExpressionParser.ParseArithmeticExpression("10 - 2 / 2");
            Assert.AreEqual(9, expression.Evaluate());

            // 1 + 2 * 3 - 4 = 3
            expression = ExpressionParser.ParseArithmeticExpression("2 * 3 + 4 / 2");
            Assert.AreEqual(8, expression.Evaluate());

            // 10 / 2 - 3 * 2 = -1
            expression = ExpressionParser.ParseArithmeticExpression("10 / 2 - 3 * 2");
            Assert.AreEqual(-1, expression.Evaluate());

            // 1 + 2 * 3 = 7
            expression = ExpressionParser.ParseArithmeticExpression("1 + 2 * 3");
            Assert.AreEqual(7, expression.Evaluate());

            // (1 + 2) * 3 = 9
            expression = ExpressionParser.ParseArithmeticExpression("(1 + 2) * 3");
            Assert.AreEqual(9, expression.Evaluate());

            // 1000000 + 2000000 * 3 = 7000000
            expression = ExpressionParser.ParseArithmeticExpression("1000000 + 2000000 * 3");
            Assert.AreEqual(7000000, expression.Evaluate());

            // (123456789 + 987654321) / 2 = 555555555
            expression = ExpressionParser.ParseArithmeticExpression("(123456789 + 987654321) / 2");
            Assert.AreEqual(555555555, expression.Evaluate());
        }

        // 負の数のテストケース
        [TestMethod]
        public void NegativeNumberTest()
        {
            IExpression<double> expression;

            // -10 = -10
            expression = ExpressionParser.ParseArithmeticExpression("-10");
            Assert.AreEqual(-10, expression.Evaluate());

            // 5 + -3 = 2
            expression = ExpressionParser.ParseArithmeticExpression("5 + -3");
            Assert.AreEqual(2, expression.Evaluate());

            // -5 - -3 = -2
            expression = ExpressionParser.ParseArithmeticExpression("-5 - -3");
            Assert.AreEqual(-2, expression.Evaluate());

            // -2 * -3 = 6
            expression = ExpressionParser.ParseArithmeticExpression("-2 * -3");
            Assert.AreEqual(6, expression.Evaluate());

            // -1000000 + 2000000 = 1000000
            expression = ExpressionParser.ParseArithmeticExpression("-1000000 + 2000000");
            Assert.AreEqual(1000000, expression.Evaluate());

            // -123456789 - 987654321 = -1111111110
            expression = ExpressionParser.ParseArithmeticExpression("-123456789 - 987654321");
            Assert.AreEqual(-1111111110, expression.Evaluate());
        }

        // 異常系のテストケース
        [TestMethod]
        public void InvalidExpressionTest()
        {
            // 無効なトークン
            Assert.IsFalse(ExpressionParser.TryParseArithmeticExpression("3 + @", out var _));

            // 括弧の不一致
            Assert.IsFalse(ExpressionParser.TryParseArithmeticExpression("(3 + 4", out var _));

            // 演算子の連続
            Assert.IsFalse(ExpressionParser.TryParseArithmeticExpression("3 ++ 4", out var _));

            // オペランド不足
            Assert.IsFalse(ExpressionParser.TryParseArithmeticExpression("3 + ", out var _));

            // 無効な演算子
            Assert.IsFalse(ExpressionParser.TryParseArithmeticExpression("3 $ 4", out var _));
        }
    }
}
