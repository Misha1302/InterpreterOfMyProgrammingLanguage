using System.Text.RegularExpressions;
using static ExceptionThrower.ExceptionThrower;

namespace MyInterpreter;

public class Code
{
    private string? _codeString;

    public Code(string? codeString = null)
    {
        if(codeString is not null)
            CodeString = codeString;
    }

    public string? CodeString
    {
        get => _codeString;
        set => _codeString = CleanCode(value);
    }

    public void AddLine(string? line)
    {
        CodeString += line;
    }

    private static string CleanCode(string? codeString)
    {
        if (string.IsNullOrEmpty(codeString)) SmthCannotBeEmpty("Code");
        var splitCode = codeString.Split('"');
        for (var i = 0; i < splitCode.Length; i++)
            if (i % 2 == 0)
            {
                splitCode[i] = splitCode[i].ToUpper();
                splitCode[i] = Regex.Replace(splitCode[i], "(\n|;| |\\t)", "");
                splitCode[i] = Regex.Replace(splitCode[i], "(?<![a-zA-Z])ENDL(?![a-zA-Z])", "\"\n\"");
                splitCode[i] = Regex.Replace(splitCode[i], "%( |)FROM", "%");
                splitCode[i] = Regex.Replace(splitCode[i], "//", "\\");
                splitCode[i] = Regex.Replace(splitCode[i], "\\*\\*", "^");
                splitCode[i] = Regex.Replace(splitCode[i], "(?<![a-zA-Z])END(?![a-zA-Z])", "☼");
            }
            else
            {
                splitCode[i] = Regex.Unescape(splitCode[i]);
            }

        return string.Join('"', splitCode);
    }
}