using System;

public abstract class ArithmeticExpression : BinaryExpression
{
    protected float leftVal;

    protected float rightVal;

    protected ArithmeticExpression(Expression left, Expression right, int line) : base(left, right, line)
    => Type = ExpType.Number;

    public override bool Validate(IContext context)
    {
        if (!base.Validate(context))
            return false;

        if (left.Type != ExpType.Number && left.Type != ExpType.Measure)
            AddError("");
        if (right.Type != ExpType.Number && right.Type != ExpType.Measure)
            AddError("");

        if (left.Type == ExpType.Measure || right.Type == ExpType.Measure)
            Type = ExpType.Measure;

        return IsValid();
    }

    public override void Evaluate()
    {
        left.Evaluate();
        leftVal = float.Parse(left.Value);
        right.Evaluate();
        rightVal = float.Parse(right.Value);

        if (Type == ExpType.Measure){
            leftVal = MathF.Round(MathF.Abs(leftVal));
            rightVal = MathF.Round(MathF.Abs(rightVal));
        }
    }
}

public class Sum : ArithmeticExpression
{
    public Sum(Expression left, Expression right, int line) : base(left, right, line) => op = "+";

    public override void Evaluate()
    {
        base.Evaluate();
        float val = leftVal + rightVal;
        Value = (Type == ExpType.Measure) ? MathF.Abs(val).ToString() : val.ToString();
    }
}

public class Sub : ArithmeticExpression
{
    public Sub(Expression left, Expression right, int line) : base(left, right, line) => op = "-";

    public override void Evaluate()
    {
        base.Evaluate();
        float val = leftVal - rightVal;
        Value = (Type == ExpType.Measure) ? MathF.Abs(val).ToString() : val.ToString();
    }
}

public class Mul : ArithmeticExpression
{
    public Mul(Expression left, Expression right, int line) : base(left, right, line) => op = "*";

    public override void Evaluate()
    {
        base.Evaluate();
        float val = leftVal * rightVal;
        Value = (Type == ExpType.Measure) ? MathF.Abs(val).ToString() : val.ToString();
    }
}

public class Div : ArithmeticExpression
{
    public Div(Expression left, Expression right, int line) : base(left, right, line) => op = "/";

    public override void Evaluate()
    {
        base.Evaluate();
        float val = leftVal / rightVal;
        Value = (Type == ExpType.Measure) ? MathF.Abs(val).ToString() : val.ToString();
    }
}

public class Mod : ArithmeticExpression
{
    public Mod(Expression left, Expression right, int line) : base(left, right, line) => op = "%";

    public override void Evaluate()
    {
        base.Evaluate();
        float val = leftVal % rightVal;
        Value = (Type == ExpType.Measure) ? MathF.Abs(val).ToString() : val.ToString();
    }
}

public class Pow : ArithmeticExpression
{
    public Pow(Expression left, Expression right, int line) : base(left, right, line) => op = "^";

    public override void Evaluate()
    {
        base.Evaluate();
        float val = MathF.Pow(leftVal, rightVal);
        Value = (Type == ExpType.Measure) ? MathF.Abs(val).ToString() : val.ToString();
    }
}