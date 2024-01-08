using Godot;
using System.Collections.Generic;

public class GeoWallE_Program : ASTNode
{
	public List<GObject> DrawingObjects = new();

	private readonly List<Statement> Statements;

	private readonly IContext globalContext = new Context();

	private Color color = Colors.Black;

	public GeoWallE_Program(List<Statement> statements) : base(0) => Statements = statements;

	public override bool Validate(IContext context = null)
	{
		foreach(var st in Statements)
			if (!st.Validate(globalContext))
				return false;

		return true;
	}

	public override void Evaluate()
	{
		foreach (Statement statement in Statements){
			statement.Evaluate();
			if (statement is Draw draw){
				foreach (GObject gObject in draw.Objects){
					if (gObject is not TextDraw)
						gObject.Color = color;
					DrawingObjects.Add(gObject);
				}
			}
			else if (statement is ColorChange colorChange)
				color = colorChange.Color;
		}
	}
}