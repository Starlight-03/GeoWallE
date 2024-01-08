using System;

public abstract class LogicExpression : BinaryExpression
{
    protected LogicExpression(Expression left, Expression right, int line) : base(left, right, line){}

    public override bool Validate(IContext context) => base.Validate(context);

    public override void Evaluate()
    {
        left.Evaluate();
        right.Evaluate();
        // Algo más
    }
}

public class And : LogicExpression
{
    public And(Expression left, Expression right, int line) : base(left, right, line) => op = "and";

    public override void Evaluate() => throw new NotImplementedException(); // Value = (double.Parse(left.Evaluate()) && double.Parse(right.Evaluate())).ToString();
}

public class Or : LogicExpression
{
    public Or(Expression left, Expression right, int line) : base(left, right, line) => op = "or";

    public override void Evaluate() => throw new NotImplementedException(); // Value = (double.Parse(left.Evaluate()) || double.Parse(right.Evaluate())).ToString();
}