﻿using CalcEngine.IO;

namespace CalcEngine.Test
{
    [TestClass]
    public sealed class ArithmeticExpressionTests
    {
        // 足し算のテストケース
        [TestMethod]
        public void AdditionTest()
        {
            IExpression<int> expression;

            // 3 + 4 = 7
            expression = ExpressionParser<int>.Parse("3 + 4");
            Assert.AreEqual(7, expression.Evaluate());

            // 1 + 1 = 2
            expression = ExpressionParser<int>.Parse("1 + 1");
            Assert.AreEqual(2, expression.Evaluate());

            // 0 + 0 = 0
            expression = ExpressionParser<int>.Parse("0 + 0");
            Assert.AreEqual(0, expression.Evaluate());

            // -1 + 1 = 0
            expression = ExpressionParser<int>.Parse("-1 + 1");
            Assert.AreEqual(0, expression.Evaluate());

            // 1000000 + 2000000 = 3000000
            expression = ExpressionParser<int>.Parse("1000000 + 2000000");
            Assert.AreEqual(3000000, expression.Evaluate());

            // 123456789 + 987654321 = 1111111110
            expression = ExpressionParser<int>.Parse("123456789 + 987654321");
            Assert.AreEqual(1111111110, expression.Evaluate());
        }

        // 引き算のテストケース
        [TestMethod]
        public void SubtractionTest()
        {
            IExpression<int> expression;

            // 5 - 3 = 2
            expression = ExpressionParser<int>.Parse("5 - 3");
            Assert.AreEqual(2, expression.Evaluate());

            // 10 - 7 = 3
            expression = ExpressionParser<int>.Parse("10 - 7");
            Assert.AreEqual(3, expression.Evaluate());

            // 0 - 0 = 0
            expression = ExpressionParser<int>.Parse("0 - 0");
            Assert.AreEqual(0, expression.Evaluate());

            // -1 - 1 = -2
            expression = ExpressionParser<int>.Parse("-1 - 1");
            Assert.AreEqual(-2, expression.Evaluate());

            // 1000000 - 500000 = 500000
            expression = ExpressionParser<int>.Parse("1000000 - 500000");
            Assert.AreEqual(500000, expression.Evaluate());

            // 987654321 - 123456789 = 864197532
            expression = ExpressionParser<int>.Parse("987654321 - 123456789");
            Assert.AreEqual(864197532, expression.Evaluate());
        }

        // 掛け算のテストケース
        [TestMethod]
        public void MultiplicationTest()
        {
            IExpression<int> expression;

            // 3 * 4 = 12
            expression = ExpressionParser<int>.Parse("3 * 4");
            Assert.AreEqual(12, expression.Evaluate());

            // 2 × 5 = 10
            expression = ExpressionParser<int>.Parse("2 × 5");
            Assert.AreEqual(10, expression.Evaluate());

            // 0 * 5 = 0
            expression = ExpressionParser<int>.Parse("0 * 5");
            Assert.AreEqual(0, expression.Evaluate());

            // -2 × 3 = -6
            expression = ExpressionParser<int>.Parse("-2 × 3");
            Assert.AreEqual(-6, expression.Evaluate());

            // 1000 * 2000 = 2000000
            expression = ExpressionParser<int>.Parse("1000 * 2000");
            Assert.AreEqual(2000000, expression.Evaluate());

            // 12345 * 6789 = 83810205
            expression = ExpressionParser<int>.Parse("12345 * 6789");
            Assert.AreEqual(83810205, expression.Evaluate());
        }

        // 割り算のテストケース
        [TestMethod]
        public void DivisionTest()
        {
            IExpression<int> expression;

            // 8 / 2 = 4
            expression = ExpressionParser<int>.Parse("8 / 2");
            Assert.AreEqual(4, expression.Evaluate());

            // 9 ÷ 3 = 3
            expression = ExpressionParser<int>.Parse("9 ÷ 3");
            Assert.AreEqual(3, expression.Evaluate());

            // 0 / 1 = 0
            expression = ExpressionParser<int>.Parse("0 / 1");
            Assert.AreEqual(0, expression.Evaluate());

            // -6 ÷ 2 = -3
            expression = ExpressionParser<int>.Parse("-6 ÷ 2");
            Assert.AreEqual(-3, expression.Evaluate());

            // 1000000 / 2 = 500000
            expression = ExpressionParser<int>.Parse("1000000 / 2");
            Assert.AreEqual(500000, expression.Evaluate());

            // 123456789 / 3 = 41152263
            expression = ExpressionParser<int>.Parse("123456789 / 3");
            Assert.AreEqual(41152263, expression.Evaluate());
        }

