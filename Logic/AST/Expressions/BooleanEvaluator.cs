public static class BooleanEvaluator // Tool
{
    public static bool Evaluate(IContext context, Expression expression)
    {
        expression.Evaluate(context);
        if (expression.Type is ExpType.Undefined 
            || (expression.Type is ExpType.Text && expression.Value is "")
            || (expression.Type is ExpType.Number && expression.Value is "0") 
            || (expression.Type is ExpType.Sequence && expression.Seq.Count is 0))
                return false;

        return true;
    }
}