using System.Collections.Generic;

public class VarDecl : Statement
{
    protected readonly string identifier;

    protected readonly ExpType type;

    public VarDecl(ExpType type, string identifier, int line) : base(line)
    {
        this.type = type;
        this.identifier = identifier;
    }

    public override bool Validate(IContext context)
    {
        if (context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue))
            AddError("");

        context.DefineVariable(identifier, type);
        return IsValid();
    }
}

public class SequenceDecl : VarDecl
{
    private readonly ExpType valType;

    public SequenceDecl(ExpType valType, string identifier, int line) : base(ExpType.Sequence, identifier, line)
    => this.valType = valType;

    public override bool Validate(IContext context)
    {
        if (!context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue))
            variableValue.Item2 = new Sequence(new List<Expression>(), 0, valType);
        else
            AddError("");

        context.DefineVariable(identifier, type);
        return IsValid();
    }
}