using System;

public abstract class ArithmeticExpression : BinaryExpression
{
    protected float leftVal;

    protected float rightVal;

    protected ArithmeticExpression(int line, Expression left, Expression right) : base(line, left, right) { }

    public override bool Validate(IContext context)
    {
        if (!base.Validate(context))
            return false;

        if ((left.Type is not ExpType.Number && left.Type is not ExpType.Measure) 
            || (right.Type is not ExpType.Number && right.Type is not ExpType.Measure))
                AddError();

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        left.Evaluate(context);
        leftVal = float.Parse(left.Value);
        right.Evaluate(context);
        rightVal = float.Parse(right.Value);

        if (Type is ExpType.Measure){
            leftVal = MathF.Round(MathF.Abs(leftVal));
            rightVal = MathF.Round(MathF.Abs(rightVal));
        }
    }
}

public class Sum : ArithmeticExpression
{
    public Sum(int line, Expression left, Expression right) : base(line, left, right) => op = "+";

    public override bool Validate(IContext context)
    {
        base.Validate(context);

        if (left.Type is ExpType.Number){
            if (right.Type is not ExpType.Number)
                AddError();
            else
                Type = ExpType.Number;
        }
        if (left.Type is ExpType.Measure){
            if (right.Type is not ExpType.Measure)
                AddError();
            else
                Type = ExpType.Measure;
        }

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        float val = leftVal + rightVal;
        Value = (Type is ExpType.Measure) ? Math.Round(MathF.Abs(val)).ToString() : val.ToString();
    }
}

public class Sub : ArithmeticExpression
{
    public Sub(int line, Expression left, Expression right) : base(line, left, right) => op = "-";

    public override bool Validate(IContext context)
    {
        base.Validate(context);

        if (left.Type is ExpType.Number){
            if (right.Type is not ExpType.Number)
                AddError();
            else
                Type = ExpType.Number;
        }
        if (left.Type is ExpType.Measure){
            if (right.Type is not ExpType.Measure)
                AddError();
            else
                Type = ExpType.Measure;
        }

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        float val = leftVal - rightVal;
        Value = (Type is ExpType.Measure) ? Math.Round(MathF.Abs(val)).ToString() : val.ToString();
    }
}

public class Mul : ArithmeticExpression
{
    public Mul(int line, Expression left, Expression right) : base(line, left, right) => op = "*";

    public override bool Validate(IContext context)
    {
        base.Validate(context);

        if ((left.Type is ExpType.Measure && right.Type is not ExpType.Number) 
            || (right.Type is ExpType.Measure && left.Type is not ExpType.Number))
                AddError();
        
        Type = (left.Type is ExpType.Measure || right.Type is ExpType.Measure) ? ExpType.Measure : ExpType.Number;

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        float val = leftVal * rightVal;
        Value = (Type is ExpType.Measure) ? Math.Round(MathF.Abs(val)).ToString() : val.ToString();
    }
}

public class Div : ArithmeticExpression
{
    public Div(int line, Expression left, Expression right) : base(line, left, right) => op = "/";

    public override bool Validate(IContext context)
    {
        base.Validate(context);

        if ((left.Type is ExpType.Measure && right.Type is not ExpType.Measure) 
            || (right.Type is ExpType.Measure && left.Type is not ExpType.Measure))
                AddError();
        
        Type = (left.Type is ExpType.Measure && right.Type is ExpType.Measure) ? ExpType.Number : ExpType.Measure;

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        float val = leftVal / rightVal;
        Value = (Type is ExpType.Measure) ? Math.Round(MathF.Abs(val)).ToString() : val.ToString();
    }
}

public class Mod : ArithmeticExpression
{
    public Mod(int line, Expression left, Expression right) : base(line, left, right)
    {
        op = "%";
        Type = ExpType.Number;
    }

    public override bool Validate(IContext context)
    {
        base.Validate(context);

        if (left.Type is not ExpType.Number || right.Type is not ExpType.Number)
            AddError();

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = (leftVal % rightVal).ToString();
    }
}

public class Pow : ArithmeticExpression
{
    public Pow(int line, Expression left, Expression right) : base(line, left, right)
    {
        op = "^";
        Type = ExpType.Number;
    }

    public override bool Validate(IContext context)
    {
        base.Validate(context);

        if (left.Type is not ExpType.Number || right.Type is not ExpType.Number)
            AddError();

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        base.Evaluate(context);
        Value = MathF.Pow(leftVal, rightVal).ToString();
    }
}