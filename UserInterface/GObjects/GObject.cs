using System;
using Godot;

public abstract partial class GObject : Panel
{
	public GObject() => Rotation = MathF.PI;

	public Vector2 Coordinates { get; protected set; } = Vector2.Zero;

	public Color Color { get => color; set => color = value; }

	private Color color = Colors.Black;

    protected readonly float width = 2.0f;

	protected readonly float pixels = 20;
}
