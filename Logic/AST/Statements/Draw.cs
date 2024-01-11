using System.Collections.Generic;

public class Draw : Statement
{
    public readonly List<GObject> Objects = new();

    public readonly string Text;

    private Expression expr;

    public Draw(int line, Expression expr, Text nameTag) : base(line)
    {
        this.expr = expr;
        Text = nameTag.Value;
    }

    public override bool Validate(IContext context)
    {
        if (!expr.Validate(context))
            AddError("Invalid expression at draw statement", expr);
        if (expr.Type is ExpType.Undefined)
            AddError("Cannot draw an undefined object");
        if (expr.Type is ExpType.Sequence){
            if (expr is Concat) 
                expr = expr.Seq;
            if (expr is Sequence seq && seq.ValType is ExpType.Number)
                AddError("Cannot draw a number");
        }
        if (expr.Type is ExpType.Number)
            AddError("Cannot draw a number");

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        expr.Evaluate(context);

        if (expr is Sequence sequence){
            foreach (var item in sequence.Values)
                if (item.Object is GObject gObject)
                    Objects.Add(gObject);
        }
        else
            Objects.Add(expr.Object);
        
        if (Text is not "")
            Objects.Add(new TextDraw(Objects[^1].Coordinates, Text));
    }
}