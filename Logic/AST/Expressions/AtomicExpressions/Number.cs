public class Number : AtomicExpression
{
    public Number(string value) : base()
    {
        Value = value;
        Type = ExpType.Number;
    }
}