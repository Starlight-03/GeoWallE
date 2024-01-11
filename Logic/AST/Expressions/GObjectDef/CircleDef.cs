public class CircleDef : Expression
{
    protected Point c;

    protected float r;

    protected readonly Expression center;

    protected readonly Expression radius;

    public CircleDef(int line, Expression center, Expression radius) : base(line)
    {
        this.center = center;
        this.radius = radius;
        Type = ExpType.Circle;
    }

    public override bool Validate(IContext context)
    {
        if (!center.Validate(context))
            AddError("Invalid argument expression at circle definition", center);
        if (center.Type is not ExpType.Point)
            AddError("First argument must be a point, at circle definition");
        if (!radius.Validate(context))
            AddError("Invalid argument expression at circle definition");
        if (radius.Type is not ExpType.Measure)
            AddError("Second argument must be a measure, at circle definition", radius);

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        center.Evaluate(context);
        c = (Point)center.Object;
        radius.Evaluate(context);
        r = float.Parse(radius.Value);
        Object = new Circle(c, r);
    }
}

public class ArcDef : CircleDef
{
    private readonly Expression p1;

    private readonly Expression p2;

    public ArcDef(int line, Expression center, Expression p1, Expression p2, Expression radius) : base(line, center, radius)
    {
        this.p1 = p1;
        this.p2 = p2;
        Type = ExpType.Arc;
    }

    public override bool Validate(IContext context)
    {
        if (!center.Validate(context))
            AddError("Invalid argument expression at arc definition", center);
        if (center.Type is not ExpType.Point)
            AddError("First argument must be a point, at arc definition");
        if (!p1.Validate(context))
            AddError("Invalid argument expression at arc definition", p1);
        if (p1.Type is not ExpType.Point)
            AddError("Second argument must be a point, at arc definition");
        if (!p2.Validate(context))
            AddError("Invalid argument expression at arc definition", p2);
        if (p2.Type is not ExpType.Point)
            AddError("Third argument must be a point, at arc definition");
        if (!radius.Validate(context))
            AddError("Invalid argument expression at arc definition", radius);
        if (radius.Type is not ExpType.Measure)
            AddError("Fourth argument must be a measure, at arc definition");

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        p1.Evaluate(context);
        Point point1 = (Point)p1.Object;
        p2.Evaluate(context);
        Point point2 = (Point)p2.Object;
        Object = new Arc(c, point1, point2, r);
    }
}