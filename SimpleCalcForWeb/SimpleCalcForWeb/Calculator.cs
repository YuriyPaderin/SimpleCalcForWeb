using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCalcForWeb
{
    public class Calculator
    {
        private List<ParsingItem> _correctItemList = new List<ParsingItem>();

        public double? Evaluate(string src, out int errorCode)
        {
            errorCode = Parse(src);
            if (errorCode == 0)
            {
                var result = Calculate();
                if (result == null)
                    errorCode = 4;

                return result;
            }

            return null;
        }

        private int Parse(string src)
        {
            var currentRange = 0;
            var words = src.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var correctItem = new ParsingItem();

            foreach (var word in words)
            {
                double tempNumber;
                if (double.TryParse(word, out tempNumber))
                {
                    if (correctItem.Number != null) return 3;
                    correctItem.Number = tempNumber;
                }
                else if (word.Length == 1)
                {
                    switch (word)
                    {
                        case "+":
                            if (correctItem.Operation != null || correctItem.Number == null)
                                return 3;
                            correctItem.Operation = new MathOperationSum();
                            correctItem.Range = currentRange;
                            break;
                        case "-":
                            if (correctItem.Operation != null || correctItem.Number == null)
                                return 3;
                            correctItem.Operation = new MathOperationMinus();
                            correctItem.Range = currentRange;
                            break;
                        case "*":
                            if (correctItem.Operation != null || correctItem.Number == null)
                                return 3;
                            correctItem.Operation = new MathOperationMultiply();
                            correctItem.Range = currentRange + 1;
                            break;
                        case "/":
                            if (correctItem.Operation != null || correctItem.Number == null)
                                return 3;
                            correctItem.Operation = new MathOperationDivide();
                            correctItem.Range = currentRange + 1;
                            break;
                        case "^":
                            if (correctItem.Operation != null || correctItem.Number != null)
                                return 3;
                            correctItem.Operation = new MathOperationPow();
                            correctItem.Range = currentRange + 2;
                            break;
                        case "(":
                            currentRange += 3;
                            break;
                        case ")":
                            currentRange -= 3;
                            break;
                        default:
                            return 1;
                    }
                }
                else
                {
                    return 2;
                }

                if (correctItem.Number != null && correctItem.Operation != null)
                {
                    _correctItemList.Add(correctItem);
                    correctItem = new ParsingItem();
                }
            }
            _correctItemList.Add(correctItem);
            if (correctItem.Number != null && correctItem.Operation != null)
                return 3;
            return 0;
        }

        private double? Calculate()
        {
            while (_correctItemList.Count > 1)
            {
                var maxRange = _correctItemList.Max(C => C.Range);
                for (var i = 0; i < _correctItemList.Count - 1; i++)
                {
                    if (maxRange == _correctItemList[i].Range)
                    {
                        _correctItemList[i + 1].Number = _correctItemList[i].Operation.Evaluate(_correctItemList[i].Number, _correctItemList[i + 1].Number);
                        _correctItemList.RemoveAt(i);
                        break;
                    }
                }
            }
            return _correctItemList[0].Number;
        }
    }
}
