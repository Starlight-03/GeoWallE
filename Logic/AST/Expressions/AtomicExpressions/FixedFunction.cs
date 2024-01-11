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

    public override void Evaluate(IContext context) 
    => Value = MathF.Sin(float.Parse(context.GetVariableValue(Args[0]).Value)).ToString();
}

public class Cos : FixedFunction
{
    public Cos() : base("x") => Type = ExpType.Number;

    public override void Evaluate(IContext context) 
    => Value = MathF.Cos(float.Parse(context.GetVariableValue(Args[0]).Value)).ToString();
}

public class Log : FixedFunction
{
    public Log() : base("a", "b") => Type = ExpType.Number;

    public override void Evaluate(IContext context) 
    => Value = MathF.Log(float.Parse(context.GetVariableValue(Args[0]).Value), float.Parse(context.GetVariableValue(Args[1]).Value)).ToString();
}

public class Ln : FixedFunction
{
    public Ln() : base("x") => Type = ExpType.Number;

    public override void Evaluate(IContext context) 
    => Value = MathF.Log(float.Parse(context.GetVariableValue(Args[0]).Value)).ToString();
}

public class Intersect : FixedFunction
{
    public Intersect() : base("f1", "f2") => Type = ExpType.Sequence;

    public override bool Validate(IContext context)
    {
        base.Validate(context);
        ExpType type = context.GetVariableType(Args[0]);
        if (type is ExpType.NotSet || type is ExpType.Number || type is ExpType.Text || type is ExpType.Undefined)
            AddError($"Function points receives a geometric object, not a {type}");
        type = context.GetVariableType(Args[1]);
        if (type is ExpType.NotSet || type is ExpType.Number || type is ExpType.Text || type is ExpType.Undefined)
            AddError($"Function points receives a geometric object, not a {type}");
        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        List<Expression> points = new();
        List<Point> _points1 = context.GetVariableValue(Args[0]).Object.GetPoints();
        List<Point> _points2 = context.GetVariableValue(Args[1]).Object.GetPoints();

        foreach (Point point1 in _points1){
            foreach (Point point2 in _points2)
                if (point1.X == point2.X && point1.Y == point2.Y)
                    points.Add(new PointDef(0, new Number(point1.X), new Number(point1.Y)));
        }

        Seq = new Sequence(0, points);
    }
}

public class Points : FixedFunction
{
    public Points() : base("f") => Type = ExpType.Sequence;

    public override bool Validate(IContext context)
    {
        base.Validate(context);
        ExpType type = context.GetVariableType(Args[0]);
        if (type is ExpType.NotSet || type is ExpType.Number || type is ExpType.Text || type is ExpType.Undefined)
            AddError($"Function points receives a geometric object, not a {type}");
        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        List<Expression> points = new();
        List<Point> _points = context.GetVariableValue(Args[0]).Object.GetPoints();
        Random random = new();
        for (int k = _points.Count; k >= 0; k--){
            int i = random.Next(_points.Count);
            Point point = _points[i];
            points.Add(new PointDef(0, new Number(point.X), new Number(point.Y)));
            _points.RemoveAt(i);
        }
        Seq = new Sequence(0, points);
    }
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
            samples.Add(new PointDef(0, new Number(random.Next().ToString()), new Number(random.Next().ToString())));
    }

    public override void Evaluate(IContext context) => Seq = new Sequence(0, samples, ExpType.Point);
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

    public override void Evaluate(IContext context) => Seq = new Sequence(0, randoms, ExpType.Number);
}