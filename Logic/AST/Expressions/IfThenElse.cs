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
            AddError("Invalid expression after \'if\' keyword", condition);
        if (!positive.Validate(context))
            AddError("Invalid expression after \'then\' keyword", positive);
        if (!negative.Validate(context))
            AddError("Invalid expression after \'else\' keyword", negative);
        if (positive.Type != negative.Type)
            AddError("\'else\' expression doesn't have the same type as \'then\' expression");
        else
            Type = positive.Type;
        
        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        condition.Evaluate(context);
        if (BooleanEvaluator.Evaluate(context, condition))
            Evaluate(positive);
        else
            Evaluate(negative);

        void Evaluate(Expression expression){
            expression.Evaluate(context);
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