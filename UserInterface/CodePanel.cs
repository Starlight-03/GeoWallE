using Godot;

public partial class CodePanel : TextEdit
{
	public override void _Ready()
	{
		base._Ready();
		GrabFocus();
	}
}
