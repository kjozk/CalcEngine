# CalcEngine

CalcEngineは、数式文字列を解析して評価するためのライブラリです。

## 使用方法

### 四則演算の解析と評価

```csharp
using CalcEngine.IO;
using CalcEngine.IO.Expressions;

string expression = "3 + 5 * 2";
IExpression<double> parsedExpression = ExpressionParser<double>.Parse(expression);
double result = parsedExpression.Evaluate();
Console.WriteLine(result); // 出力: 13
```

### 比較演算の解析と評価

```csharp
using CalcEngine.IO;
using CalcEngine.IO.Expressions;

string expression = "5 > 3";
IExpression<bool> parsedExpression = ExpressionParser<double>.ParseComparison(expression);
bool result = parsedExpression.Evaluate();
Console.WriteLine(result); // 出力: True
```

## 対応している演算子

### 算術演算子
- `+` : 加算
- `-` : 減算
- `*` : 乗算
- `×` : 乗算
- `/` : 除算
- `÷` : 除算
- `%` : 余算

### 比較演算子
- `>` : 大なり
- `<` : 小なり
- `>=` : 大なり等しい
- `≧` : 大なり等しい
- `<=` : 小なり等しい
- `≦` : 小なり等しい
- `==` : 等しい
- `!=` : 等しくない
- `≠` : 等しくない

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。
