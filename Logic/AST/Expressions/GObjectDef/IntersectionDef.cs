using System;

public class IntersectionDef : Expression
{
    private readonly Expression f1;

    private readonly Expression f2;

    public IntersectionDef(Expression f1, Expression f2, int line) : base(line)
    {
        this.f1 = f1;
        this.f2 = f2;
    }

    public override bool Validate(IContext context)
    => f1.Validate(context) && f2.Validate(context);

    public override void Evaluate()
    {
        throw new NotImplementedException();
    }
}