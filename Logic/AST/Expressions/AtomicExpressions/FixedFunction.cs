using System;

public abstract class FixedFunction : AtomicExpression
{
    protected readonly string[] Args;

    protected IContext context;

    protected FixedFunction(params string[] args) : base() => Args = args;

    public override bool Validate(IContext context)
    {
        this.context = context;
        return true;
    }
}

public class Sin : FixedFunction
{
    public Sin() : base("x") => Type = ExpType.Number;

    public override void Evaluate() 
    => Value = MathF.Sin(float.Parse(context.GetVariableValue(Args[0]).Value)).ToString();
}

public class Cos : FixedFunction
{
    public Cos() : base("x") => Type = ExpType.Number;

    public override void Evaluate() 
    => Value = MathF.Cos(float.Parse(context.GetVariableValue(Args[0]).Value)).ToString();
}

public class Log : FixedFunction
{
    public Log() : base("a", "b") => Type = ExpType.Number;

    public override void Evaluate() 
    => Value = MathF.Log(float.Parse(context.GetVariableValue(Args[0]).Value), float.Parse(context.GetVariableValue(Args[1]).Value)).ToString();
}

public class Ln : FixedFunction
{
    public Ln() : base("x") => Type = ExpType.Number;

    public override void Evaluate() 
    => Value = MathF.Log(float.Parse(context.GetVariableValue(Args[0]).Value)).ToString();
}