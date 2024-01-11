public abstract class LogicExpression : BinaryExpression
{
    protected bool leftValue;

    protected bool rightValue;

    protected LogicExpression(int line, Expression left, Expression right) : base(line, left, right){}

    public override bool Validate(IContext context) => base.Validate(context);

    public override void Evaluate(IContext context)
    {
        left.Evaluate(context);
        bool leftVal = BooleanEvaluator.Evaluate(context, left);
        right.Evaluate(context);
        leftValue = leftVal;
        rightValue = BooleanEvaluator.Evaluate(context, right);
    }
}

public class And : LogicExpression
{
    public And(int line, Expression left, Expression right) : base(line, left, right) => op = "and";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftValue && rightValue) ? "1" : "0";
    }
}

public class Or : LogicExpression
{
    public Or(int line, Expression left, Expression right) : base(line, left, right) => op = "or";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftValue || rightValue) ? "1" : "0";
    }
}