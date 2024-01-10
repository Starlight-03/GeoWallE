public class IfThenElse : Expression
{
    private readonly Expression condition;

    private readonly Expression positive;

    private readonly Expression negative;

    public IfThenElse(int line, Expression condition, Expression positive, Expression negative) : base(line)
    {
        this.condition = condition;
        this.positive = positive;
        this.negative = negative;
    }

    public override bool Validate(IContext context)
    {
        if (!condition.Validate(context))
            AddError("");
        if (!positive.Validate(context))
            AddError("");
        if (!negative.Validate(context))
            AddError("");
        if (positive.Type != negative.Type)
            AddError("");
        else
            Type = positive.Type;
        
        return IsValid();
    }

    public override void Evaluate()
    {
        condition.Evaluate();
        if (BooleanEvaluator.Evaluate(condition))
            Evaluate(positive);
        else
            Evaluate(negative);

        void Evaluate(Expression expression){
            expression.Evaluate();
            if (expression.Type is ExpType.Number || expression.Type is ExpType.Text 
                || expression.Type is ExpType.Measure)
                    Value = expression.Value;
            else if (expression.Type is ExpType.Sequence)
                Seq = expression.Seq;
            else 
                Object = expression.Object;
        }
    }
}