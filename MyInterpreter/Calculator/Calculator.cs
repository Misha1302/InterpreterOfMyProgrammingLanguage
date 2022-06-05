using System.Numerics;
using static ExceptionThrower.ExceptionThrower;

namespace MyInterpreter;

public static class Calculator
{
    private static readonly string[] DoubleOperands = { "+", "-", "*", "/", "^", "\\", "%" };
    private static readonly string[] SinglesOperands = { "!" };

    public static BigFloat Calculate(string str)
    {
        str = str.Replace('_', '\0');
        if (string.IsNullOrEmpty(str)) SmthCannotBeEmpty("Line for count");
        return Compute(Preparation(str));
    }

    private static int Priority(char x)
    {
        switch (x)
        {
            case '(':
            case ')':
                return -1;

            case '+':
            case '-':
                return 0;

            case '*':
            case '/':
                return 1;

            // case '↓':
            // case '↑':
            default:
                return 2;
        }
    }

    private static bool IsDigit(char digit)
    {
        return decimal.TryParse(digit.ToString(), out _);
    }


    private static List<string?> Preparation(string equation)
    {
        var res = new List<string?>();
        var stack = new List<char>();

        var x = 0;

        var digit = "";

        while (x < equation.Length)
        {
            digit = string.Empty;
            while (x < equation.Length)
                if (IsDigit(equation[x]) || equation[x] == '.')
                {
                    digit += equation[x++];
                }
                else if (equation[x] == '-' && digit == string.Empty && IsDigit(equation[x + 1]))
                {
                    digit += equation[x];
                    x += 1;
                }
                else
                {
                    if (digit != "")
                    {
                        res.Add(digit);
                        digit = "";
                    }


                    if (equation[x] == ')')
                    {
                        while (stack.Count - 1 > 0 && stack[^1] != '(')
                        {
                            res.Add(stack[^1].ToString());
                            stack.RemoveAt(stack.Count - 1);
                        }

                        if (stack.Count - 1 > 0) stack.RemoveAt(stack.Count - 1);
                    }

                    else if (equation[x] == '(')
                    {
                        stack.Add(equation[x]);
                    }
                    else
                    {
                        if (equation[x] == '?') break;

                        var temp = Priority(equation[x]);
                        while (stack.Count != 0 && temp <= Priority(stack[^1]))
                        {
                            res.Add(stack[^1].ToString());
                            stack.RemoveAt(stack.Count - 1);
                        }

                        stack.Add(equation[x]);
                    }

                    break;
                }

            x += 1;
        }

        if (digit != "") res.Add(digit);

        for (var i = stack.Count - 1; i > -1; i--) res.Add(stack[i].ToString());

        return res;
    }

    private static BigFloat Compute(IList<string?> res)
    {
        // foreach (var VARIABLE in res) Console.Write(VARIABLE + " ");
        // Console.WriteLine();

        BigFloat aM;

        for (var x = 1; x < res.Count; x++)
            if (DoubleOperands.Contains(res[x]))
            {
                aM = BigFloat.Parse(res[x - 1]);
                var bM = BigFloat.Parse(res[x - 2]);


                object? sum = res[x] switch
                {
                    "+" => bM + aM,
                    "-" => bM - aM,
                    "*" => bM * aM,
                    "/" => bM / aM,
                    "%" => GetPercent(bM, aM),
                    "\\" => bM % aM,
                    "^" => bM ^ aM,
                    _ => res[x - 2]
                };

                res[x - 2] = sum.ToString().Replace(',', '.');
                res.RemoveAt(x);
                res.RemoveAt(x - 1);
                x -= 2;
            }
            else if (SinglesOperands.Contains(res[x]))
            {
                aM = BigFloat.Parse(res[x - 1]);

                var sum = res[x] switch
                {
                    "!" => Factorial(BigFloat.Round(aM)),
                    //"√" => GetRoot(aM)
                    _ => aM
                };

                res[x - 1] = sum.ToString().Replace(',', '.');
                res.RemoveAt(x);

                x--;
            }


        return BigFloat.Parse(res[0]);
    }

    private static decimal ToRadians(decimal degrees)
    {
        return (decimal)Math.PI / 180 * degrees;
    }

    private static decimal ToDegrees(decimal radians)
    {
        return 180 / (decimal)Math.PI * radians;
    }

    private static BigFloat Factorial(BigFloat digit)
    {
        BigFloat res = 1;
        for (var i = digit; i > 0; i--)
        {
            if (i % 50 == 0) Thread.Sleep(5);
            res *= i;
        }
        return res;
    }

    private static BigFloat GetPercent(BigFloat number, BigFloat percents)
    {
        return number * percents / 100;
    }
}