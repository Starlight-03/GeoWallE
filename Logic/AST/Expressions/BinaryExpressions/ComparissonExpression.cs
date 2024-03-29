public abstract class ComparisonExpression : BinaryExpression
{
    protected float leftVal;

    protected float rightVal;

    protected ComparisonExpression(int line, Expression left, Expression right) : base(line, left, right)
    => Type = ExpType.Number;

    public override bool Validate(IContext context)
    {
        if (!base.Validate(context))
            return false;

        if ((left.Type is ExpType.Number && right.Type is not ExpType.Number) 
            || (left.Type is ExpType.Measure && right.Type is not ExpType.Measure))
                AddError();

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        left.Evaluate(context);
        float leftValue = float.Parse(left.Value);
        right.Evaluate(context);
        leftVal = leftValue;
        rightVal = float.Parse(right.Value);
    }
}

public class Minor : ComparisonExpression
{
    public Minor(int line, Expression left, Expression right) : base(line, left, right) => op = "<";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal < rightVal) ? "1" : "0";
    }
}

public class MinorEqual : ComparisonExpression
{
    public MinorEqual(int line, Expression left, Expression right) : base(line, left, right) => op = "<=";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal <= rightVal) ? "1" : "0";
    }
}

public class Major : ComparisonExpression
{
    public Major(int line, Expression left, Expression right) : base(line, left, right) => op = ">";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal > rightVal) ? "1" : "0";
    }
}

public class MajorEqual : ComparisonExpression
{
    public MajorEqual(int line, Expression left, Expression right) : base(line, left, right) => op = ">=";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal >= rightVal) ? "1" : "0";
    }
}

public class Equals : ComparisonExpression
{
    public Equals(int line, Expression left, Expression right) : base(line, left, right) => op = "==";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal == rightVal) ? "1" : "0";
    }
}

public class NotEqual : ComparisonExpression
{
    public NotEqual(int line, Expression left, Expression right) : base(line, left, right) => op = "!=";

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal != rightVal) ? "1" : "0";
    }
}