public class LineDef : Expression
{
    private readonly Expression p1;

    private readonly Expression p2;

    protected Point point1;

    protected Point point2;

    public LineDef(Expression p1, Expression p2, int line) : base(line)
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
        point1 = (Point)p1.Object;
        p2.Evaluate();
        point2 = (Point)p2.Object;
        Object = new Line(point1, point2);
    }
}

public class RayDef : LineDef
{
    public RayDef(Expression p1, Expression p2, int line) : base(p1, p2, line) { }

    public override void Evaluate()
    {
        base.Evaluate();
        Object = new Ray(point1, point2);
    }
}

public class SegmentDef : LineDef
{
    public SegmentDef(Expression p1, Expression p2, int line) : base(p1, p2, line) { }

    public override void Evaluate()
    {
        base.Evaluate();
        Object = new Segment(point1, point2);
    }
}