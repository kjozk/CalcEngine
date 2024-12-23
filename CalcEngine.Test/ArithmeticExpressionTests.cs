using CalcEngine.IO;

namespace CalcEngine.Test
{
    [TestClass]
    public sealed class ArithmeticExpressionTests
    {
        [TestMethod]
        public void AdditionTest()
        {
            IExpression<double, double> expression;

            expression = ExpressionParser.ParseArithmeticExpression("3 + 4");
            Assert.AreEqual(7, expression.Evaluate());

            expression = ExpressionParser.ParseArithmeticExpression("1 + 1");
            Assert.AreEqual(2, expression.Evaluate());
        }

        [TestMethod]
        public void SubtractionTest()
        {
            IExpression<double, double> expression;

            expression = ExpressionParser.ParseArithmeticExpression("5 - 3");
            Assert.AreEqual(2, expression.Evaluate());

            expression = ExpressionParser.ParseArithmeticExpression("10 - 7");
            Assert.AreEqual(3, expression.Evaluate());
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            IExpression<double, double> expression;

            expression = ExpressionParser.ParseArithmeticExpression("3 * 4");
            Assert.AreEqual(12, expression.Evaluate());

            expression = ExpressionParser.ParseArithmeticExpression("2 * 5");
            Assert.AreEqual(10, expression.Evaluate());
        }

        [TestMethod]
        public void DivisionTest()
        {
            IExpression<double, double> expression;

            expression = ExpressionParser.ParseArithmeticExpression("8 / 2");
            Assert.AreEqual(4, expression.Evaluate());

            expression = ExpressionParser.ParseArithmeticExpression("9 / 3");
            Assert.AreEqual(3, expression.Evaluate());
        }

        [TestMethod]
        public void MixedOperationsTest()
        {
            IExpression<double, double> expression;

            expression = ExpressionParser.ParseArithmeticExpression("3 + 5 * 2");
            Assert.AreEqual(13, expression.Evaluate());

            expression = ExpressionParser.ParseArithmeticExpression("10 - 2 / 2");
            Assert.AreEqual(9, expression.Evaluate());
        }

        [TestMethod]
        public void NegativeNumberTest()
        {
            IExpression<double, double> expression;

            expression = ExpressionParser.ParseArithmeticExpression("-10");
            Assert.AreEqual(-10, expression.Evaluate());

            expression = ExpressionParser.ParseArithmeticExpression("5 + -3");
            Assert.AreEqual(2, expression.Evaluate());
        }
    }
}
