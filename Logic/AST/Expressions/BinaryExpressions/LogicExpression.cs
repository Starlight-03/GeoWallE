using System;

public abstract class LogicExpression : BinaryExpression
{
    protected LogicExpression(int line, Expression left, Expression right) : base(line, left, right){}

    public override bool Validate(IContext context) => base.Validate(context);

    public override void Evaluate()
    {
        left.Evaluate();
        right.Evaluate();
        // ! Algo más
    }
}

public class And : LogicExpression
{
    public And(int line, Expression left, Expression right) : base(line, left, right) => op = "and";

    public override void Evaluate() => throw new NotImplementedException(); // Value = (double.Parse(left.Evaluate()) && double.Parse(right.Evaluate())).ToString();
}

public class Or : LogicExpression
{
    public Or(int line, Expression left, Expression right) : base(line, left, right) => op = "or";

    public override void Evaluate() => throw new NotImplementedException(); // Value = (double.Parse(left.Evaluate()) || double.Parse(right.Evaluate())).ToString();
}