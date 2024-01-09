using System;

public partial class Circle : GObject
{
    protected readonly Point center;

    protected readonly float radius;

    public Circle(Point center, float radius)
    {
        this.center = center;
        this.radius = radius * pixels;
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
      double startAngle = (double)Math.Atan2(p1.Y - center.Y, p1.X-center.X);
	  double finalAngle = (double)Math.Atan2(p2.Y - center.Y, p2.X-center.X);   
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