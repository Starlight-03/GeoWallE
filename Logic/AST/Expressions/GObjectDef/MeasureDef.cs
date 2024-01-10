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
    => p1 is not null && p1.Validate(context) && p1.Type == ExpType.Point 
        && p2 is not null && p2.Validate(context) && p2.Type == ExpType.Point;

    public override void Evaluate()
    {
        p1.Evaluate();
        Point point1 = (Point)p1.Object;
        p2.Evaluate();
        Point point2 = (Point)p2.Object;
        float measure = MathF.Sqrt(MathF.Pow(point2.X - point1.X, 2) + MathF.Pow(point2.Y - point1.Y, 2));
        Value = MathF.Round(MathF.Abs(measure)).ToString();
    }
}