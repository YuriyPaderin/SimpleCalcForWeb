using System;

namespace SimpleCalcForWeb
{
    public interface IMathOperation
    {
        double? Evaluate(double? a, double? b);
    }

    public class MathOperationSum : IMathOperation
    {
        public double? Evaluate(double? a, double? b)
        {
            return a + b;
        }
    }

    public class MathOperationMinus : IMathOperation
    {
        public double? Evaluate(double? a, double? b)
        {
            return a - b;
        }
    }

    public class MathOperationMultiply : IMathOperation
    {
        public double? Evaluate(double? a, double? b)
        {
            return a * b;
        }
    }

    public class MathOperationDivide : IMathOperation
    {
        public double? Evaluate(double? a, double? b)
        {
            return b != 0 ? a / b : null;
        }
    }

    public class MathOperationPow : IMathOperation
    {
        public double? Evaluate(double? a, double? b)
        {
            double? pow = 1;
            for (int i = 0; i <= b; i++)
                pow *= a;
            return pow;
        }
    }
}
