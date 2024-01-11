using System.Collections.Generic;
using Godot;

public partial class Point : GObject
{
	public Point(float x, float y) => Coordinates = new Vector2(x * pixels, y * -pixels);

	public float X => Coordinates.X;

	public float Y => Coordinates.Y;

    public override List<Point> GetPoints() { return new List<Point>() { this };}

    public override void _Draw() => DrawCircle(Coordinates, 3.0f, Color);
}
