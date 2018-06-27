using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCalcForWeb
{
    public class Calculator
    {
        private List<ParsingItem> _correctItemCollect = new List<ParsingItem>();

        public double Evaluate(string src, out int codeError)
        {
            codeError = Parse(src);
            return codeError == 0 ? Calculate() : 0f;
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
                    if (correctItem.IsFilledNumber) return 3;
                    correctItem.Number = tempNumber;
                }
                else if (word.Length == 1)
                {
                    switch (word)
                    {
                        case "+":
                            if (correctItem.Operation != null || correctItem.IsFilledNumber == false)
                                return 3;
                            correctItem.Operation = new MathOperationSum();
                            correctItem.Range = currentRange;
                            break;
                        case "-":
                            if (correctItem.Operation != null || correctItem.IsFilledNumber == false)
                                return 3;
                            correctItem.Operation = new MathOperationMinus();
                            correctItem.Range = currentRange;
                            break;
                        case "*":
                            if (correctItem.Operation != null || correctItem.IsFilledNumber == false)
                                return 3;
                            correctItem.Operation = new MathOperationMultiply();
                            correctItem.Range = currentRange + 1;
                            break;
                        case "/":
                            if (correctItem.Operation != null || correctItem.IsFilledNumber == false)
                                return 3;
                            correctItem.Operation = new MathOperationDivide();
                            correctItem.Range = currentRange + 1;
                            break;
                        case "^":
                            if (correctItem.Operation != null || correctItem.IsFilledNumber == false)
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

                if (correctItem.IsFilled)
                {
                    _correctItemCollect.Add(correctItem);
                    correctItem = new ParsingItem();
                }
            }
            _correctItemCollect.Add(correctItem);
            if (correctItem.IsFilled)
                return 3;
            return 0;
        }

        private double Calculate()
        {
            while (_correctItemCollect.Count > 1)
            {
                var maxRange = _correctItemCollect.Max(C => C.Range);
                for (var i = 0; i < _correctItemCollect.Count - 1; i++)
                {
                    if (maxRange == _correctItemCollect[i].Range)
                    {
                        _correctItemCollect[i + 1].Number = _correctItemCollect[i].Operation.Evaluate(_correctItemCollect[i].Number, _correctItemCollect[i + 1].Number);
                        _correctItemCollect.RemoveAt(i);
                        break;
                    }
                }
            }
            return _correctItemCollect[0].Number;
        }
    }
}
