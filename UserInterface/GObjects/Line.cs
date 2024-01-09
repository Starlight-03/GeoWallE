using Godot;

public partial class Line : GObject
{
    public Point P1 { get; protected set; }

    public Point P2 { get; protected set; }

    public Line(Point p1, Point p2)
    {
        P1 = p1;
        P2 = p2;
    }

    public float M => (P2.Y - P1.Y) / (P2.X - P1.X);

    protected readonly int k = 100;

    public override void _Draw()
    {
        DrawLine(P1.Coordinates, new Vector2(P2.X * k, P2.Y * k), Color, width, true);
        DrawLine(P1.Coordinates, new Vector2(P2.X * -k, P2.Y * -k), Color, width, true);
    }
}

public partial class Ray : Line
{
    public Ray(Point p1, Point p2) : base(p1, p2) { }

    public override void _Draw() => DrawLine(P1.Coordinates, new Vector2(P2.X * k, P2.Y * k), Color, width, true);
}

public partial class Segment : Line
{
    public Segment(Point p1, Point p2) : base(p1, p2) { }

    public override void _Draw() => DrawLine(P1.Coordinates, P2.Coordinates, Color, width, true);
}