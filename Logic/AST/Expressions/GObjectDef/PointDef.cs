public class PointDef : Expression
{
	private readonly Expression x;

	private readonly Expression y;

	public PointDef(int line, Expression x, Expression y) : base(line)
	{
		this.x = x;
		this.y = y;
		Type = ExpType.Point;
	}

	public override bool Validate(IContext context)
	{
		if (!x.Validate(context))
			AddError("Invalid argument expression at point definition", x);
		if (x.Type is not ExpType.Number)
			AddError("First argument must be a number, at point definition");
		if (!y.Validate(context))
			AddError("Invalid argument expression at point definition", y);
		if (y.Type is not ExpType.Number)
			AddError("Second argument must be a number, at point definition");

		return IsValid();
	}

	public override void Evaluate(IContext context)
	{
		x.Evaluate(context);
		float X = float.Parse(x.Value); 
		y.Evaluate(context);
		float Y = float.Parse(y.Value);
		Object = new Point(X, Y);
	}
}
