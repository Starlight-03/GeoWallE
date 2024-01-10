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
        if (context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue))
            AddError("");
        else
            context.DefineVariable(identifier, type);

        if (def is not null && !def.Validate(context))
            AddError("");

        return IsValid();
    }
}

public class SequenceDecl : VarDecl
{
    private readonly ExpType valType;

    public SequenceDecl(int line, ExpType valType, string identifier, Expression arg) : base(line, ExpType.Sequence, identifier, arg)
    => this.valType = valType;

    public override bool Validate(IContext context)
    {
        if (context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue))
            AddError("");
        else{
            context.DefineVariable(identifier, type);
            if (def is not null)
                variableValue.Item2 = new Sequence(0, new List<Expression>(), valType);
        }

        if (def is not null){
            if (!def.Validate(context))
                AddError("");
            else if (def.Expr.Seq.ValType != valType)
                AddError("");
        }

        return IsValid();
    }
}

public class VarDef : Statement
{
    public Expression Expr { get; private set; }

    private readonly string identifier;

    public VarDef(int line, string identifier, Expression Expr) : base(line)
    {
        this.identifier = identifier;
        this.Expr = Expr;
    }

    public override bool Validate(IContext context)
    {
        if (!Expr.Validate(context))
            AddError("");

        if (context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue)){
            if (variableValue.Item2 is not null)
                AddError("");
            else{
                if (variableValue.Item1 != Expr.Type)
                    AddError("");
                else
                    context.SetVariableValue(identifier, Expr);
            }
        }
        else
            context.DefineVariable(identifier, Expr.Type, Expr);

        return IsValid();
    }

    public override void Evaluate() { }
}