        // 余算のテストケース
        [TestMethod]
        public void ModulusTest()
        {
            IExpression<int> expression;

            // 10 % 3 = 1
            expression = ExpressionParser<int>.Parse("10 % 3");
            Assert.AreEqual(1, expression.Evaluate());

            // 20 % 4 = 0
            expression = ExpressionParser<int>.Parse("20 % 4");
            Assert.AreEqual(0, expression.Evaluate());

            // 7 % 5 = 2
            expression = ExpressionParser<int>.Parse("7 % 5");
            Assert.AreEqual(2, expression.Evaluate());

            // -10 % 3 = -1
            expression = ExpressionParser<int>.Parse("-10 % 3");
            Assert.AreEqual(-1, expression.Evaluate());

            // 10 % -3 = 1
            expression = ExpressionParser<int>.Parse("10 % -3");
            Assert.AreEqual(1, expression.Evaluate());

            // -10 % -3 = -1
            expression = ExpressionParser<int>.Parse("-10 % -3");
            Assert.AreEqual(-1, expression.Evaluate());
        }

        // 混合演算のテストケース
        [TestMethod]
        public void MixedOperationsTest()
        {
            IExpression<double> expression;

            // 3 + 5 * 2 = 13
            expression = ExpressionParser<double>.Parse("3 + 5 * 2");
            Assert.AreEqual(13, expression.Evaluate());

            // 10 - 2 / 2 = 9
            expression = ExpressionParser<double>.Parse("10 - 2 / 2");
            Assert.AreEqual(9, expression.Evaluate());

            // 1 + 2 * 3 - 4 = 3
            expression = ExpressionParser<double>.Parse("2 * 3 + 4 / 2");
            Assert.AreEqual(8, expression.Evaluate());

            // 10 / 2 - 3 * 2 = -1
            expression = ExpressionParser<double>.Parse("10 / 2 - 3 * 2");
            Assert.AreEqual(-1, expression.Evaluate());

            // 1 + 2 * 3 = 7
            expression = ExpressionParser<double>.Parse("1 + 2 * 3");
            Assert.AreEqual(7, expression.Evaluate());

            // (1 + 2) * 3 = 9
            expression = ExpressionParser<double>.Parse("(1 + 2) * 3");
            Assert.AreEqual(9, expression.Evaluate());

            // 1000000 + 2000000 * 3 = 7000000
            expression = ExpressionParser<double>.Parse("1000000 + 2000000 * 3");
            Assert.AreEqual(7000000, expression.Evaluate());

            // (123456789 + 987654321) / 2 = 555555555
            expression = ExpressionParser<double>.Parse("(123456789 + 987654321) / 2");
            Assert.AreEqual(555555555, expression.Evaluate());
        }

        // 負の数のテストケース
        [TestMethod]
        public void NegativeNumberTest()
        {
            IExpression<double> expression;

            // -10 = -10
            expression = ExpressionParser<double>.Parse("-10");
            Assert.AreEqual(-10, expression.Evaluate());

            // 5 + -3 = 2
            expression = ExpressionParser<double>.Parse("5 + -3");
            Assert.AreEqual(2, expression.Evaluate());

            // -5 - -3 = -2
            expression = ExpressionParser<double>.Parse("-5 - -3");
            Assert.AreEqual(-2, expression.Evaluate());

            // -2 * -3 = 6
            expression = ExpressionParser<double>.Parse("-2 * -3");
            Assert.AreEqual(6, expression.Evaluate());

            // -1000000 + 2000000 = 1000000
            expression = ExpressionParser<double>.Parse("-1000000 + 2000000");
            Assert.AreEqual(1000000, expression.Evaluate());

            // -123456789 - 987654321 = -1111111110
            expression = ExpressionParser<double>.Parse("-123456789 - 987654321");
            Assert.AreEqual(-1111111110, expression.Evaluate());
        }

        // 異常系のテストケース
        [TestMethod]
        public void InvalidExpressionTest()
        {
            // 無効なトークン
            Assert.IsFalse(ExpressionParser<double>.TryParse("3 + @", out var _));

            // 括弧の不一致
            Assert.IsFalse(ExpressionParser<double>.TryParse("(3 + 4", out var _));

            // 演算子の連続
            Assert.IsFalse(ExpressionParser<double>.TryParse("3 +/ 4", out var _));

            // オペランド不足
            Assert.IsFalse(ExpressionParser<double>.TryParse("3 + ", out var _));

            // 無効な演算子
            Assert.IsFalse(ExpressionParser<double>.TryParse("3 $ 4", out var _));
        }

