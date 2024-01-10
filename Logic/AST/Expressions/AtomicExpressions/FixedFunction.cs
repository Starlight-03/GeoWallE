using System;
using System.Collections.Generic;

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

public class Intersect : FixedFunction
{
    public Intersect() : base("f1", "f2") => Type = ExpType.Sequence;

    public override void Evaluate() { }
}

public class Points : FixedFunction
{
    public Points() : base("f") => Type = ExpType.Sequence;

    public override void Evaluate() { }
}

public class Samples : FixedFunction
{
    private readonly List<Expression> samples;

    public Samples()
    {
        Type = ExpType.Sequence;
        samples = new List<Expression>();

        Random random = new();
        for (int i = 0; i < 999; i++)
            samples.Add(new PointDef(new Number(random.Next().ToString()), new Number(random.Next().ToString()), 0));
    }

    public override void Evaluate() => Seq = new Sequence(0, samples, ExpType.Point);
}

public class Randoms : FixedFunction
{
    private readonly List<Expression> randoms;

    public Randoms()
    {
        Type = ExpType.Sequence;
        randoms = new List<Expression>();

        Random random = new();
        for (int i = 0; i < 999; i++) randoms.Add(new Number(random.Next().ToString()));
    }

    public override void Evaluate() => Seq = new Sequence(0, randoms, ExpType.Number);
}