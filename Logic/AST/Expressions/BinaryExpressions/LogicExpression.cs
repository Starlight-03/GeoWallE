using System;

public abstract class LogicExpression : BinaryExpression
{
    protected bool leftValue;

    protected bool rightValue;

    protected LogicExpression(int line, Expression left, Expression right) : base(line, left, right){}

    public override bool Validate(IContext context) => base.Validate(context);

    public override void Evaluate()
    {
        left.Evaluate();
        leftValue = BooleanEvaluator.Evaluate(left);
        right.Evaluate();
        rightValue = BooleanEvaluator.Evaluate(right);
    }
}

public class And : LogicExpression
{
    public And(int line, Expression left, Expression right) : base(line, left, right) => op = "and";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftValue && rightValue) ? "1" : "0";
    }
}

public class Or : LogicExpression
{
    public Or(int line, Expression left, Expression right) : base(line, left, right) => op = "or";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftValue || rightValue) ? "1" : "0";
    }
}

public static class BooleanEvaluator
{
    public static bool Evaluate(Expression expression)
    {
        expression.Evaluate();
        if (expression.Type is ExpType.Undefined 
            || (expression.Type is ExpType.Text && expression.Value is "")
            || (expression.Type is ExpType.Number && expression.Value is "0") 
            || (expression.Type is ExpType.Sequence && expression.Seq.Count is 0))
                return false;

        return true;
    }
}