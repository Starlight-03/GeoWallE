using System.Collections.Generic;

public class LetIn : Expression
{
    private readonly List<Statement> statements;

    private readonly Expression body;

    public LetIn(List<Statement> statements, Expression body, int line) : base(line)
    {
        this.statements = statements;
        this.body = body;
    }

    public override bool Validate(IContext context)
    {
        IContext innerContext = context.CreateChildContext();

        foreach (var statement in statements)
            if (!statement.Validate(innerContext))
                AddError("");

        if (!body.Validate(innerContext))
            AddError("");

        return IsValid();
    }

    public override void Evaluate()
    {
        foreach (var statement in statements)
            statement.Evaluate();

        body.Evaluate();
        Value = body.Value;
    }
}