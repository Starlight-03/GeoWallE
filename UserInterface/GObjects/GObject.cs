using Godot;
using System.Collections.Generic;

public abstract partial class GObject : Panel
{
	public Vector2 Coordinates { get; protected set; } = Vector2.Zero;

	public abstract List<Point> GetPoints();

	public Color Color { get => color; set => color = value; }

	private Color color = Colors.Black;

    protected readonly float width = 2.0f;

	protected readonly float pixels = 5;
}
