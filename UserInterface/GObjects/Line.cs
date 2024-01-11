using System.Collections.Generic;
using Godot;

public partial class Segment : GObject
{
    public Point P1 { get; protected set; }

    public Point P2 { get; protected set; }

    protected float A;

    protected float B;

    protected float C;

    public Segment(Point p1, Point p2)
    {
        P1 = p1;
        P2 = p2;
    }

    public float M => (P2.Y - P1.Y) / (P2.X - P1.X);

    public override List<Point> GetPoints()
    {
        List<Point> points = new();
        float a, b;
        a = P2.Y - P1.Y;
        b = P2.X - P1.X;
        A = -b;
        B = a;
        C = b*P1.X - a*P1.Y;

        for (float x = P1.X; x < P2.Y; x++)
            points.Add(new Point(x, -(A*x + C)/B));

        return points;
    }

    public override void _Draw() => DrawLine(P1.Coordinates, P2.Coordinates, Color, width, true);
}

public partial class Ray : Segment
{
    public Ray(Point p1, Point p2) : base(p1, p2) { }

    protected readonly int k = 100;

    public override List<Point> GetPoints()
    {
        List<Point> points = new();
        float a, b;
        a = P2.Y - P1.Y;
        b = P2.X - P1.X;
        A = -b;
        B = a;
        C = b*P1.X - a*P1.Y;

        for (float x = P1.X; x < P2.Y*k; x++)
            points.Add(new Point(x, -(A*x + C)/B));

        return points;
    }

    public override void _Draw() => DrawLine(P1.Coordinates, new Vector2(P2.X * k, P2.Y * k), Color, width, true);
}

public partial class Line : Ray
{
    
    public Line(Point p1, Point p2) : base(p1, p2) { }

    public override List<Point> GetPoints()
    {
        List<Point> points = base.GetPoints();

        for (float x = P1.X; x > -P2.Y*k; x++)
            points.Add(new Point(x, -(A*x + C)/B));

        return points;
    }

    public override void _Draw()
    {
        base._Draw();
        DrawLine(P1.Coordinates, new Vector2(P2.X * -k, P2.Y * -k), Color, width, true);
    }
}