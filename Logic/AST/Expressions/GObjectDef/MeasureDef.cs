using System;

public class MeasureDef : Expression
{
    private readonly Expression p1;

    private readonly Expression p2;

    public MeasureDef(int line, Expression p1, Expression p2) : base(line)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public override bool Validate(IContext context)
    {
        if (!p1.Validate(context))
            AddError($"Invalid argument expression at measure definition", p1);
        if (p1.Type is not ExpType.Point)
            AddError($"First argument must be a point, at measure definition");
        if (!p2.Validate(context))
            AddError($"Invalid argument expression at measure definition", p2);
        if (p2.Type is not ExpType.Point)
            AddError($"Second argument must be a point, at measure definition");

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        p1.Evaluate(context);
        Point point1 = (Point)p1.Object;
        p2.Evaluate(context);
        Point point2 = (Point)p2.Object;
        float measure = MathF.Sqrt(MathF.Pow(point2.X - point1.X, 2) + MathF.Pow(point2.Y - point1.Y, 2));
        Value = MathF.Round(MathF.Abs(measure)).ToString();
    }
}