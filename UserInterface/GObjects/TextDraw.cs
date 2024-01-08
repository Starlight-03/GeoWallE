using Godot;

public partial class TextDraw : GObject
{
    private readonly Vector2 coordinates;

    private readonly string text;

    public TextDraw(Vector2 coordinates, string text)
    {
        this.coordinates = coordinates;
        this.text = text;
    }

    public override void _Draw() => DrawString(new SystemFont(), coordinates, text);
}