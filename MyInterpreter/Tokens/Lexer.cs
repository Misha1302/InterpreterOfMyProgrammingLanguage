using System.Numerics;
using System.Text;
using GlobalData;
using static ExceptionThrower.ExceptionThrower;

namespace MyInterpreter;

public class Lexer
{
    private readonly string _code;
    private int _lastMethodIndex;
    private int _position;

    public Lexer(string code)
    {
        if (string.IsNullOrEmpty(code)) SmthCannotBeEmpty("Code");
        _code = code;
    }

    public Token GetNextToken()
    {
        if (_position >= _code.Length) return new Token(TokenKind.End, "END", _position++);
        if (char.IsDigit(_code[_position]) || (_code[_position] == '-' && char.IsDigit(_code[_position + 1])))
            return ReturnNumber();
        if (GlobalData.GlobalData.MathSymbols.Contains(_code[_position].ToString()))
            return new Token(TokenKind.MathSign, _code[_position].ToString(), _position++);
        switch (_code[_position])
        {
            case '"':
                var startIndex = _position;
                do
                {
                    _position++;
                    if (_position == _code.Length) ClosingDoubleQuotationMarkNeeded();
                } while (_code[_position] != '"');

                _position++;

                var strText = _code.Substring(startIndex, _position - startIndex - 1);
                var strValue = strText.Substring(1, strText.Length - 1);
                return new Token(TokenKind.String, strText, _position, strValue);
            case ',':
                return new Token(TokenKind.Сomma, ",", _position++);
            case '(':
                return new Token(TokenKind.ParenthesesOpen, "(", _position++);
            case ')':
                return new Token(TokenKind.ParenthesesClose, ")", _position++);
            case '☼':
                return new Token(TokenKind.End, "END", _position = int.MaxValue);
            default:
                return ReturnCommandOrUnknown();
        }
    }

    private Token ReturnCommandOrUnknown()
    {
        for (var i = 0; i < CommandsAndWords.Commands.Length; i++)
            if (_code[_position..].IndexOf(CommandsAndWords.Commands[i].@string, StringComparison.Ordinal) == 0)
                return new Token(CommandsAndWords.Commands[i].@enum,
                    CommandsAndWords.Commands[i].@string,
                    _position += CommandsAndWords.Commands[i].@string.Length, isCommand: true);

        return new Token(TokenKind.Unknown, _code[_position].ToString(), _position++);
    }

    private Token ReturnNumber()
    {
        var amountDots = 0;
        var number = new StringBuilder();
        do
        {
            if (_code[_position] != '_') number.Append(_code[_position]);
            if (_code[_position] == '.')
                if (++amountDots == 2)
                    NumberCannotContainMoreThanOneDot();
            _position++;
            if (_code.Length == _position) MaybeForgotParenthese("Index was outside the bounds of the code. ");
        } while (char.IsDigit(_code[_position]) || _code[_position] == '.' || _code[_position] == '_');

        return new Token(TokenKind.Number, number.ToString(), _position, BigFloat.Parse(number.ToString()));
    }
}