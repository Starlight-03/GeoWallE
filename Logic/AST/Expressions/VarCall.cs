public class VarCall : Expression
{
    private readonly string identifier;

    private IContext context;

    public VarCall(int line, string identifier) : base(line) => this.identifier = identifier;

    public void SetVariableType(ExpType type)
    {
        Type = type;
        context.SetVariableType(identifier, type);
    }

    public override bool Validate(IContext context)
    {
        this.context = context;

        if (context.VariableIsDefined(identifier))
            Type = context.GetVariableType(identifier);
        else
            AddError($"Variable {identifier} has not been declared");

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        Expression val = context.GetVariableValue(identifier);
        if (val.Value is null && val.Seq is null && val.Object is null)
            val.Evaluate(context);
        if (val.Type is ExpType.Number || val.Type is ExpType.Text)
            Value = val.Value;
        else if (val.Type is ExpType.Sequence)
            Seq = (Sequence)val;
        else if (val.Type is not ExpType.Undefined)
            Object = val.Object;
    }
}