# CalcEngine

CalcEngineは、数式文字列を解析して評価するためのライブラリです。

## 使用方法

### 四則演算の解析と評価

```csharp
using CalcEngine.IO;
using CalcEngine.IO.Expressions;

string expression = "3 + 5 * 2";
IExpression<double, double> parsedExpression = ExpressionParser.ParseArithmeticExpression(expression);
double result = parsedExpression.Evaluate();
Console.WriteLine(result); // 出力: 13
```

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。
