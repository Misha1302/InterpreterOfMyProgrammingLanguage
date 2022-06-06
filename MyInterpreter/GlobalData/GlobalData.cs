namespace GlobalData;

public static class GlobalData
{
    public static IList<Token> Tokens = new List<Token>();
    
    public static IList<Variable> Variables = new List<Variable>();
    
    public static string[] MathSymbols =
    {
        "+", "-",
        "*", 
        "/", ":",
        "^", // "^" образуется из "**"
        "%", @"\",  // "\" образуется из "//"; "%" образуются из "% from" и "%from" 
        "!",
        "S" // Sqrt
    };
}
/*
+, -, *, / - выполняют функции, для которых и предназначены
** и ^ - являются степенью чисел
% - вычисляет процент от числа, к примеру "35 %from 50" = 17.5, а "74 % 100" = 74
"\" - деление с математическим округлением, пример: "5 / 2" = 3, так как 5 / 2 = 2.5 => 2.5 ≈ 3
"!" - факториал от числа
*/