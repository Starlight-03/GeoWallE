public class VarCall : Expression
{
    private readonly string identifier;

    private IContext context;

    public VarCall(string identifier, int line) : base(line) => this.identifier = identifier;

    public override bool Validate(IContext context)
    {
        this.context = context;

        if (context.VariableIsDefined(identifier, out (ExpType, Expression) variableValue))
            Type = variableValue.Item1;
        else
            AddError("");

        return IsValid();
    }

    public override void Evaluate()
    {
        Expression val = context.GetVariableValue(identifier);
        val.Evaluate();
        if (val.Type is ExpType.Number || val.Type is ExpType.Text)
            Value = val.Value;
        else if (val.Type is ExpType.Sequence)
            Seq = (Sequence)val;
        else if (val.Type is not ExpType.Undefined)
            Object = val.Object;
    }
}