namespace ExceptionThrower;

public static class ExceptionThrower
{
    public static void ThrowMaxRepetitions(object MaxRepetition)
    {
        throw new Exception($"!Maximum number of repetitions has been exceeded ({MaxRepetition})");
    }

    public static void AttemptToFindFactorialFromNonInteger(object nonInteger)
    {
        throw new Exception($"!Attempt to find a factorial from a float: {nonInteger}");
    }

    public static void AttemptToChangeConstant(string nameConstant = "")
    {
        var exception = "!Attempt to change a constant";
        if (nameConstant != "") exception += $": {nameConstant}";
        throw new Exception(exception);
    }

    public static void NumberCannotContainMoreThanOneDot()
    {
        throw new Exception("!Number cannot contain more than one dot");
    }

    public static void MaybeForgotParenthese(string strBeforeException = "!")
    {
        if (strBeforeException[0] != '!') strBeforeException = '!' + strBeforeException;
        throw new Exception(strBeforeException + "Maybe you forgot \")\"");
    }

    public static void ClosingDoubleQuotationMarkNeeded()
    {
        throw new Exception("!The end of the string was not found (a closing double quotation mark (\") is needed)");
    }

    public static void PrintCanPrintOnlyStrAndNumbers(string strBeforeException = "!")
    {
        if (strBeforeException[0] != '!') strBeforeException = '!' + strBeforeException;
        throw new Exception(strBeforeException + " command \"print\" can print only numbers and strings");
    }

    public static void SmthCannotBeEmpty(string smth)
    {
        throw new Exception($"!{smth} cannot be empty");
    }
}