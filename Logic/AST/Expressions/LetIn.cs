using System.Collections.Generic;

public class LetIn : Expression
{
    private readonly List<Statement> statements;

    private readonly Expression body;

    public LetIn(int line, List<Statement> statements, Expression body) : base(line)
    {
        this.statements = statements;
        this.body = body;
    }

    public override bool Validate(IContext context)
    {
        IContext innerContext = context.CreateChildContext();

        foreach (var statement in statements)
            if (!statement.Validate(innerContext))
                AddError("Invalid statement at \'let-in\' expression", statement);

        if (!body.Validate(innerContext))
            AddError("Invalid argument as \'let-in\' body", body);

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        foreach (var statement in statements)
            statement.Evaluate(context);

        body.Evaluate(context);
        Value = body.Value;
    }
}