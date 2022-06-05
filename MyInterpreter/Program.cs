namespace MyInterpreter;

static class Program
{
    public const int MaxRepetition = 100_000;

    private static void Main()
    {
        var code = new Code { CodeString = "printLn(\"Res is: \" !1000)" };
        var interpreter = new Interpreter(code);
        interpreter.Execute();
    }
}