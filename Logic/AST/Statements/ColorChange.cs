using Godot;

public class ColorChange : Statement
{
    public readonly Color Color;

    public ColorChange(int line, Color color) : base(line) => Color = color;

    public override bool Validate(IContext context) => true;
}

public class Restore : ColorChange
{
    public Restore(int line) : base(line, Colors.Black) { }
}