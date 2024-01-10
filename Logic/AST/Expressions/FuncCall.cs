using System.Collections.Generic;

public class FuncCall : Expression
{
    private readonly string identifier;

    private readonly List<Expression> args;

    private IContext context;

    public FuncCall(int line, string identifier, List<Expression> args) : base(line)
    {
        this.identifier = identifier;
        this.args = args;
    }

    public override bool Validate(IContext context)
    {
        this.context = context;

        foreach (var expr in args)
            if (!expr.Validate(context))
                return false;

        if (context.FunctionIsDefined(identifier, args.Count, out (ExpType, Expression) functionBody))
            Type = functionBody.Item1;
        else
            AddError("");

        return IsValid();
    }

    public override void Evaluate()
    {
        foreach (var expr in args)
            expr.Evaluate();

        Expression body = context.GetFunctionBody(identifier, args.Count);
        body.Evaluate();
        Value = body.Value;
    }
}