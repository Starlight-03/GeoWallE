public abstract class Statement : ASTNode
{
    public Statement(int line) : base(line) { }

    public override void Evaluate(IContext context) { }
}