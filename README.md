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

### 論理演算の解析と評価

```csharp
using CalcEngine.IO;
using CalcEngine.IO.Expressions;

string expression = "真 ⊕ 偽";
IExpression<bool> parsedExpression = ExpressionParser<bool>.ParseLogical(expression);
bool result = parsedExpression.Evaluate();
Console.WriteLine(result); // 出力: False
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
- `≡` : 同値

### 論理演算子
- `AND` : 論理積
- `OR` : 論理和
- `XOR` : 排他的論理和
- `NOT` : 否定
- `&&` : 論理積
- `||` : 論理和
- `∧` : 論理積
- `∨` : 論理和
- `¬` : 否定
- `NAND` : 否定論理積
- `NOR` : 否定論理和
- `XNOR` : 否定排他的論理和
- `IMPLIES` : 含意
- `EQUIV` : 同値
- `⊅` : 否定論理積
- `⊃` : 含意
- `↑` : 否定論理積
- `⊄` : 否定論理積
- `↚` : 否定含意
- `←` : 含意
- `⊂` : 否定論理積
- `P↮` : 不等
- `≢` : 不等
- `⊕` : 排他的論理和
- `⊻` : 排他的論理和
- `↔` : 同値
- `IFF` : 同値
- `↓` : 否定論理和
- `⇔` : 同値
- `∥` : 論理和
- `+` : 論理和
- `·` : 論理積
- `˜` : 否定
- `!` : 否定

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。
