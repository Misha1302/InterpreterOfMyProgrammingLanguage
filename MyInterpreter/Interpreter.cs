using System.Text;
using GlobalData;
using static ExceptionThrower.ExceptionThrower;
using static ExecutionTime.ExecutionTime;
using static GlobalData.GlobalData;

namespace MyInterpreter;

public class Interpreter
{
    private readonly Code _code;
    private readonly Executor _executor;
    private readonly Parser.Parser _parser;
    private string? _codeString;

    internal int position;

    public Interpreter(Code code)
    {
        _code = code;
        _parser = new Parser.Parser(_code);
        _executor = new Executor(this);
        SortAllEnumerations();
    }

    public void Execute(bool writeDebugInfo = true)
    {
        StringBuilder a = new();
        for (var i = 0; i < Console.BufferWidth; i++) a.Append('-');
        
        _parser.GenerateTokens();
        if (writeDebugInfo)
        {
            Console.Write($"Start\n{a}\n");
            Thread.Sleep(1);
            var executionTime = MethodExecutionTime(Main);
            Console.WriteLine($"\n{a}\nExecution time: " + executionTime);
        }
        else
        {
            Main();
        }
    }

    private int Main()
    {
        if (Tokens[position].IsCommand) _executor.RunCommands();
        return 0;
    }

    private static void SortAllEnumerations()
    {
        EnumerationCommandsAndWords.BubbleEnumerationCommandsSort();
        EnumerationCommandsAndWords.BubbleEnumerationWordSort();
    }
}

public class Executor
{
    private static Interpreter _interpreter;

    public Executor(Interpreter interpreter)
    {
        _interpreter = interpreter;
    }

    public void RunCommands()
    {
        var commandKind = Tokens[_interpreter.position].Kind;
        if (commandKind is TokenKind.print or TokenKind.printLn)
        {
            _interpreter.position += 2;
            StringBuilder outputStr = new();
            var count = 0;
            while (Tokens[_interpreter.position].Kind != TokenKind.ParenthesesClose)
            {
                if (count++ == Program.MaxRepetition) ThrowMaxRepetitions(Program.MaxRepetition);
                switch (Tokens[_interpreter.position].Kind)
                {
                    case TokenKind.Сomma:
                        _interpreter.position++;
                        continue;
                    case TokenKind.String or TokenKind.Number:
                        outputStr.Append(Tokens[_interpreter.position].Value);
                        break;
                    default:
                        PrintCanPrintOnlyStrAndNumbers("\"{Tokens[_interpreter.position].Text}\" is {Tokens[_interpreter.position].Kind}");
                        break;
                }

                _interpreter.position++;
            }

            if (commandKind == TokenKind.printLn) outputStr.Append('\n');
            Console.Write(outputStr);
        }
    }
}