        // 複雑な数式のテストケース
        [TestMethod]
        public void ComplexExpressionTest()
        {
            IExpression<int> expression;

            // (3 + 4) * 2 = 14
            expression = ExpressionParser<int>.Parse("(3 + 4) * 2");
            Assert.AreEqual(14, expression.Evaluate());

            // 3 + 4 * 2 = 11
            expression = ExpressionParser<int>.Parse("3 + 4 * 2");
            Assert.AreEqual(11, expression.Evaluate());

            // (1 + 2) * (3 - 4) / 2 = -1
            expression = ExpressionParser<int>.Parse("(1 + 2) * (3 - 4) / 2");
            Assert.AreEqual(-1, expression.Evaluate());

            // 10 / (2 + 3) = 2
            expression = ExpressionParser<int>.Parse("10 / (2 + 3)");
            Assert.AreEqual(2, expression.Evaluate());

            // (2 + 3) * (4 - 1) = 15
            expression = ExpressionParser<int>.Parse("(2 + 3) * (4 - 1)");
            Assert.AreEqual(15, expression.Evaluate());

            // 5 * (6 + 2) - 3 = 37
            expression = ExpressionParser<int>.Parse("5 * (6 + 2) - 3");
            Assert.AreEqual(37, expression.Evaluate());

            // 10 + (2 * 3) - (4 / 2) = 14
            expression = ExpressionParser<int>.Parse("10 + (2 * 3) - (4 / 2)");
            Assert.AreEqual(14, expression.Evaluate());

            // (5 + 3) * (2 - 1) + 4 / 2 = 10
            expression = ExpressionParser<int>.Parse("(5 + 3) * (2 - 1) + 4 / 2");
            Assert.AreEqual(10, expression.Evaluate());

            // 100 / (5 + 5) * 2 = 20
            expression = ExpressionParser<int>.Parse("100 / (5 + 5) * 2");
            Assert.AreEqual(20, expression.Evaluate());

            // (10 + 20) * (30 / 5) - 40 = 140
            expression = ExpressionParser<int>.Parse("(10 + 20) * (30 / 5) - 40");
            Assert.AreEqual(140, expression.Evaluate());
        }

        // 複雑な数式のテストケース (double)
        [TestMethod]
        public void ComplexExpressionDoubleTest()
        {
            IExpression<double> expression;

            // (3.5 + 4.5) * 2 = 16
            expression = ExpressionParser<double>.Parse("(3.5 + 4.5) * 2");
            Assert.AreEqual(16, expression.Evaluate());

            // 3.2 + 4.8 * 2 = 12.8
            expression = ExpressionParser<double>.Parse("3.2 + 4.8 * 2");
            Assert.AreEqual(12.8, expression.Evaluate());

            // (1.1 + 2.2) * (3.3 - 4.4) / 2.2 = -1.65（誤差を許容）
            expression = ExpressionParser<double>.Parse("(1.1 + 2.2) * (3.3 - 4.4) / 2.2");
            Assert.AreEqual(-1.65, expression.Evaluate(), 0.0001);

            // 10.5 / (2.5 + 3.5) = 1.75
            expression = ExpressionParser<double>.Parse("10.5 / (2.5 + 3.5)");
            Assert.AreEqual(1.75, expression.Evaluate(), 0.0001);

            // (2.2 + 3.3) * (4.4 - 1.1) = 18.15
            expression = ExpressionParser<double>.Parse("(2.2 + 3.3) * (4.4 - 1.1)");
            Assert.AreEqual(18.15, expression.Evaluate(), 0.0001);

            // 5.5 * (6.6 + 2.2) - 3.3 = 45.1
            expression = ExpressionParser<double>.Parse("5.5 * (6.6 + 2.2) - 3.3");
            Assert.AreEqual(45.1, expression.Evaluate(), 0.0001);

            // 10.1 + (2.2 * 3.3) - (4.4 / 2.2) = 15.36
            expression = ExpressionParser<double>.Parse("10.1 + (2.2 * 3.3) - (4.4 / 2.2)");
            Assert.AreEqual(15.36, expression.Evaluate(), 0.0001);

            // (5.5 + 3.3) * (2.2 - 1.1) + 4.4 / 2.2 = 11.68
            expression = ExpressionParser<double>.Parse("(5.5 + 3.3) * (2.2 - 1.1) + 4.4 / 2.2");
            Assert.AreEqual(11.68, expression.Evaluate(), 0.0001);

            // 100.5 / (5.5 + 5.5) * 2.2 = 20.1
            expression = ExpressionParser<double>.Parse("100.5 / (5.5 + 5.5) * 2.2");
            Assert.AreEqual(20.1, expression.Evaluate(), 0.0001);

            // (10.1 + 20.2) * (30.3 / 5.5) - 40.4 = 126.52545...
            expression = ExpressionParser<double>.Parse("(10.1 + 20.2) * (30.3 / 5.5) - 40.4");
            Assert.AreEqual(126.52545, expression.Evaluate(), 0.0001);
        }
    }
}
