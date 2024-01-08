public class Text : AtomicExpression
{
    public Text(string text) : base()
    {
        Value = text;
        Type = ExpType.Text;
    }
}