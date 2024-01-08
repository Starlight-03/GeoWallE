using Godot;

public class ColorChange : Statement
{
    public readonly Color Color;

    public ColorChange(Color color, int line) : base(line) => Color = color;

    public override bool Validate(IContext context) => true;
}

public class Restore : ColorChange
{
    public Restore(int line) : base(Colors.Black, line) {}
}