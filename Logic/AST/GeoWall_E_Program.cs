using Godot;
using System.Collections.Generic;

public class GeoWallE_Program : ASTNode
{
	public List<GObject> DrawingObjects { get; private set; } = new();

	public IContext GlobalContext { get; private set; } = new Context();

	private readonly List<Statement> Statements;

	private Color color = Colors.Black;

	public GeoWallE_Program(List<Statement> statements) : base(0) => Statements = statements;

	public override bool Validate(IContext context = null)
	{
		foreach(var st in Statements){
			if (st is not FuncDef)
				continue;
			else if (!st.Validate(GlobalContext))
				return false;
		}
		foreach(var st in Statements){
			if (st is FuncDef)
				continue;
			else if (!st.Validate(GlobalContext))
				return false;
		}
		return true;
	}

	public override void Evaluate(IContext context = null)
	{
		foreach (Statement statement in Statements){
			statement.Evaluate(GlobalContext);
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
