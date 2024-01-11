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

    protected void AddError() => AddError($"Operator {op} cannot be used with {left.Type} and {right.Type}");

    public override bool Validate(IContext context)
    {
        if (!left.Validate(context))
            AddError($"Invalid expression at left of operator {op}", left);
        if (!right.Validate(context))
            AddError($"Invalid expression at right of operator {op}", right);

        if (left is VarCall leftCall && leftCall.Type is ExpType.NotSet && right.Type is not ExpType.NotSet)
            leftCall.SetVariableType(right.Type);
        if (right is VarCall rightCall && rightCall.Type is ExpType.NotSet && left.Type is not ExpType.NotSet)
            rightCall.SetVariableType(left.Type);

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
        
        if ((left.Type != ExpType.Sequence && left.Type != ExpType.Undefined) 
            || (right.Type != ExpType.Sequence && right.Type != ExpType.Undefined))
                AddError();

        if (left.Type is ExpType.Undefined) 
            Type = ExpType.Undefined;

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        if (Type == ExpType.Sequence){
            left.Evaluate(context);
            Seq = (Sequence)left;
            if (right.Type == ExpType.Sequence){
                right.Evaluate(context);
                Sequence other = (Sequence)right;
                Seq.Concat(other);
            }
        }
    }
}