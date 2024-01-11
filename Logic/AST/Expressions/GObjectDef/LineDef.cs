public class LineDef : Expression
{
    private readonly Expression p1;

    private readonly Expression p2;

    protected string lineType;

    protected Point point1;

    protected Point point2;

    public LineDef(int line, Expression p1, Expression p2) : base(line)
    {
        lineType = "line";
        this.p1 = p1;
        this.p2 = p2;
    }

    public override bool Validate(IContext context)
    {
        if (!p1.Validate(context))
            AddError($"Invalid argument expression at {lineType} definition", p1);
        if (p1.Type is not ExpType.Point)
            AddError($"First argument must be a point, at {lineType} definition");
        if (!p2.Validate(context))
            AddError($"Invalid argument expression at {lineType} definition", p2);
        if (p2.Type is not ExpType.Point)
            AddError($"Second argument must be a point, at {lineType} definition");

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        p1.Evaluate(context);
        point1 = (Point)p1.Object;
        p2.Evaluate(context);
        point2 = (Point)p2.Object;
        Object = new Line(point1, point2);
    }
}

public class RayDef : LineDef
{
    public RayDef(int line, Expression p1, Expression p2) : base(line, p1, p2) => lineType = "ray";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Object = new Ray(point1, point2);
    }
}

public class SegmentDef : LineDef
{
    public SegmentDef(int line, Expression p1, Expression p2) : base(line, p1, p2) => lineType = "segment";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Object = new Segment(point1, point2);
    }
}