public abstract class BinaryExpression : Expression
{
    protected string op;

    protected readonly Expression left;

    protected readonly Expression right;

    public BinaryExpression(int line, Expression left, Expression right) : base(line)
    {
        op = "";
        this.left = left;
        this.right = right;
    }

    public override bool Validate(IContext context)
    {
        if (!left.Validate(context))
            AddError("");
        if (!right.Validate(context))
            AddError("");

        return IsValid();
    }
}

public class Concat : BinaryExpression
{
    public Concat(int line, Expression left, Expression right) : base(line, left, right) 
    => Type = ExpType.Sequence;

    public override bool Validate(IContext context)
    {
        if (!base.Validate(context))
            return false;
        
        if (left.Type != ExpType.Sequence && left.Type != ExpType.Undefined)
            AddError("");
        if (right.Type != ExpType.Sequence && right.Type != ExpType.Undefined)
            AddError("");
        
        if (left.Type == ExpType.Undefined) 
            Type = ExpType.Undefined;
        
        return IsValid();
    }

    public override void Evaluate()
    {
        if (Type == ExpType.Sequence){
            left.Evaluate();
            Seq = (Sequence)left;
            if (right.Type == ExpType.Sequence){
                right.Evaluate();
                Sequence other = (Sequence)right;
                Seq.Concat(other);
            }
        }
    }
}