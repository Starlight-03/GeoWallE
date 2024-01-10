using System.Collections.Generic;

public class FuncDef : Statement
{
    private readonly string identifier;

    private readonly List<string> args;
    
    private readonly Expression body;

    public FuncDef(int line, string identifier, List<string> args, Expression body) : base(line)
    {
        this.identifier = identifier;
        this.args = args;
        this.body = body;
    }

    public override bool Validate(IContext context)
    {
        var innerContext = context.CreateChildContext();

        foreach(string arg in args)
            innerContext.DefineVariable(arg, ExpType.Undefined);

        if (context.FunctionIsDefined(identifier, args.Count, out (ExpType, Expression) functionBody))
            AddError("");
        else
            context.DefineFunction(identifier, args.Count, ExpType.Undefined, body);

        if (!body.Validate(innerContext))
            AddError("");

        if (body.Type is not ExpType.Undefined)
            context.SetFunctionType(identifier, args.Count, body.Type);

        return IsValid();
    }

    public override void Evaluate() { }
}