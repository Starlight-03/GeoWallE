public class Number : AtomicExpression
{
    public Number(string value)
    {
        Value = value;
        Type = ExpType.Number;
    }

    public Number(float value)
    {
        Value = value.ToString();
        Type = ExpType.Number;
    }
}