public abstract class ComparisonExpression : BinaryExpression
{
    protected double leftVal;

    protected double rightVal;

    protected ComparisonExpression(Expression left, Expression right, int line) : base(left, right, line){}

    public override bool Validate(IContext context)
    {
        if (!base.Validate(context))
            return false;

        if (left.Type != ExpType.Number && left.Type != ExpType.Number) 
            AddError("");
        if (left.Type != ExpType.Measure && left.Type != ExpType.Measure)
            AddError("");

        return IsValid();
    }

    public override void Evaluate()
    {
        left.Evaluate();
        leftVal = double.Parse(left.Value);
        right.Evaluate();
        rightVal = double.Parse(right.Value);
    }
}

public class Minor : ComparisonExpression
{
    public Minor(Expression left, Expression right, int line) : base(left, right, line) => op = "<";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftVal < rightVal).ToString();
    }
}

public class MinorEqual : ComparisonExpression
{
    public MinorEqual(Expression left, Expression right, int line) : base(left, right, line) => op = "<=";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftVal <= rightVal).ToString();
    }
}

public class Major : ComparisonExpression
{
    public Major(Expression left, Expression right, int line) : base(left, right, line) => op = ">";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftVal > rightVal).ToString();
    }
}

public class MajorEqual : ComparisonExpression
{
    public MajorEqual(Expression left, Expression right, int line) : base(left, right, line) => op = ">=";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftVal >= rightVal).ToString();
    }
}

public class Equals : ComparisonExpression
{
    public Equals(Expression left, Expression right, int line) : base(left, right, line) => op = "==";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftVal == rightVal).ToString();
    }
}

public class NotEqual : ComparisonExpression
{
    public NotEqual(Expression left, Expression right, int line) : base(left, right, line) => op = "!=";

    public override void Evaluate()
    {
        base.Evaluate();
        Value = (leftVal != rightVal).ToString();
    }
}