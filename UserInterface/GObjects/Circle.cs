using System;
using System.Collections.Generic;

public partial class Circle : GObject
{
    protected readonly Point center;

    protected readonly float radius;

    public Circle(Point center, float radius)
    {
        this.center = center;
        this.radius = radius * pixels;
    }

    public override List<Point> GetPoints()
    {
        // List<Point> points = new();
        // for (float x = radius*MathF.Cos(0); x )
        throw new NotImplementedException();
    }

    public override void _Draw() => DrawArc(center.Coordinates, radius, 0, MathF.PI * 2, 1000, Color, width, true);
}

public partial class Arc : Circle
{
    private readonly Point p1;

    private readonly Point p2;

    public Arc(Point center, Point p1, Point p2, float radius) : base(center, radius)
    {
        this.p1= p1;
        this.p2= p2;
    }

    public override void _Draw() 
    {
        float startAngle = MathF.Atan2(p1.Y - center.Y, p1.X-center.X);
        float finalAngle = MathF.Atan2(p2.Y - center.Y, p2.X-center.X);   
        if (startAngle < 0 && startAngle > -Math.PI)
            startAngle = -startAngle;
        else if(startAngle > 0)
            startAngle = 2 * MathF.PI - startAngle;
        if (finalAngle < 0 && finalAngle > -Math.PI)
            finalAngle = -finalAngle;
        else if (finalAngle > 0)
            finalAngle = 2 * MathF.PI - finalAngle;
        if(startAngle < finalAngle){
            startAngle = 2 * MathF.PI - startAngle;
            finalAngle = 2 * MathF.PI - finalAngle;  
            DrawArc(center.Coordinates, radius, startAngle, finalAngle, 1000, Color, width, true);
        }
        else{
            startAngle = 2 * MathF.PI - startAngle;
            finalAngle = 2 * MathF.PI - finalAngle;  
            DrawArc(center.Coordinates, radius, finalAngle, 2 * MathF.PI, 1000, Color, width, true);
            DrawArc(center.Coordinates, radius, 0, startAngle, 1000, Color, width, true);
        }
    }
}