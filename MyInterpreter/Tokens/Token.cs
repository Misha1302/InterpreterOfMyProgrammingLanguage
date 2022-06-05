public class Token
{
    public TokenKind Kind;

    public int Position;

    public string? Text;
    public object? Value;
    public bool IsCommand;

    public Token(TokenKind kind, string? text, int position = -1, object? value = null, bool isCommand = false)
    {
        Kind = kind;
        Text = text;
        Position = position;
        Value = value;
        IsCommand = isCommand;
    }

    public Type Type => Value is not null ? Value.GetType() : typeof(object);
}