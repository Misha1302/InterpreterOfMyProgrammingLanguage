using System.Text;
using GlobalData;
using static ExceptionThrower.ExceptionThrower;
using static ExecutionTime.ExecutionTime;
using static GlobalData.GlobalData;

namespace MyInterpreter;

public class Interpreter
{
    private readonly Executor _executor;
    private readonly Parser.Parser _parser;

    internal int Position;

    public Interpreter(Code code)
    {
        _parser = new Parser.Parser(code);
        _executor = new Executor(this);
        SortAllEnumerations();
    }

    public void Execute(bool writeDebugInfo = true)
    {
        StringBuilder bufferSeparator = new();
        for (var i = 0; i < Console.BufferWidth; i++) bufferSeparator.Append('-');

        if (writeDebugInfo)
        {
            Console.WriteLine("\nPrepare code...");
            var executionTime = MethodExecutionTime(_parser.GenerateTokens);
            Console.WriteLine($"Code preparation time: {executionTime} ms");

            Console.WriteLine($"Start execute the code\n{bufferSeparator}");
            executionTime = MethodExecutionTime(Main);
            Console.WriteLine($"\n{bufferSeparator}Execution time: {executionTime} ms");
        }
        else
        {
            Main();
        }
    }

    private int Main()
    {
        while (Position < Tokens.Count)
            if (Tokens[Position].IsCommand) Executor.RunCommands();
            else Position++;

        return 0;
    }

    private static void SortAllEnumerations()
    {
        CommandsAndWords.BubbleCommandsSort();
        CommandsAndWords.BubbleWordSort();
    }
}

public class Executor
{
    private static Interpreter _interpreter;

    public Executor(Interpreter interpreter)
    {
        _interpreter = interpreter;
    }

    public static void RunCommands()
    {
        var commandKind = Tokens[_interpreter.Position].Kind;
        if (commandKind is TokenKind.print or TokenKind.printLn)
        {
            _interpreter.Position += 1;
            StringBuilder outputStr = new();
            var count = 0;
            while (Tokens[_interpreter.Position].Kind != TokenKind.ParenthesesClose)
            {
                if (++count == Program.MaxRepetition) ThrowMaxRepetitions(Program.MaxRepetition);
                switch (Tokens[_interpreter.Position].Kind)
                {
                    case TokenKind.Сomma:
                        _interpreter.Position++;
                        continue;
                    case TokenKind.String or TokenKind.Number:
                        outputStr.Append(Tokens[_interpreter.Position].Value);
                        break;
                    default:
                        PrintCanPrintOnlyStrAndNumbers(
                            $"\"{Tokens[_interpreter.Position].Text}\" is {Tokens[_interpreter.Position].Kind}");
                        break;
                }

                _interpreter.Position++;
                if (_interpreter.Position >= Tokens.Count) MaybeForgotParenthese();
            }

            if (commandKind == TokenKind.printLn) outputStr.Append('\n');
            Console.Write(outputStr);
        }
    }
}