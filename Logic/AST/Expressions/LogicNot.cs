public class LogicNot : Expression
{
    private readonly Expression expression;

    public LogicNot(int line, Expression expression) : base(line)
    {
        Type = ExpType.Number;
        this.expression = expression;
    }

    public override bool Validate(IContext context)
    {
        if (!expression.Validate(context))
            AddError("Invalid expression after \'not\' operator", expression);
        
        return IsValid();
    }

    public override void Evaluate(IContext context) => Value = BooleanEvaluator.Evaluate(context, expression) ? "1" : "0";
}