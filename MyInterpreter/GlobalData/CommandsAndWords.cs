namespace GlobalData;

public static class CommandsAndWords
{
    public static readonly (string @string, TokenKind @enum)[] Commands =
    {
        ("print", TokenKind.print),
        ("printLn", TokenKind.printLn)
    };

    public static readonly (string @string, TokenKind @enum)[] Words =
    {
        ("var", TokenKind.var)
    };

    public static void BubbleCommandsSort()
    {
        for (var i = 0; i < Commands.Length; i++)
        for (var j = 0; j < Commands.Length - 1; j++)
        {
            Commands[j + 1].@string = Commands[j + 1].@string.ToUpper();
            Commands[j].@string = Commands[j].@string.ToUpper();
            if (Commands[j].@string.Length < Commands[j + 1].@string.Length)
                (Commands[j + 1], Commands[j]) = (Commands[j], Commands[j + 1]);
        }
    }

    public static void BubbleWordSort()
    {
        for (var i = 0; i < Words.Length; i++)
        for (var j = 0; j < Words.Length - 1; j++)
        {
            Words[j + 1].@string = Words[j + 1].@string.ToUpper();
            Words[j].@string = Words[j].@string.ToUpper();
            if (Words[j].@string.Length < Words[j + 1].@string.Length)
                (Words[j + 1], Words[j]) = (Words[j], Words[j + 1]);
        }
    }
}