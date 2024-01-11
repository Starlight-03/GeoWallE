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
    private readonly Line l1;

    private readonly Line l2;

    private readonly Line OX = new(new(0,0), new(1,0));

    public Arc(Point center, Point p1, Point p2, float radius) : base(center, radius)
    {
        l1 = new Line(center, p1);
        l2 = new Line(center, p2);
    }

    private float GetAngle(Line l)
    {
        float a = - MathF.Atan((OX.M - l.M) / (1 - OX.M * l.M));
        if (a is float.NaN) a = MathF.PI / 2 * ((l.P2.Y < 0) ? 3 : 1);
        if (a is 0 && l.P2.X < 0) a = MathF.PI;
        return a;
    }

    public override void _Draw() 
    {
      double startAngle = (double)Math.Atan2(P1.Y - center.Y, P1.X-center.X);
	  double finalAngle = (double)Math.Atan2(P2.Y - center.Y, P2.X-center.X);   
      if (startAngle < 0 && startAngle > -Math.PI)
	  {
		startAngle = -startAngle;
	  }
      else if(startAngle > 0)
      {
        startAngle = 2 * Math.PI - startAngle;
      }
      if (finalAngle < 0 && finalAngle > -Math.PI)
	  {
	  	finalAngle = -finalAngle;
	  }
	  else if (finalAngle > 0)
      {
        finalAngle = 2 * Math.PI - finalAngle;
      }
      if(startAngle < finalAngle)
	  {
        startAngle = 2 * Math.PI - startAngle;
        finalAngle = 2 * Math.PI - finalAngle;  
        DrawArc(center.Coordinates, radius, (float)(startAngle), (float)(finalAngle), 64, Color);
      }
	  else
	  {
	  	startAngle = 2 * Math.PI - startAngle;
        finalAngle = 2 * Math.PI - finalAngle;  
	  	DrawArc(center.Coordinates, radius, (float)(finalAngle), (float)(2*Math.PI), 64, Color);
        DrawArc(center.Coordinates, radius, (float)(0), (float)(startAngle), 64, Color); 
      }   
    }
}