using System;

public class IfThenElse : Expression
{
    private readonly Expression condition;

    private readonly Expression positive;

    private readonly Expression negative;

    public IfThenElse(Expression condition, Expression positive, Expression negative, int line) : base(line)
    {
        this.condition = condition;
        this.positive = positive;
        this.negative = negative;
    }

    public override bool Validate(IContext context) => condition.Validate(context) 
                                                        && positive.Validate(context)
                                                        && negative.Validate(context);

    public override void Evaluate()
    {
        throw new NotImplementedException();
    }
}