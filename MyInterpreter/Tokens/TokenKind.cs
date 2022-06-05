public enum TokenKind
{
    End,
    Unknown,
    
    Command,
    Word,
    Сomma,
    
    MathSign,
    Number,
    String,
    
    ParenthesesOpen, // (
    ParenthesesClose, // )
    
    BracketOpen, // {
    BracketClose, // }
    
    // Commands
    print,
    printLn,
    
    // Words
    var
}