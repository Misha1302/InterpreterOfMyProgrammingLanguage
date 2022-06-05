namespace GlobalData;

public class Variable
{
    public readonly string Name;
    private bool _isConst;
    private object? _value;

    public Variable(string name, object? value, bool isConst = false)
    {
        Name = name;
        Value = value;
        IsConst = isConst;
    }

    public Type Type => Value is not null ? Value.GetType() : typeof(object);

    public object? Value
    {
        get => _value;
        set
        {
            if (IsConst)
                ExceptionThrower.ExceptionThrower.AttemptToChangeConstant(Name);
            _value = value;
        }
    }

    public bool IsConst
    {
        get => _isConst;
        set
        {
            if (IsConst)
                ExceptionThrower.ExceptionThrower.AttemptToChangeConstant(Name);
            _isConst = value;
        }
    }
}