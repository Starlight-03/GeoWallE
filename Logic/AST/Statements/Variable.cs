using System.Collections.Generic;

public class VarDecl : Statement
{
    protected readonly ExpType type;

    protected readonly string identifier;

    protected readonly VarDef def;

    public VarDecl(int line, ExpType type, string identifier, Expression arg) : base(line)
    {
        this.type = type;
        this.identifier = identifier;
        def = (arg is not null) ? new VarDef(line, identifier, arg) : null;
    }

    public override bool Validate(IContext context)
    {
        if (context.VariableIsDefined(identifier))
            AddError($"Variable {identifier} is already defined");
        else
            context.DefineVariable(identifier, type);

        if (def is not null && !def.Validate(context))
            AddError($"Invalid expression at variable {identifier} declaration", def);

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        if (def is not null)
            def.Evaluate(context);
    }
}

public class SequenceDecl : VarDecl
{
    private readonly ExpType valType;

    public SequenceDecl(int line, ExpType valType, string identifier, Expression arg) : base(line, ExpType.Sequence, identifier, arg)
    => this.valType = valType;

    public override bool Validate(IContext context)
    {
        if (context.VariableIsDefined(identifier))
            AddError($"Variable {identifier} is already defined");
        else{
            context.DefineVariable(identifier, type);
            if (def is not null)
                context.SetVariableValue(identifier, new Sequence(0, new List<Expression>(), valType));
        }

        if (def is not null){
            if (!def.Validate(context))
                AddError($"Invalid expression at variable {identifier} declaration", def);
            else if (def.Expr.Seq.ValType != valType)
                AddError($"Expression must be a sequence at variable {identifier} declaration");
        }

        return IsValid();
    }
}

public class VarDef : Statement
{
    public Expression Expr { get; private set; }

    private readonly string identifier;

    private IContext context;

    public VarDef(int line, string identifier, Expression Expr) : base(line)
    {
        this.identifier = identifier;
        this.Expr = Expr;
    }

    public override bool Validate(IContext context)
    {
        this.context = context;

        if (!Expr.Validate(context))
            AddError($"Invalid expression in variable {identifier} definition", Expr);

        if (context.VariableIsDefined(identifier)){
            if (context.GetVariableValue(identifier) is not null)
                AddError($"Variable {identifier} is already defined");
            else{
                ExpType type = context.GetVariableType(identifier);
                if (type != Expr.Type)
                    AddError($"Variable {identifier} has been declared as {type}, not {Expr.Type}");
                else
                    context.SetVariableValue(identifier, Expr);
            }
        }
        else
            context.DefineVariable(identifier, Expr.Type, Expr);

        return IsValid();
    }

    public override void Evaluate(IContext context) => context.GetVariableValue(identifier).Evaluate(context);
}