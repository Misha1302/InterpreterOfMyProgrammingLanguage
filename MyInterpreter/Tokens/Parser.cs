using System.Text;
using System.Text.RegularExpressions;
using MyInterpreter;
using static ExceptionThrower.ExceptionThrower;
using static GlobalData.GlobalData;

namespace Parser;

public class Parser
{
    private readonly Code _code;
    private string? _codeString;

    public Parser(Code code)
    {
        _code = code;
    }

    public void GenerateTokens()
    {
        var lexer = new Lexer(_code.CodeString);

        var token = lexer.GetNextToken();
        while (token.Kind != TokenKind.End)
        {
            Tokens.Add(token);
            token = lexer.GetNextToken();
        }

        CombineUnknownTokens();
        PreCalculate();
    }

    private static void CombineUnknownTokens()
    {
        for (var i = 0; i < Tokens.Count; i++)
        {
            if (Tokens[i].Kind != TokenKind.Unknown) continue;
            var count = 0;
            while (i < Tokens.Count - 1 && Tokens[i + 1].Kind == TokenKind.Unknown)
            {
                if (count++ == Program.MaxRepetitions) ThrowMaxRepetitions(Program.MaxRepetitions);
                Tokens[i].Text += Tokens[i + 1].Text;
                Tokens.RemoveAt(i + 1);
            }
        }
    }

    private static void PreCalculate()
    {
        StringBuilder stringToCalculate = new();
        var i = 0;
        while (i < Tokens.Count - 1)
        {
            i++;
            if (Tokens[i].Kind is not (TokenKind.ParenthesesOpen or TokenKind.Number or TokenKind.MathSign))
                continue;

            stringToCalculate.Clear();
            var startIndex = i;

            while (i < Tokens.Count && Tokens[i].Kind is TokenKind.ParenthesesOpen or TokenKind.ParenthesesClose
                       or TokenKind.MathSign or TokenKind.Number)
            {
                stringToCalculate.Append(Tokens[i].Text);
                i++;
            }

            i--;


            if (stringToCalculate.Length < 2 ||
                string.IsNullOrEmpty(stringToCalculate.ToString().Replace("(", "").Replace(")", "")))
            {
                stringToCalculate.Clear();
                continue;
            }

            var result = Calculator.Calculator.Calculate(stringToCalculate.ToString());

            startIndex += CountDontClosedParentheses(stringToCalculate);
            Tokens[startIndex].Kind = TokenKind.Number;
            Tokens[startIndex].Text = result.ToString();
            Tokens[startIndex].Value = result;

            for (var j = i - CountDontOpenedParentheses(stringToCalculate); j > startIndex; j--)
                Tokens.RemoveAt(j);
            i = startIndex + 1;
        }
    }

    private static int CountDontOpenedParentheses(StringBuilder stringBuilder)
    {
        var position = 0;
        var opened = 0;
        while (stringBuilder.Length > position)
        {
            if (stringBuilder[position] == '(')
            {
                opened++;
            }
            else if (opened > 0 && stringBuilder[position] == ')')
            {
                opened--;
                stringBuilder.Remove(position, 1);
            }

            position++;
        }

        return Regex.Match(stringBuilder.ToString(), "\\)").Length;
    }

    private static int CountDontClosedParentheses(StringBuilder stringBuilder)
    {
        var position = 0;
        var opened = 0;
        while (stringBuilder.Length > position)
        {
            if (stringBuilder[position] == '(')
            {
                opened++;
                stringBuilder.Remove(position, 1);
            }
            else if (opened > 0 && stringBuilder[position] == ')')
            {
                opened--;
            }

            position++;
        }

        return Regex.Match(stringBuilder.ToString(), "\\(").Length;
    }
}