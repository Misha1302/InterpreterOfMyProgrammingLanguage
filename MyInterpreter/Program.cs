namespace MyInterpreter;

public static class Program
{
    public const int MaxRepetition = 100_000;

    private static void Main()
    {
        var code = new Code { CodeString = "printLn(5**2 \" \" sqrt 25)" };
        var interpreter = new Interpreter(code);
        interpreter.Execute();
    }
}