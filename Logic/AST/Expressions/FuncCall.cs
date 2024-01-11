using System.Collections.Generic;

public class FuncCall : Expression
{
    private readonly string identifier;

    private readonly List<Expression> args;

    public FuncCall(int line, string identifier, List<Expression> args) : base(line)
    {
        this.identifier = identifier;
        this.args = args;
    }

    public override bool Validate(IContext context)
    {
        List<string> argNames = context.GetFunctionArgs(identifier);

        if (!context.FunctionIsDefined(identifier, args.Count)){
            if (argNames is null)
                AddError($"Function {identifier} has not been defined");
            else if (args.Count != argNames.Count)
                AddError($"Function {identifier} receives {argNames.Count} arguments, but {args.Count} were given");
            return false;
        }

        IContext innerContext = context.GetInnerContext(identifier, args.Count);
        Expression body = context.GetFunctionBody(identifier, args.Count);
        foreach (var arg in argNames)
            innerContext.DefineVariable(arg);
        for (int i = 0; i < args.Count; i++){
            if (!args[i].Validate(context))
                AddError($"Invalid argument No.{i} at {identifier} function call", args[i]);
            var argType = innerContext.GetVariableType(argNames[i]);
            if (argType is not ExpType.NotSet && argType != args[i].Type)
                AddError($"Function \'{identifier}\' receives \'{argType}\', not \'{args[i].Type}\'");
        }
        Type = body.Type;
        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        List<string> argNames = context.GetFunctionArgs(identifier);
        Expression body = context.GetFunctionBody(identifier, args.Count);
        IContext innerContext = context.CreateChildContext();
        for (int i = 0; i < args.Count; i++){
            args[i].Evaluate(context);
            innerContext.DefineVariable(argNames[i], args[i].Type, args[i]);
        }
        body.Evaluate(innerContext);
        Value = body.Value;
    }
}