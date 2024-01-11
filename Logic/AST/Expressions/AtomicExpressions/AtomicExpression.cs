public abstract class AtomicExpression : Expression
{
    protected AtomicExpression() : base(0) { }

    public override bool Validate(IContext context) => true;

    public override void Evaluate(IContext context) { }
}