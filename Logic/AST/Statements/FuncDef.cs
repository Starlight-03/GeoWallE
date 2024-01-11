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
            innerContext.DefineVariable(arg);

        if (context.FunctionIsDefined(identifier, args.Count))
            AddError($"Function {identifier} is already defined");
        else
            context.DefineFunction(identifier, args, body);

        if (!body.Validate(innerContext))
            AddError("Invalid expression as a body of a function", body);

        return IsValid();
    }

    public override void Evaluate(IContext context) { }
}