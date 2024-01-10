public class CircleDef : Expression
{
    protected Point c;

    protected float r;

    private readonly Expression center;

    private readonly Expression radius;

    public CircleDef(int line, Expression center, Expression radius) : base(line)
    {
        this.center = center;
        this.radius = radius;
        Type = ExpType.Circle;
    }

    public override bool Validate(IContext context)
    => center is not null && center.Validate(context) && center.Type == ExpType.Point 
        && radius is not null && radius.Validate(context) && radius.Type == ExpType.Measure;

    public override void Evaluate()
    {
        center.Evaluate();
        c = (Point)center.Object;
        radius.Evaluate();
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
    => base.Validate(context) 
        && p1 is not null && p1.Validate(context) 
        && p2 is not null && p2.Validate(context);

    public override void Evaluate()
    {
        base.Evaluate();
        p1.Evaluate();
        Point point1 = (Point)p1.Object;
        p2.Evaluate();
        Point point2 = (Point)p2.Object;
        Object = new Arc(c, point1, point2, r);
    }
}