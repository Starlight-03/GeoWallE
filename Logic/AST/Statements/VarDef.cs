public class VarDef : Statement
{
    private readonly string identifier;

    private readonly Expression expr;

    public VarDef(string identifier, Expression expr, int line) : base(line)
    {
        this.identifier = identifier;
        this.expr = expr;
    }

    public override bool Validate(IContext context)
    {
        if (context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue))
            AddError("");
        if (!expr.Validate(context))
            AddError("");

        context.DefineVariable(identifier, expr.Type, expr);
        return IsValid();
    }

    public override void Evaluate() { }
}