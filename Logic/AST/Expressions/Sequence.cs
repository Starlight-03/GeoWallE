using System.Collections.Generic;

public class Sequence : Expression
{
    public readonly List<Expression> Values;

    public ExpType valType;

    public Sequence(List<Expression> values, int line, ExpType valType = ExpType.Undefined) : base(line)
    {
        Values = values;
        this.valType = valType;
        Type = ExpType.Sequence;
    }

    public int Count => Values.Count;
    
    public Expression this[int index] { get => (index >= 0 || index < Count) ? Values[index] : null; }

    public override bool Validate(IContext context)
    {
        foreach (var item in Values){
            if (!item.Validate(context))
                return false;
            if (valType == ExpType.Undefined)
                valType = item.Type;
            else if (item.Type != valType)
                return false;
        }
        
        return true;
    }

    public override void Evaluate()
    {
        foreach (var item in Values)
            item.Evaluate();
    }

    public void Concat(Sequence other)
    {
        foreach (Expression item in other.Values)
            Values.Add(item);
    }
}