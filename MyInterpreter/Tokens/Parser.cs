using System.Text;
using MyInterpreter;
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
        _codeString = _code.CodeString;

        var lexer = new Lexer(_codeString);

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
                if (count++ == Program.MaxRepetition) ExceptionThrower.ExceptionThrower.ThrowMaxRepetitions(Program.MaxRepetition);
                Tokens[i].Text += Tokens[i + 1].Text;
                Tokens.RemoveAt(i + 1);
            }
        }
    }

    private static void PreCalculate()
    {
        var commandWas = false;
        for (var i = 0; i < Tokens.Count; i++)
        {
            if (Tokens[i].IsCommand)
            {
                commandWas = true;
                i+=2;
            }

            if (Tokens[i].Kind is TokenKind.ParenthesesOpen or TokenKind.Number or TokenKind.MathSign)
            {
                StringBuilder stringToCalculate = new();
                var startIndex = i;
                while (Tokens.Count > i && Tokens[i].Kind is TokenKind.ParenthesesOpen or TokenKind.ParenthesesClose
                       or TokenKind.MathSign or TokenKind.Number)
                {
                    stringToCalculate.Append(Tokens[i].Text);
                    i++;
                }

                if (commandWas)
                {
                    i-=2;
                    stringToCalculate.Remove(stringToCalculate.Length - 1, 1);
                }

                var result = Calculator.Calculate(stringToCalculate.ToString());

                Tokens[startIndex].Kind = TokenKind.Number;
                Tokens[startIndex].Text = result.ToString();
                Tokens[startIndex].Value = result;

                for (var j = i; j > startIndex; j--)
                    Tokens.RemoveAt(j);
            }
        }
    }
}