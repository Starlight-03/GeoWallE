using System;

public class LogicNot : Expression
{
    private readonly Expression expression;

    public LogicNot(Expression expression, int line) : base(line) => this.expression = expression;

    public override bool Validate(IContext context)
    {
        if (!expression.Validate(context))
            AddError("");
        
        return IsValid();
    }

    public override void Evaluate()
    {
        expression.Evaluate();
        throw new NotImplementedException();
    }
}