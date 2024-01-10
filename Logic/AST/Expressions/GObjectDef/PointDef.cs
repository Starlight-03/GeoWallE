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
	=> x is not null && x.Validate(context) && x.Type == ExpType.Number 
		&& y is not null && y.Validate(context) && y.Type == ExpType.Number;

	public override void Evaluate()
	{
		x.Evaluate();
		float X = float.Parse(x.Value); 
		y.Evaluate();
		float Y = float.Parse(y.Value);
		Object = new Point(X, Y);
	}
}
