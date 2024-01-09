using System.Collections.Generic;
public class Draw : Statement
{
    public readonly List<GObject> Objects = new();

    public readonly string Text;

    private Expression expr;

    public Draw(Expression expr, Text nameTag, int line) : base(line)
    {
        this.expr = expr;
        Text = nameTag.Value;
    }

    public override bool Validate(IContext context)
    {
        if (!expr.Validate(context))
            AddError("");
        if (expr.Type is ExpType.Undefined)
            AddError("");
        if (expr.Type is ExpType.Sequence){
            if (expr is Concat) 
                expr = expr.Seq;
            if (expr is Sequence seq && (seq.valType is ExpType.Number || seq.valType is ExpType.Text))
                AddError("");
        }

        return IsValid();
    }

    public override void Evaluate()
    {
        expr.Evaluate();

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